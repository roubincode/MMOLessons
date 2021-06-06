using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 选角面板
/// </summary>
public class SettingPanel : BasePanel
{
    public Button btn_back;
    public Button btn_Logout;
    protected override void Awake()
    {
        base.Awake();
        thisPanel = PanelType.Setting;

    }

    void Start(){
        btn_Logout.onClick.SetListener(() => {
            ExitPanel();
            RPGManager.Instance.ClearLoaclPlayer();
            mUIFacade.ChangeScene(new RoleScene(mUIFacade));
        });
        btn_back.onClick.SetListener(() => {
           ExitPanel(); //局部界面返回只需要退出本界面
        });
    }

    public override void EnterPanel()
    {
        GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
        base.EnterPanel();
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        // 更新局部lastPanel,此界面回退定回到选角界面
        lastPanel = PanelType.MapUIFrame;
    }

}