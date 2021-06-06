// 生命能力 全系
// 蓝量能力 法系
// 能量能力 盗贼 Rogue energy
// 怒气能力 战士 Warrior rage

public interface IAbility
{
    int GetHealthBonus(int baseHealth);
    int GetHealthRecoveryBonus();
    int GetEnduranceBonus();

    int GetManaBonus(int baseMana);
    int GetManaRecoveryBonus();
    int GetIntellectBonus();

    int GetEnergyBonus(int baseHealth);
    int GetEnergyRecoveryBonus();

    int GetRageBonus(int baseHealth);
    int GetRageRecoveryBonus();
}



