public abstract class BaseAbility : Ability
{
    public bool reduce = false;
    public bool baseOnLevel = false;

    public LinearInt baseAbility = new LinearInt{baseValue=100};

    protected IAbility[] _bonusComponents;
    protected IAbility[] bonusComponents =>
        _bonusComponents ?? (_bonusComponents = GetComponents<IAbility>());

    // 计算总蓝量
    // 这里定每点耐力30生命
    public override int max
    {
        get
        {
            int bonus = 0;
            int baseThisLevel = baseOnLevel? baseAbility.Get(level.current):baseAbility.Get();
            foreach (IAbility bonusComponent in bonusComponents)
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
            foreach (IAbility bonusComponent in bonusComponents)
                bonus += bonusComponent.GetRageRecoveryBonus();
            if(reduce) return -baseRecovery + bonus;
            else return baseRecovery + bonus;
        }
    }
}