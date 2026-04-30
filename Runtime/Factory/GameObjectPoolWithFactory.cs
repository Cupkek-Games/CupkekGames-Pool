using UnityEngine;
using CupkekGames.Pool;

namespace CupkekGames.Pool.Factory
{
  public class GameObjectPoolWithFactory : GameObjectPoolBase
  {
    private FactoryMonoGameObject _factory;
    public GameObjectPoolWithFactory(FactoryMonoGameObject factory, int defaultCapacity, int maxSize, bool prewarm = true, bool collectionCheck = true) : base(defaultCapacity, maxSize, collectionCheck)
    {
      _factory = factory;

      if (prewarm)
      {
        Prewarm();

        // Debug.Log("Created GameObject pool for " + typeof(GameObject) + " with " + Pool.CountAll + " objects");
      }
    }

    public override GameObject CreateObject()
    {
      return _factory.Create();
    }
  }
}