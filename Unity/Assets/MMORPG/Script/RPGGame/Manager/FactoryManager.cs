using System.Collections.Generic;

/// <summary>
/// 工厂管理,负责管理各种类型的工厂以及对象池
/// </summary>
public class FactoryManager
{
    public Dictionary<FactoryType, IBaseFactory> factoryDict = new Dictionary<FactoryType, IBaseFactory>();

    public FactoryManager()
    {
        // 实例化各工厂
        factoryDict.Add(FactoryType.UIPanel, new UIPanelFactory());
        factoryDict.Add(FactoryType.UI, new UIFactory());
        factoryDict.Add(FactoryType.GameObject, new GameFactory());
    }
}
