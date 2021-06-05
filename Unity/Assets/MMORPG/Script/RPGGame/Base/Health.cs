using UnityEngine;

// 角色生命组件
[DisallowMultipleComponent]
public class Health : MainAbility
{
    public int perHealth;

    // 计算总蓝量
    // 这里定每点耐力30生命
    public override int max
    {
        get
        {
            int bonus = 0;
            int baseThisLevel =  baseOnLevel? baseAbility.Get(level.current):baseAbility.Get();
            foreach (IAbility bonusComponent in bonusComponents)
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
            foreach (IAbility bonusComponent in bonusComponents)
                bonus += bonusComponent.GetEnduranceBonus();
            return bonus;
        }
    }
    

    // 如果设置为非重生满能力值，这里设置重生血量为20
    void Start(){
        if(!spawnFull) current = 20;
    }

}