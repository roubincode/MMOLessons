using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏物体(怪物,塔,子弹,道具等)工厂
/// </summary>
public class GameFactory : BaseFactory
{
    public GameFactory()
    {
        loadPath += "Game/";
    }
}
