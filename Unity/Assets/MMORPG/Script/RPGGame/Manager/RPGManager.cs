using UnityEngine;
using System.Collections.Generic;
using Mirror;
/// <summary>
/// 游戏总管理,负责管理其他所有的管理者
/// </summary>
public class RPGManager : MonoBehaviour
{
    public static RPGManager Instance { get; private set; } // RPGManager单例
    public static IMMOManager MMOMgr;
    public Transform select_camLocation;
    public Transform create_spawnLoaction;
    public Transform create_camLoaction;
    public Transform select_spawnLoaction;

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

    public void ClearPlayerList(){
        playerList.Clear();
    }

    // 实例化游戏物体的方法
    public GameObject CreateItem(GameObject prefab)
    {
        return Instantiate(prefab);
    }

}