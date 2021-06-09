using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 登录面板
/// </summary>
public class LoginPanel : BasePanel
{
    public InputField nickName;
    public InputField pass;
    public Button btn_submit;
    public Button btn_register;
    protected override void Awake()
    {
        base.Awake();
        thisPanel = PanelType.Login;
    }

    void Start(){
        btn_submit.onClick.SetListener(() => {
            // 判断登录成功
            // ...
            nickName.text = "";
            pass.text = "";

            ExitPanel();
            mUIFacade.ChangeScene(new RoleScene(mUIFacade));
        });

        btn_register.onClick.SetListener(() => {
            ExitPanel();
            mUIFacade.GetUIPanel(StringManager.RegisterPanel).EnterPanel();     
        });
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
    }

}