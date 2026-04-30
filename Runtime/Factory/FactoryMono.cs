using UnityEngine;

namespace CupkekGames.Pool.Factory
{
  public abstract class FactoryMono<T> : MonoBehaviour, IFactory<T>
  {
    public abstract T Create();
  }
}