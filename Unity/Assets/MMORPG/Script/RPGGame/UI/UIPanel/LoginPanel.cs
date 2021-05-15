using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 登录面板
/// </summary>
public class LoginPanel : BasePanel
{
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

            ExitPanel();
            mUIFacade.ChangeScene(new RoleScene(mUIFacade));
            mUIFacade.ExitScene();//应该是加载新场景完成后执行，暂时直接这里调用
        });

        btn_register.onClick.SetListener(() => {
            ExitPanel();
            mUIFacade.GetUI(StringManager.RegisterPanel).EnterPanel();     
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