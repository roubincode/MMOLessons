using UnityEngine;

// 能量组件
[DisallowMultipleComponent]
public class Energy : ExAbility
{
    public LinearInt baseEnergy = new LinearInt{baseValue=100};

    IEnergyBonus[] _bonusComponents;
    IEnergyBonus[] bonusComponents =>
        _bonusComponents ?? (_bonusComponents = GetComponents<IEnergyBonus>());

    // 计算总蓝量
    // 这里定每点耐力30生命
    public override int max
    {
        get
        {
            int bonus = 0;
            int baseThisLevel = baseEnergy.Get(level.current);
            foreach (IEnergyBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetEnergyBonus(baseThisLevel);
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
            foreach (IEnergyBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetEnergyRecoveryBonus();
            return baseRecovery + bonus;
        }
    }

    // 如果设置为非重生满能力值，这里设置重生血量为20
    void Start(){
        if(!spawnFull) current = 20;
    }

}