using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attribute", menuName = "GemRPG/Ability/Attribute")]
public class Ability_Attribute : Ability
{
  [Header("Attributes")]
  public List<ResourceAmount> resources;

  public override void Use(AbilityManager abilityManager)
  {
    UseEnergy(abilityManager);
    foreach(ResourceAmount res in resources)
    {
      abilityManager.gameManager.ModifyResource(res.resource, res.amount, res.usesTokens, abilityManager.GetAbilityButtonPosition(this));
    }
  }

  public override bool CanBeUsed(AbilityManager abilityManager)
  {
    if (base.CanBeUsed(abilityManager))
    {
      bool canBeUsed = true;
      foreach (ResourceAmount res in resources)
      {
        if (res.amount < 0)
        {
          canBeUsed &= (abilityManager.gameManager.GetResource(res.resource) + res.amount) >= (res.resource == Resource.Life ? 1 : 0);
        }
        else
        {
          switch(res.resource)
          {
            case Resource.Life:
              canBeUsed &= abilityManager.gameManager.GetResource(res.resource) < DataManager.instance.saveData.maxLife;
              break;
          }
        }
      }
      return canBeUsed;
    }
    return false;
  }

  public override ResourceAmount[] GetCost(){
    List<ResourceAmount> res = resources.FindAll(r => (r.amount < 0));
    if (energyCost > 0){
      res.Add(new ResourceAmount(){resource=Resource.Energy, amount=energyCost});
    }
    return res.ToArray();
  }

  public override int GetSortValue(){
    int v = 0;
    if (resources != null && resources.Count > 0){
      Resource[] list = new Resource[]{Resource.Attack, Resource.Defence, Resource.Life, Resource.Gold, Resource.Power, Resource.RoomGold, Resource.RoomPower};
      for(int i = 0; i < list.Length; i++){
        if (list[i] == resources[0].resource){
          v = i;
          break;
        }
      }
    }
    return base.GetSortValue() + v;
  }
}

