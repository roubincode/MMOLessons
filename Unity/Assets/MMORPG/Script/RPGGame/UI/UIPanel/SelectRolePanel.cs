using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 选角面板
/// </summary>
public class SelectRolePanel : BasePanel
{
    public Button btn_enterMap;

    public Button btn_Create;
    public Button btn_back;

    private Transform viewRoot;

    // 玩家已经创建角色
    private Player[] players;
    private string[] nicks;

    protected override void Awake()
    {
        base.Awake();
        thisPanel = PanelType.SelectRole;

        players = RPGManager.Instance.playerList.ToArray();

        viewRoot = GameObject.Find("SelViewRoot").transform;
    }

    void Start(){
        btn_Create.onClick.SetListener(() => {
            ExitPanel();
            mUIFacade.GetUIPanel(StringManager.CreateRolePanel).EnterPanel();     
        });

        btn_enterMap.onClick.SetListener(() => {
            ExitPanel();
            mUIFacade.ChangeScene(new MapScene(mUIFacade));
        });

        btn_back.onClick.SetListener(() => {
            ExitPanel();
            mUIFacade.ChangeScene(new AccountScene(mUIFacade));

            RPGManager.Instance.ClearPlayerList();
        });
    }

    public override void EnterPanel()
    {
        GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
        base.EnterPanel();

        RPGManager.Instance.CamLocation(RPGManager.Instance.select_camLocation);

        // 动态生成内容不放在Start方法中，要放在EnterPanel方法中
        players = RPGManager.Instance.playerList.ToArray();
        nicks = RPGManager.Instance.playerNicks.ToArray();
        InitListView();
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        // 更新局部lastPanel，此界面回退定回到登录界面
        lastPanel = PanelType.Login;
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
                preGo.GetComponent<CharacterMovement>().enabled = false;
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