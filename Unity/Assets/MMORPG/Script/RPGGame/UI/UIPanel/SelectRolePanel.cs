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
            mUIFacade.GetUI(StringManager.CreateRolePanel).EnterPanel();     
        });

        btn_back.onClick.SetListener(() => {
            ExitPanel();
            mUIFacade.ChangeScene(new AccountScene(mUIFacade));
            mUIFacade.ExitScene();//应该是加载新场景完成后执行，暂时直接这里调用
        });
    }

    public override void EnterPanel()
    {
        GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
        base.EnterPanel();
        RPGManager.Instance.CamLocation(RPGManager.Instance.selectRole_CamLocation);

        players = RPGManager.Instance.playerList.ToArray();
        
        InitListView();
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        // 更新局部lastPanel
        lastPanel = PanelType.Login;

        ClearPreview();
    }

    void InitListView(){
        Vector3 sp = RPGManager.Instance.role_SpawnLoaction.position;
        if(players.Length>0){
            
            // 生成创建角色列表
            foreach(Player player in players){
                GameObject preGo =  RPGManager.Instance.CreateItem(player.gameObject);
                preGo.transform.parent = viewRoot;
                preGo.transform.position = sp+=Vector3.left*1.2f;
                preGo.transform.rotation = RPGManager.Instance.role_SpawnLoaction.rotation;
                preGo.GetComponent<CharacterMovement>().enabled = false;
                preGo.name = player.ClassName;
            }
        }  
    }

    void ClearPreview(){
        for (int i = viewRoot.childCount - 1; i >= 0; i--) {
            Destroy(viewRoot.GetChild(i).gameObject);
        }
    }
}