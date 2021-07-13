using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace MMOGame
{
    public static class UnitResources
    {
        public static GameObject Get(string type)
        {
            ResourcesComponent rc = Game.Scene.GetComponent<ResourcesComponent>();

            GameObject bundleGOB = rc.GetAsset("unit.unity3d", "Unit") as GameObject;

            GameObject prefab = bundleGOB.Get<GameObject>($"{type}");

            return prefab;
        }
        public static GameObject[] GetAll(string bundleName){
            ResourcesComponent rc = Game.Scene.GetComponent<ResourcesComponent>();

            rc.LoadBundle($"{bundleName}");

            GameObject bundleGOB = rc.GetAsset($"{bundleName}","Unit") as GameObject;

            UnityEngine.Object[] prefabs =bundleGOB.GetAll<GameObject>();

            GameObject[] tmp = ConvertTo<GameObject,UnityEngine.Object>(prefabs);

            return tmp;  

        }
        public static T[] ConvertTo<T, K>(K[] k) where T : class,K,new()
        {
            List<T> tL = new List<T>();
            foreach (var item in k)
            {
                tL.Add(item as T);
            }
            T[] tA;

            tA = tL.ToArray();

            return tA;
        }
    }
}