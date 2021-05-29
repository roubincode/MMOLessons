using UnityEngine;
using UnityEngine.UI;
using MTAssets.EasyMinimapSystem;
/// <summary>
/// 选角面板
/// </summary>
public class MapUIFramePanel : BasePanel
{
    public Button btn_setting;

    public GameObject chatContent;
    
    public UIAbility uIAbility;
    public UIMessageSlot messageSlot;
    public MinimapRenderer mapRenderer;
    protected override void Awake()
    {
        base.Awake();
        thisPanel = PanelType.MapUIFrame;
    }

    void Start(){
        // 世界频道通知
        GameObject message = RPGManager.Instance.CreateItem(messageSlot.gameObject);
        message.transform.SetParent(chatContent.transform);
        UIMessageSlot slot = message.GetComponent<UIMessageSlot>();
        slot.text.text = "系统：欢迎来到MMORPG游戏的黎明镇！\n愿你有一个美好的游戏体验，快乐游戏，文明游戏。";

        btn_setting.onClick.SetListener(() => {
            mUIFacade.GetUI(StringManager.SettingPanel).EnterPanel();     
        });
    }

    public override void EnterPanel()
    {
        GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
        base.EnterPanel();

        RPGManager.Instance.CreateLocalPlayer();
        if(uIAbility!=null) uIAbility.RefreshAbility();
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        // 更新局部lastPanel,此界面回退定回到选角界面
        lastPanel = PanelType.SelectRole;
    }

}