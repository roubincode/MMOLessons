using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有UI的管理者
/// </summary>
public class UIManager
{
    public UIFacade mUIFacade;
    // UI面板GameObject容器
    public Dictionary<string, GameObject> currentScenePanelGoDict;
    //UIPanel对象容器(当前场景状态下的UIPanel脚本对象)
    public Dictionary<string, IBasePanel> currentScenePanelDict;
    private RPGManager mGameManager;

    

    public UIManager()
    {
        mGameManager = RPGManager.Instance;
        currentScenePanelGoDict = new Dictionary<string, GameObject>();
        currentScenePanelDict = new Dictionary<string, IBasePanel>();
        mUIFacade = new UIFacade(this);
        mUIFacade.currentScene = new AccountScene(mUIFacade);
    }

    public IBasePanel GetUIPanel(string pName){
        return currentScenePanelDict[pName];
    }

    public GameObject GetUI(string uiName){
        return currentScenePanelGoDict[uiName];
    }

    public void AddPanelToDict(string uIPanelName,GameObject go){
        currentScenePanelGoDict.Add(uIPanelName,go);
    }

    // 清空UIPanel字典,并将所有UIPanel放回对象池
    public void ClearUIPanelDict()
    {
        foreach (var item in currentScenePanelGoDict)
        {
            mGameManager.PushItem(FactoryType.UIPanel, item.Key, item.Value);
        }

        currentScenePanelGoDict.Clear();
    }
}