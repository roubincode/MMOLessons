using UnityEngine;
using UnityEngine.SceneManagement;
public class UIScene :IUIScene
{
    string sceneName;
    protected UIFacade mUIFacade;
    
    public UIScene(UIFacade uIFacade)
    {
        mUIFacade = uIFacade;
        sceneName = SceneManager.GetActiveScene().name;
    }

    public virtual void EnterScene()
    {
        mUIFacade.InitUIPanelDict();
    }

    public virtual void ExitScene()
    {
        mUIFacade.ClearUIPanelDict();
        UIScene self = this;
        self = null;
    }
}