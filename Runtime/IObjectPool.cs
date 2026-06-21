using System;

namespace CupkekGames.Pool
{
  public interface IObjectPool<T>
  {
    // Events
    event Action<T> OnCreateEvent;
    event Action<T> OnTakeFromPoolEvent;
    event Action<T> OnReturnToPoolEvent;
    event Action<T> OnDestroyObjectEvent;

    // Methods
    /// <summary>Take a live instance (destroyed pooled instances are skipped).</summary>
    T Get();

    /// <summary>Return an instance; destroyed instances are ignored.</summary>
    void Release(T instance);

    T CreatePooledObject();

    void OnTakeFromPool(T Instance);

    void OnReturnToPool(T Instance);

    void OnDestroyObject(T Instance);
  }
}