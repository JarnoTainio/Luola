using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
  public string abilityName;
  public Sprite icon;

  public int energyCost;
  public bool endless;
  public List<GamePhase> refreshPhases;
  public int numberOfUses = 1;

  [HideInInspector]
  public bool exhausted;

  public int usesLeft;

  public void Refresh(GamePhase phase)
  {
    if (endless || refreshPhases.Contains(phase))
    {
      usesLeft = numberOfUses;
      exhausted = false;
    }
  }

  public abstract void Use(AbilityManager abilityManager);

  public virtual bool CanBeUsed(AbilityManager abilityManager)
  {
    return !exhausted
    && abilityManager.gameManager.GetResource(Resource.Energy) >= energyCost
    && abilityManager.gameManager.PlayerControl(false)
    && (endless || usesLeft > 0)
    ;
  }

  protected void UseEnergy(AbilityManager abilityManager)
  {
    if (endless == false)
    {
      usesLeft -= 1;
      if (usesLeft == 0)
      {
        exhausted = true;
      }
    }
    if  (energyCost != 0){
      abilityManager.gameManager.ModifyResource(Resource.Energy, -energyCost);
    }
  }

  public virtual ResourceAmount[] GetCost(){ 
    return new ResourceAmount[]{new ResourceAmount{amount = energyCost, resource = Resource.Energy}};
  }

  public virtual int GetSortValue(){
    if (endless){
      return 0;
    }
    GamePhase refresh = refreshPhases[0];

    int value = 0;
    switch (refresh){
      case GamePhase.EncounterStart:{
        value = 500 - numberOfUses + GetCost()[0].amount;
        break;
      }
      case GamePhase.RoundStart:{
        value = 100 - numberOfUses + GetCost()[0].amount;
        break;
      }
      
      
    }
    return value;
  }

  public virtual bool IsActive(GameManager gameManager) { return false; }
}