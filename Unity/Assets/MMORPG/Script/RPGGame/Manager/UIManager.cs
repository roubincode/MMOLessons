using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有UI的管理者
/// </summary>
public class UIManager
{
    public UIFacade mUIFacade;
    public Dictionary<string, GameObject> currentScenePanelDict;
    private RPGManager mGameManager;

    

    public UIManager()
    {
        mGameManager = RPGManager.Instance;
        currentScenePanelDict = new Dictionary<string, GameObject>();
        mUIFacade = new UIFacade(this);
        mUIFacade.currentScene = new AccountScene(mUIFacade);
    }

    public IBasePanel Get(string pName){
        return mUIFacade.currentScenePanelDict[pName];
    }

    // 清空UIPanel字典,并将所有UIPanel放回对象池
    public void ClearUIPanelDict()
    {
        foreach (var item in currentScenePanelDict)
        {
            mGameManager.PushItem(FactoryType.UIPanel, item.Key, item.Value);
        }

        currentScenePanelDict.Clear();
    }
}