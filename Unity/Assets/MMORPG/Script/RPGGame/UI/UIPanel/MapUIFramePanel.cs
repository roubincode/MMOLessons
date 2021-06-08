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
    public MinimapRenderer minimapRenderer;
    public MinimapRenderer bigmapRenderer;
    public KeyCode[] activationKeys = {KeyCode.M, KeyCode.M};
    public GameObject bigMapPanel;
    protected override void Awake()
    {
        base.Awake();
        thisPanel = PanelType.MapUIFrame;
    }

    void Start(){
        
        btn_setting.onClick.SetListener(() => {
            mUIFacade.GetUIPanel(StringManager.SettingPanel).EnterPanel();     
        });
    }

    void Update(){
        if (Utils.AnyKeyDown(activationKeys)){
            if(!bigMapPanel.activeSelf) bigMapPanel.SetActive(true);
            else bigMapPanel.SetActive(false);
        }
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