using UnityEngine;

// 角色生命组件
[RequireComponent(typeof(Level))]
[DisallowMultipleComponent]
public class Health : Ability
{
    public int perHealth;
    public LinearInt baseHealth = new LinearInt{baseValue=100};

    // 缓存实现IHealthBonus的组件实例
    // 角色能力系统中的血蓝，攻防能力的属性值来自于装备系统，技能buff系统，宠物坐骑系统组件
    // 角色装备组件，角色技能等组件都继承了能力接口与战斗接口，我们可能通过接口来获取到他们，并调用接口方法
    IHealthBonus[] _bonusComponents;
    IHealthBonus[] bonusComponents =>
        _bonusComponents ?? (_bonusComponents = GetComponents<IHealthBonus>());

    // 计算总蓝量
    // 这里定每点耐力30生命
    public override int max
    {
        get
        {
            int bonus = 0;
            int baseThisLevel = baseHealth.Get(level.current);
            foreach (IHealthBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetHealthBonus(baseThisLevel);
            return baseThisLevel + bonus + endurance*perHealth;
        }
    }

    //获取耐力总量
    public int endurance
    {
        get
        {
            int bonus = 0;
            foreach (IHealthBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetEnduranceBonus();
            return bonus;
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
            foreach (IHealthBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetHealthRecoveryBonus();
            return baseRecovery + bonus;
        }
    }

    // 如果设置为非重生满能力值，这里设置重生血量为20
    void Start(){
        if(!spawnFull) current = 20;
    }

}