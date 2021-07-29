using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "GemRPG/Reward/Ability")]
public class RewardAbility : Reward
{
  public Ability ability;
  public override void Equip(GameManager gameManager)
  {
    DataManager.instance.abilities.Add(ability);
  }

  public override bool CanBeReward(GameManager gameManager)
  {
    if (base.CanBeReward(gameManager) == false){
      return false;
    }
    return DataManager.instance.abilities.Count < 7 && !DataManager.instance.abilities.Contains(ability);
  }

  public override Sprite GetSprite()
  {
    return ability.icon;
  }

  public override string GetName()
  {
    return ability.abilityName;
  }

  public override RewardType GetRewardType()
  {
    return RewardType.Ability;
  }

    public override int GetSortValue(){
    return 20;
  }
}
