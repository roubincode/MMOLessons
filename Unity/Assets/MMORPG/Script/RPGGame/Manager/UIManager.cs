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
    public Transform canvas; // UIPanel放置的容器

    public UIManager()
    {
        mGameManager = RPGManager.Instance;
        currentScenePanelGoDict = new Dictionary<string, GameObject>();
        currentScenePanelDict = new Dictionary<string, IBasePanel>();
        canvas = GameObject.Find("Canvas").transform;

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

    // 实例化当前场景下的UIPanel并存入字典
    public void InitUIPanelDict()
    {
        
        foreach (var item in currentScenePanelGoDict)
        {
            item.Value.transform.SetParent(canvas);
            item.Value.transform.localPosition = Vector3.zero;
            item.Value.transform.localScale = Vector3.one;
            item.Value.SetActive(false); // 初始时先取消Panel的激活状态
            IBasePanel basePanel = item.Value.GetComponent<IBasePanel>();
            if (basePanel == null)
            {
                Debug.LogWarning(string.Format("{0}上的IBasePanel脚本丢失!", item.Key));
            }
            basePanel.InitPanel();  // UIPanel初始化UI
            currentScenePanelDict.Add(item.Key, basePanel); // 将该场景下的UIPanel身上的Panel脚本添加进字典中
        }
    }

    // 清空UIPanel字典,并将所有UIPanel放回对象池
    public void ClearUIPanelDict()
    {
        currentScenePanelDict.Clear();
        foreach (var item in currentScenePanelGoDict)
        {
            mGameManager.PushItem(FactoryType.UIPanel, item.Key, item.Value);
        }

        currentScenePanelGoDict.Clear();
    }
}