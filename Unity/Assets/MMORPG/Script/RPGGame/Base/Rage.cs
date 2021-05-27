using UnityEngine;

// 怒气组件
[DisallowMultipleComponent]
public class Rage : ExAbility
{
    public LinearInt baseRage = new LinearInt{baseValue=100};

    IRageBonus[] _bonusComponents;
    IRageBonus[] bonusComponents =>
        _bonusComponents ?? (_bonusComponents = GetComponents<IRageBonus>());

    // 计算总蓝量
    // 这里定每点耐力30生命
    public override int max
    {
        get
        {
            int bonus = 0;
            int baseThisLevel = baseRage.Get(level.current);
            foreach (IRageBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetRageBonus(baseThisLevel);
            return baseThisLevel + bonus;
        }
    }
    
    // 基础的单位时间回血量
    public int baseRecovery = 2;
    // 获取总单位时间回血量
    public override int recovery
    {
        get
        {
            int bonus = 0;
            foreach (IRageBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetRageRecoveryBonus();
            return -baseRecovery + bonus;
        }
    }

    // 如果设置为非重生满能力值，这里设置重生血量为20
    void Start(){
        if(!spawnFull) current = 20;
    }

}