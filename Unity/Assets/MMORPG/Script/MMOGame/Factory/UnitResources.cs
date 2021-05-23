using UnityEngine;
using ETModel;
namespace MMOGame
{
    public static class UnitResources
    {
        public static UnityEngine.Object[] GetAll()
        {
	        ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle("unit.unity3d");
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset("unit.unity3d", "Unit");

	        UnityEngine.Object[] prefabs = bundleGameObject.GetAll<GameObject>();
            return prefabs;
        }

    }
}