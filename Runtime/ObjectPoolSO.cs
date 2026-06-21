using System;
using UnityEngine;

namespace CupkekGames.Pool
{
  public abstract class ObjectPoolSO<T> : ScriptableObject, IObjectPool<T> where T : class
  {
    [SerializeField] private bool prewarm = true;
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 20;
    [SerializeField] private UnityEngine.Pool.ObjectPool<T> pool;

    // Events from interface
    public event Action<T> OnCreateEvent;
    public event Action<T> OnTakeFromPoolEvent;
    public event Action<T> OnReturnToPoolEvent;
    public event Action<T> OnDestroyObjectEvent;

    /// <summary>
    /// Raw Unity pool. Prefer <see cref="Get"/> / <see cref="Release"/> —
    /// they skip destroyed instances, which the raw pool can hand out.
    /// </summary>
    public UnityEngine.Pool.ObjectPool<T> Pool => pool;

    /// <summary>
    /// Take an instance, skipping destroyed ones (see
    /// <see cref="ObjectPoolBase{T}.Get"/> for the rationale).
    /// </summary>
    public T Get()
    {
      while (true)
      {
        T instance = pool.Get();
        if (instance is UnityEngine.Object obj && obj == null)
        {
          continue;
        }
        return instance;
      }
    }

    /// <summary>Return an instance; destroyed instances are ignored.</summary>
    public void Release(T instance)
    {
      if (instance == null) return;
      if (instance is UnityEngine.Object obj && obj == null) return;
      pool.Release(instance);
    }

    public void Init()
    {
      pool = new UnityEngine.Pool.ObjectPool<T>(CreatePooledObjectInternal, OnTakeFromPoolInternal, OnReturnToPoolInternal, OnDestroyObjectInternal, collectionCheck, defaultCapacity, maxSize);

      if (prewarm)
      {
        Prewarm();

        // Debug.Log("Created ScriptableObject pool for " + typeof(T) + " with " + Pool.CountAll + " objects");
      }
    }

    public void Prewarm()
    {
      T[] instances = new T[defaultCapacity];
      for (int i = 0; i < defaultCapacity; i++)
      {
        instances[i] = Get();
      }
      for (int i = 0; i < defaultCapacity; i++)
      {
        Release(instances[i]);
      }
    }

    private T CreatePooledObjectInternal()
    {
      T instance = CreatePooledObject();
      OnCreateEvent?.Invoke(instance);
      return instance;
    }

    private void OnTakeFromPoolInternal(T instance)
    {
      OnTakeFromPool(instance);
      OnTakeFromPoolEvent?.Invoke(instance);
    }

    private void OnReturnToPoolInternal(T instance)
    {
      OnReturnToPool(instance);
      OnReturnToPoolEvent?.Invoke(instance);
    }

    private void OnDestroyObjectInternal(T instance)
    {
      OnDestroyObject(instance);
      OnDestroyObjectEvent?.Invoke(instance);
    }

    public abstract T CreatePooledObject();

    public abstract void OnTakeFromPool(T Instance);

    public abstract void OnReturnToPool(T Instance);

    public abstract void OnDestroyObject(T Instance);
  }
}