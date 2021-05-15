using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 游戏总管理,负责管理其他所有的管理者
/// </summary>
public class RPGManager : MonoBehaviour
{
    public Transform Item;
    public static RPGManager Instance { get; private set; } // RPGManager单例
    public FactoryManager FactoryManager { get; private set; }
    public UIManager UIManager { get; private set; }

    public Transform selectRole_CamLocation;
    public Transform role_PreviewLoaction;
    public Transform role_PreviewCamLoaction;
    public Transform role_SpawnLoaction;

    // 可选角色列类列表
    [HideInInspector] public List<Player> playerClasses = new List<Player>(); 
    [HideInInspector] public List<Player> playerList = new List<Player>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Application.isPlaying) DontDestroyOnLoad(gameObject);

        UIManager = new UIManager();
        FactoryManager = new FactoryManager();
        UIManager.mUIFacade.currentScene.EnterScene();
    }

    private void Start(){
        
    }


    // 将游戏物体放回对象池的方法
    public void PushItem(FactoryType factoryType, string itemName, GameObject item)
    {
        FactoryManager.factoryDict[factoryType].PushItem(itemName, item);
    }

    // 获取游戏物体的方法
    public GameObject GetItem(FactoryType factoryType, string itemName)
    {
        return FactoryManager.factoryDict[factoryType].GetItem(itemName);
    }

    public void CamLocation(Transform location){
        Camera.main.transform.position = location.position;
        Camera.main.transform.rotation = location.rotation;
    }

    // 实例化游戏物体的方法
    public GameObject CreateItem(GameObject prefab)
    {
        return Instantiate(prefab);
    }

}