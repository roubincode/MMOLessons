using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 注册面板
/// </summary>
public class RegisterPanel : BasePanel
{
    public Button btn_submit;
    public Button btn_back;
    
    protected override void Awake()
    {
        base.Awake();
        thisPanel = PanelType.Register;
    }

    void Start(){
        btn_submit.onClick.SetListener(() => {

        });

        btn_back.onClick.SetListener(() => {
            base.BackToPanel();     
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