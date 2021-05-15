using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIPanel之间的中介,以及Manager与panel之间的外观角色
/// </summary>
public class UIFacade
{
    // 管理者
    private readonly UIManager mUIManager;
    private readonly RPGManager mGameManager;

    //UI面板(当前场景状态下的UIPanel脚本对象)
    public Dictionary<string, IBasePanel> currentScenePanelDict = new Dictionary<string, IBasePanel>();

    // 场景状态
    public ISceneState currentScene;
    public ISceneState lastScene;

    // 全局lastPanel
    public PanelType lastPanel = PanelType.Null;

    public Transform canvas; // UIPanel放置的容器

    public UIFacade(UIManager uIManager)
    {
        mUIManager = uIManager;
        mGameManager = RPGManager.Instance;
        Init();
    }

    public void Init()
    {
        canvas = GameObject.Find("Canvas").transform;
    }

    // 改变当前场景的状态
    public void ChangeScene(ISceneState scene)
    {
        lastScene = currentScene;
        currentScene = scene;
    }

    public IBasePanel GetUI(string pName){
        return mUIManager.Get(pName);
    }

    // 返回之前界面
    public void BackToPanel(PanelType panel){
        mUIManager.Get(panel.ToString()).EnterPanel();
    }

    // 将UIPanel添加进UIManager字典
    public void AddPanelToDict(string uIPanelName)
    {
        mUIManager.currentScenePanelDict.Add(uIPanelName, GetItem(FactoryType.UIPanel, uIPanelName));
        //Debug.Log(mUIManager.currentScenePanelDict.Count);
    }

    // 实例化当前场景下的UIPanel并存入字典
    public void InitUIPanelDict()
    {
        
        foreach (var item in mUIManager.currentScenePanelDict)
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

    // 清空UIPanel字典
    public void ClearUIPanelDict()
    {
        currentScenePanelDict.Clear();
        mUIManager.ClearUIPanelDict();
    }

    // 离开当前场景的方法
    public void ExitScene()
    {
        lastScene.ExitScene();
        currentScene.EnterScene();
    }

    // 获取游戏物体的方法
    public GameObject GetItem(FactoryType factoryType, string itemName)
    {
        //Debug.Log(mGameManager.ToString());
        return mGameManager.GetItem(factoryType, itemName);
    }

    // 将游戏物体放回对象池的方法
    public void PushItem(FactoryType factoryType, string itemName, GameObject item)
    {
        mGameManager.PushItem(factoryType, itemName, item);
    }

}
