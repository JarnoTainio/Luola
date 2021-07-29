using UnityEngine;

[CreateAssetMenu(fileName = "Transform", menuName = "GemRPG/Ability/Transform")]
public class Ability_Transform : Ability
{
  public Resource original;
  public Resource target;
  public int maximumAmount;

  public override void Use(AbilityManager abilityManager)
  {
    UseEnergy(abilityManager);
    int value = abilityManager.gameManager.GetResource(original);
    value = Mathf.Min(value, maximumAmount);
    abilityManager.gameManager.ModifyResource(original, -value);
    abilityManager.gameManager.ModifyResource(target, value);
  }

  public override bool CanBeUsed(AbilityManager abilityManager)
  {
    if (base.CanBeUsed(abilityManager))
    {
      return abilityManager.gameManager.GetResource(original) > 0;
    }
    return false;
  }

    public override int GetSortValue(){
    return base.GetSortValue() + 200;
  }
}