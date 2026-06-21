using System;
using UnityEngine;
using UnityEngine.Pool;

namespace CupkekGames.Pool
{
  public abstract class ObjectPoolBase<T> : IObjectPool<T> where T : class
  {
    private bool _collectionCheck = true;
    private int _defaultCapacity;
    private int _maxSize;
    private ObjectPool<T> pool;

    // Events from interface
    public event Action<T> OnCreateEvent;
    public event Action<T> OnTakeFromPoolEvent;
    public event Action<T> OnReturnToPoolEvent;
    public event Action<T> OnDestroyObjectEvent;

    /// <summary>
    /// Raw Unity pool. Prefer <see cref="Get"/> / <see cref="Release"/> —
    /// they skip destroyed instances, which the raw pool can hand out
    /// (a pooled object destroyed externally stays in the internal list;
    /// <see cref="ObjectPool{T}"/> has no removal API).
    /// </summary>
    public ObjectPool<T> Pool => pool;

    /// <summary>
    /// Take an instance, skipping destroyed ones. A pooled UnityEngine.Object
    /// can die while inactive in the pool (scene unload, direct Destroy) —
    /// popped corpses are dropped and the next instance is taken instead;
    /// the pool creates a fresh one when it runs dry, so this always
    /// returns a live instance.
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

    /// <summary>
    /// Return an instance to the pool. A destroyed instance is ignored —
    /// a corpse must never re-enter the pool.
    /// </summary>
    public void Release(T instance)
    {
      if (instance == null) return;
      if (instance is UnityEngine.Object obj && obj == null) return;
      pool.Release(instance);
    }

    public ObjectPoolBase(int defaultCapacity, int maxSize, bool collectionCheck = true)
    {
      _collectionCheck = collectionCheck;
      _defaultCapacity = defaultCapacity;
      _maxSize = maxSize;

      pool = new ObjectPool<T>(CreatePooledObjectInternal, OnTakeFromPoolInternal, OnReturnToPoolInternal, OnDestroyObjectInternal, _collectionCheck, _defaultCapacity, _maxSize);
    }

    public void Prewarm()
    {
      T[] instances = new T[_defaultCapacity];
      for (int i = 0; i < _defaultCapacity; i++)
      {
        instances[i] = Get();
      }
      for (int i = 0; i < _defaultCapacity; i++)
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