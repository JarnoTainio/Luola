using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger", menuName = "GemRPG/Ability/Trigger")]
public class Ability_Trigger : Ability
{
  public AbilityTrigger trigger;
  public override void Use(AbilityManager abilityManager)
  {
    UseEnergy(abilityManager);
    abilityManager.AddTrigger(trigger);
  }
  public override bool CanBeUsed(AbilityManager abilityManager)
  {
    return base.CanBeUsed(abilityManager) && abilityManager.triggers.Contains(trigger) == false;
  }
  public override bool IsActive(GameManager gameManager) { return gameManager.abilityManager.triggers.Contains(trigger); }

  public override int GetSortValue(){
    return base.GetSortValue() + 300 +(int) trigger;
  }
}

[System.Serializable]
public enum AbilityTrigger { FreeMove, BombGem, SliderGem, DoubleOrbs, DestroyGem }