using UnityEngine;

// 角色蓝量组件
[DisallowMultipleComponent]
public class Mana : ExAbility
{
    public int perMana;

    // 计算总蓝量
    // 这里定每点智力16蓝
    public override int max
    {
        get
        {
            int bonus = 0;
            int baseThisLevel = baseOnLevel? baseAbility.Get(level.current):baseAbility.Get();
            foreach (IAbility bonusComponent in bonusComponents)
                bonus += bonusComponent.GetManaBonus(baseThisLevel);
            return baseThisLevel + bonus  + intellect*perMana;
        }
    }

    //获取智力总量
    public int intellect
    {
        get
        {
            int bonus = 0;
            foreach (IAbility bonusComponent in bonusComponents)
                bonus += bonusComponent.GetIntellectBonus();
            return bonus;
        }
    }


    // 如果设置为非重生满能力值，这里设置重生蓝量为10
    void Start(){
        if(!spawnFull) current = 10;
    }
}