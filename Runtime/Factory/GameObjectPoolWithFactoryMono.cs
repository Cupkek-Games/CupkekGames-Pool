using UnityEngine;
using CupkekGames.Pool;

namespace CupkekGames.Pool.Factory
{
  public class GameObjectPoolWithFactoryMono : MonoBehaviour
  {
    [SerializeField] private FactoryMonoGameObject _factory;
    [SerializeField] private int _defaultCapacity = 5;
    [SerializeField] private int _maxSize = 20;
    private GameObjectPoolWithFactory _pool;
    private void Awake()
    {
      _pool = new GameObjectPoolWithFactory(_factory, _defaultCapacity, _maxSize);
    }

    public GameObject Spawn()
    {
      return _pool.Pool.Get();
    }
  }
}