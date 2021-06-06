using System.Collections.Generic;
using UnityEngine;
using ETModel;
namespace MMOGame
{
    public static class UIResources
    {
        public static UI Get(string type)
        {
	        //加载AB包
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{type}.unity3d");

            //加载界面预设并生成实例
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
            GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);

            //设置UI层级，只有UI摄像机可以渲染
            go.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = ComponentFactory.Create<UI, GameObject>(go);
	        
            return ui;
        }

        public static UI[] GetAll(string type)
        {
	        ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{type}.unity3d");

            //加载界面预设并生成实例
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
	        UnityEngine.Object[] prefabs = bundleGameObject.GetAll<GameObject>();

            List<UI> list = new List<UI>();
            foreach (GameObject go in prefabs)
            {
                //设置UI层级，只有UI摄像机可以渲染
                go.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(go);
                list.Add(ui);
            }

            return list.ToArray();
        }

    }
}