using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneState :ISceneState
{
    string sceneName;
    protected UIFacade mUIFacade;
    
    public SceneState(UIFacade uIFacade)
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
    }
}