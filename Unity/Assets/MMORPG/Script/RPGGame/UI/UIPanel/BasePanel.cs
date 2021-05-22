using UnityEngine;

/// <summary>
/// Panel基类
/// </summary>
public class BasePanel : MonoBehaviour, IBasePanel
{
    protected UIFacade mUIFacade;

    // 局部lastPanel
    protected PanelType lastPanel = PanelType.Null;
    protected PanelType thisPanel;

    protected virtual void Awake()
    {
        mUIFacade = RPGManager.Instance.UIManager.mUIFacade;
    }

    public virtual void InitPanel()
    {
        
    }

    public virtual void EnterPanel()
    {
        gameObject.SetActive(true);
    }

    public virtual void ExitPanel()
    {
        gameObject.SetActive(false);
        // 更新全局lastPanel
        mUIFacade.lastPanel = thisPanel;
    }

    public virtual void UpdatePanel()
    {
        
    }

    public virtual void BackToPanel()
    {
        if(lastPanel == PanelType.Null && mUIFacade.lastPanel==PanelType.Null) 
            return;

        // 局部lastPanel高于全局lastPanel，局部未指定的话，使用全局lastPanel
        // 比如不论从登录界面来到选角界面，还是从创角界面来到选角界面，在选角界面返回都应该是回到登录界面
        if(lastPanel != PanelType.Null){
            mUIFacade.BackToPanel(lastPanel);
            return;
        }

        if(mUIFacade.lastPanel != PanelType.Null){
            mUIFacade.BackToPanel(mUIFacade.lastPanel);
        }

        ExitPanel();
    }
}
