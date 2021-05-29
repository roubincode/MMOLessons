using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MTAssets.EasyMinimapSystem;
/// <summary>
/// 游戏总管理,负责管理其他所有的管理者
/// </summary>
public class RPGManager : MonoBehaviour
{
    public Transform Item;
    public static RPGManager Instance { get; private set; } // RPGManager单例
    public FactoryManager FactoryManager { get; private set; }
    public UIManager UIManager { get; private set; }

    public Transform select_camLocation;
    public Transform create_spawnLoaction;
    public Transform create_camLoaction;
    public Transform select_spawnLoaction;

    // 可选角色类集
    [HideInInspector] public List<Player> playerClasses = new List<Player>(); 
    // 创建的角色集
    [HideInInspector] public List<Player> playerList = new List<Player>();
    // 创建的角色昵称集
    [HideInInspector] public List<string> playerNicks = new List<string>();

    // 选中的进入游戏地图场景的本地角色
    private GameObject localPlayer;

    [HideInInspector] public string selectClass;
    [HideInInspector] public string selectName;

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

    public void CreateLocalPlayer(){
        Player player = playerClasses.ToList().Find(p => p.ClassName == selectClass);
        localPlayer =  RPGManager.Instance.CreateItem(player.gameObject);
        localPlayer.transform.position = select_spawnLoaction.position;
        localPlayer.transform.rotation = select_spawnLoaction.rotation;
        localPlayer.name = selectName;
        localPlayer.GetComponent<CharacterMovement>().enabled = true;
        localPlayer.GetComponent<Player>().nickName = selectName;
        Player.localPlayer = localPlayer.GetComponent<Player>();
        
        // camera
        CameraMMO cameraMMO = Camera.main.GetComponent<CameraMMO>();
        cameraMMO.enabled = true;
        cameraMMO.target = localPlayer.transform;

        // minimap
        MapUIFramePanel mapUI = (MapUIFramePanel)UIManager.mUIFacade.GetUI(StringManager.MapUIFramePanel);
        mapUI.mapRenderer.minimapCameraToShow = localPlayer.GetComponent<MinimapCamera>();
    }

    public void ClearLoaclPlayer(){
        CameraMMO cameraMMO = Camera.main.GetComponent<CameraMMO>();
        cameraMMO.enabled = false;
        cameraMMO.target = null;
        if(localPlayer!=null) Destroy(localPlayer);
    }

    public void ClearPlayerList(){
        playerList.Clear();
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

    public void NoneSelectd(){
        foreach(Player player in playerList){
            selectClass = "";
            player.GetComponent<SelectableCharacter>().selected = false;
        }  
    }

    // 实例化游戏物体的方法
    public GameObject CreateItem(GameObject prefab)
    {
        return Instantiate(prefab);
    }

}