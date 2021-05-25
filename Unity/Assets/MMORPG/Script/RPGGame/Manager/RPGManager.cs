using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Mirror;
/// <summary>
/// 游戏总管理,负责管理其他所有的管理者
/// </summary>
public class RPGManager : MonoBehaviour
{
    public static RPGManager Instance { get; private set; } // RPGManager单例
    public static IMMOManager MMOMgr;
    public Transform select_camLocation;
    public Transform select_spawnLoaction;
    public Transform create_spawnLoaction;
    public Transform create_camLoaction;
    
    [HideInInspector] public string selectClass;
    [HideInInspector] public string selectName;
    [HideInInspector] public List<string> playerNicks = new List<string>();

    // 可选角色类集
    [HideInInspector] public List<Player> playerClasses = new List<Player>(); 
    // 创建的角色集
    [HideInInspector] public List<Player> playerList = new List<Player>();

    // 选中的进入游戏地图场景的本地角色
    private GameObject localPlayer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Application.isPlaying) DontDestroyOnLoad(gameObject);

    }

    private void Start(){
        playerClasses = MMOMgr.FindPlayerClasses();

        foreach(Player player in playerClasses){
            Debug.Log(player.CNName);
        }
    }

    public void CamLocation(Transform location){
        Camera.main.transform.position = location.position;
        Camera.main.transform.rotation = location.rotation;
    }

    public void CreateLocalPlayer(){
        Player player = playerClasses.ToList().Find(p => p.ClassName == selectClass);
        localPlayer =  RPGManager.Instance.CreateItem(player.gameObject);
        localPlayer.transform.position = select_spawnLoaction.position;
        localPlayer.transform.rotation = select_spawnLoaction.rotation;
        localPlayer.name = selectName;
        localPlayer.GetComponent<Player>().nickName = selectName;
        //localPlayer.GetComponent<CharacterMovement>().enabled = true;
        // CameraMMO cameraMMO = Camera.main.GetComponent<CameraMMO>();
        // cameraMMO.enabled = true;
        // cameraMMO.target = localPlayer.transform;
    }

    public void ClearLoaclPlayer(){
        // CameraMMO cameraMMO = Camera.main.GetComponent<CameraMMO>();
        // cameraMMO.enabled = false;
        // cameraMMO.target = null;
        if(localPlayer!=null) Destroy(localPlayer);
    }

    public void ClearPlayerList(){
        playerList.Clear();
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