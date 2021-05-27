using UnityEngine;

// 角色蓝量组件
[RequireComponent(typeof(Level))]
[DisallowMultipleComponent]
public class Mana : ExAbility
{
    public int perMana;
    public LinearInt baseMana = new LinearInt{baseValue=100};

    // 缓存实现IManaBonus的组件实例
    IManaBonus[] _bonusComponents;
    IManaBonus[] bonusComponents =>
        _bonusComponents ?? (_bonusComponents = GetComponents<IManaBonus>());

    // 计算总蓝量
    // 这里定每点智力16蓝
    public override int max
    {
        get
        {
            int bonus = 0;
            int baseThisLevel = baseMana.Get(level.current);
            foreach (IManaBonus bonusComponent in bonusComponents)
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
            foreach (IManaBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetIntellectBonus();
            return bonus;
        }
    }

    // 基础的单位时间回蓝量
    public int baseRecovery = 2;
    // 获取总单位时间回蓝量
    // 可以在这里增加通过装备，技能等提高单位时间的恢复量，但这一点功能的实现放到以后的课程
    public override int recovery
    {
        get
        {
            int bonus = 0;
            foreach (IManaBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetManaRecoveryBonus();
            return baseRecovery + bonus;
        }
    }

    // 如果设置为非重生满能力值，这里设置重生蓝量为10
    void Start(){
        if(!spawnFull) current = 10;
    }
}