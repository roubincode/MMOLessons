using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 游戏物体工厂的基类
/// </summary>
public class BaseFactory : IBaseFactory
{
    protected Dictionary<string, GameObject> factoryDict = new Dictionary<string, GameObject>(); // 游戏物体资源(预制体)的字典
    protected Dictionary<string, Stack<GameObject>> objectPoolDict = new Dictionary<string, Stack<GameObject>>(); // 对象池字典
    protected string loadPath; // 加载路径

    public BaseFactory()
    {
        loadPath = "Prefabs/";
    }

    // 放入对象池的方法
    public void PushItem(string itemName, GameObject item)
    {
        item.SetActive(false); // 将要放入对象池的游戏物体失效
        item.transform.SetParent(RPGManager.Instance.Item);

        if (objectPoolDict.ContainsKey(itemName)) // 如果存在该对象池才放入(安全校验)
        {
            if (objectPoolDict[itemName].Count > 0) return;
            objectPoolDict[itemName].Push(item);
        }
        else // 异常处理(警告)
        {
            objectPoolDict.Add(itemName, new Stack<GameObject>());
            objectPoolDict[itemName].Push(item);
            //Debug.LogWarning(string.Format("对象池Push异常: 不存在{0}对象池", itemName));
        }
    }

    // 从对象池中取得实例的方法
    public GameObject GetItem(string itemName)
    {
        GameObject itemGO = null;
        if (objectPoolDict.ContainsKey(itemName)) // 有需要取得的对象池
        {
            if (objectPoolDict[itemName].Count == 0) // 对象池为空时直接实例化游戏物体
            {
                GameObject go = GetResource(itemName);
                if (go != null) 
                {
                    itemGO = RPGManager.Instance.CreateItem(go);
                }
            }
            else // 对象池不为空时则直接取出实例
            {
                itemGO = objectPoolDict[itemName].Pop();
                itemGO.SetActive(true); // 激活对象池取出的实例
            }
        }
        else // 没有该对象池时创建该实例的对象池
        {
            objectPoolDict.Add(itemName, new Stack<GameObject>());

            GameObject go = GetResource(itemName);
            if (go != null)
            {
                itemGO = RPGManager.Instance.CreateItem(go); // 直接实例化游戏物体返回
            }
        }

        if (itemGO == null) //异常处理
        {
            Debug.LogError(string.Format("对象池获取异常: 获取对象池实例{0}失败!", itemName));
        }

        return itemGO;
    }

    // 取资源的方法
    private GameObject GetResource(string itemName)
    {
        GameObject itemGO = null;
        string path = loadPath + itemName;
        if (factoryDict.ContainsKey(itemName)) // 如果工厂中有该资源
        {
            itemGO = factoryDict[itemName];
        }
        else
        {
            itemGO = Resources.Load<GameObject>(path); // 从文件夹中加载该资源
            factoryDict.Add(itemName, itemGO); // 将得到的资源放入资源字典当中
        }

        if (itemGO == null) //异常处理
        {
            Debug.LogError(string.Format("资源加载异常: {0}资源加载失败,加载路径为{1}", itemName, path));
        }

        return itemGO;
    }
}
