using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "GemRPG/Ability/Action")]
public class Ability_Action : Ability
{
  public override void Use(AbilityManager abilityManager)
  {
    UseEnergy(abilityManager);
    abilityManager.gameManager.ModifyTurn(1);
  }

    public override int GetSortValue(){
    return base.GetSortValue() - 10;// - 10;
  }
}
