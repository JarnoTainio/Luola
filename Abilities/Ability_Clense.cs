using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Clense", menuName = "GemRPG/Ability/Clense")]
public class Ability_Clense : Ability
{
  public int count = 1;
  public override void Use(AbilityManager abilityManager)
  {
    UseEnergy(abilityManager);
    abilityManager.gameManager.gemContainer.RemoveEffects(count);
  }

  public override bool CanBeUsed(AbilityManager abilityManager)
  {
    if (base.CanBeUsed(abilityManager))
    {
      return abilityManager.gameManager.gemContainer.GetEffectCount() > 0;
    }
    return false;
  }

    public override int GetSortValue(){
    return base.GetSortValue() + 5;
  }
}