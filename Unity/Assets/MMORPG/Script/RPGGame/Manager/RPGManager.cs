using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RPGManager : MonoBehaviour
{
    public static RPGManager Instance { get; private set; }
    public static IManager MMOMgr;
    public Transform select_camLocation;
    public Transform select_spawnLoaction;
    public Transform create_camLoaction;
    public Transform create_spawnLoaction;

    [HideInInspector] public List<Entity> entities = new List<Entity>();
    [HideInInspector] public List<Player> players = new List<Player>();

    private GameObject localPlaer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Application.isPlaying) DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        players = MMOMgr.FindPlayerClasses();

        foreach (Player item in players)
        {
            Debug.Log(item.CNName);
        }
    }
}
