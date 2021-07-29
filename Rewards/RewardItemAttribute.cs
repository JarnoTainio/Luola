using UnityEngine;

[CreateAssetMenu(fileName = "Item_attribute", menuName = "GemRPG/Reward/Item/Attribute")]
public class RewardItemAttribute : RewardItem
{
  [Header("EquipEffect")]
  public Resource equipResource;
  public int equipAmount = 0;

  [Header("TriggerEffect")]
  public Trigger trigger;
  public Resource resource;
  public int amount;
  public bool useReturnValue = false;
  public Condition condition;
  public bool triggerUsesTokens = true;

  public override void Equip(GameManager gameManager)
  {
    base.Equip(gameManager);
    if (equipAmount != 0)
    {
      gameManager.ModifyResource(equipResource, equipAmount, false);
    }
  }
  public override bool Trigger(GameManager gameManager, Trigger trigger, object data)
  {
    if ((amount != 0 || useReturnValue) && trigger == this.trigger)
    {
      if (condition.Test(gameManager))
      {
        if (useReturnValue){
          amount = (int)data;
        }
        gameManager.AddTrigger(this);
        return true;
      }
    }
    return false;
  }

}

[System.Serializable]
public struct Condition
{
  public Resource resource;
  public Comparator comparator;
  public int value;

  public Condition(Resource resource, Comparator comparator, int value)
  {
    this.resource = resource;
    this.comparator = comparator;
    this.value = value;
  }

  public bool Test(GameManager gameManager)
  {
    switch (comparator)
    {
      case Comparator.None:
        return true;
      case Comparator.Less:
        return gameManager.GetResource(resource, true) < value;
      case Comparator.Equal:
        return gameManager.GetResource(resource, true) == value;
      case Comparator.Greater:
        return gameManager.GetResource(resource, true) > value;
    }
    return true;
  }
}

public enum Comparator { None, Less, Equal, Greater }