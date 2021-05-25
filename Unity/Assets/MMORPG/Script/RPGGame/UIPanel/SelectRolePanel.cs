using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectRolePanel : UIPanel
{
    public Button btn_back; 
    public Button btn_create;
    public Button btn_enter;

    private Transform viewRoot;

    // 玩家已经创建角色
    private Player[] players;
    private string[] nicks;

    void Awake()
    {
        players = RPGManager.Instance.playerList.ToArray();

        viewRoot = GameObject.Find("SelViewRoot").transform;
    }
    void Start()
    {
        
        btn_create.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterCreateRolePanel();
        });
        btn_enter.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterMapUIFramePanel();
            
        });
        btn_back.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterLoginPanel();
            RPGManager.Instance.ClearPlayerList();
        });
    }

    public override void EnterPanel()
    {
        GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
        base.EnterPanel();
        Camera.main.GetComponent<Animator>().enabled = false;
        RPGManager.Instance.CamLocation(RPGManager.Instance.select_camLocation);

        // 动态生成内容不放在Start方法中，要放在EnterPanel方法中
        players = RPGManager.Instance.playerList.ToArray();
        nicks = RPGManager.Instance.playerNicks.ToArray();
        InitListView();
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        // 移除动态内容
        ClearPreview();
    }

    void InitListView(){
        Vector3 sp = RPGManager.Instance.select_spawnLoaction.position;
        if(players.Length>0){
            
            // 生成创建角色列表
            for(int i=0;i<players.Length;++i){
                Player player = players[i];
                GameObject preGo =  RPGManager.Instance.CreateItem(player.gameObject);
                preGo.transform.parent = viewRoot;
                preGo.transform.position = sp+=Vector3.left*1.2f;
                preGo.transform.rotation = RPGManager.Instance.select_spawnLoaction.rotation;
                
                preGo.name = nicks[i];
                preGo.GetComponent<Player>().nickName = nicks[i];
            }
        }  
    }

    void ClearPreview(){
        for (int i = viewRoot.childCount - 1; i >= 0; i--) {
            Destroy(viewRoot.GetChild(i).gameObject);
        }
    }
}
