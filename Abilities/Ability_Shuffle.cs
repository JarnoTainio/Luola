using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shuffle", menuName = "GemRPG/Ability/Shuffle")]
public class Ability_Shuffle : Ability
{
  public override void Use(AbilityManager abilityManager)
  {
    UseEnergy(abilityManager);
    abilityManager.gameManager.ShuffleBoard();
  }

    public override int GetSortValue(){
    return base.GetSortValue() - 10;
  }
}
