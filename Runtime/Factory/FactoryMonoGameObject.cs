using UnityEngine;

namespace CupkekGames.Pool.Factory
{
  public class FactoryMonoGameObject : FactoryMono<GameObject>
  {
    [SerializeField] private GameObject[] _prefabs;
    private int _index = 0;

    public override GameObject Create()
    {
      GameObject instance = Instantiate(_prefabs[_index], transform);
      _index = (_index + 1) % _prefabs.Length;

      instance.SetActive(false);

      return instance;
    }
  }
}