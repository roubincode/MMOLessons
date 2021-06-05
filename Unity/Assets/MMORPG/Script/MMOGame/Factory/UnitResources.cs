using System.Collections.Generic;
using UnityEngine;
using ETModel;
namespace MMOGame
{
    public static class UnitResources
    {
        public static GameObject Get(string type)
        {
	        ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
	          
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
	        GameObject prefab = bundleGameObject.Get<GameObject>($"{type}");

            return prefab;
        }

        public static GameObject[] GetAll(string type)
        {
	        ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{type}.unity3d");

            //加载unit预设并生成实例
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
	        UnityEngine.Object[] prefabs = bundleGameObject.GetAll<GameObject>();
            

            List<GameObject> list = new List<GameObject>();
            foreach (GameObject go in prefabs)
            {
                list.Add(go);
            }
            return list.ToArray();
        }

    }
}