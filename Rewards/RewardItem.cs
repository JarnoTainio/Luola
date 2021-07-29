using UnityEngine;

public abstract class RewardItem : Reward
{
  public string itemName;
  public Sprite sprite;

  public abstract bool Trigger(GameManager gameManager, Trigger trigger, object data = null);

  public override void Equip(GameManager gameManager)
  {
    DataManager.instance.items.Add(this);
  }

  public override bool CanBeReward(GameManager gameManager)
  {
    if (base.CanBeReward(gameManager) == false){
      return false;
    }
    if (requirements != null)
    {
      foreach (Condition condition in requirements)
      {
        if (!condition.Test(gameManager))
        {
          return false;
        }
      }
    }
    return !DataManager.instance.items.Contains(this);
  }

  public override Sprite GetSprite()
  {
    return sprite;
  }

  public override string GetName()
  {
    return itemName;
  }

  public override RewardType GetRewardType()
  {
    return RewardType.Item;
  }

    public override int GetSortValue(){
    return 10;
  }
}

public enum Trigger { None, RoomStart, TurnStart, RoundStart, CombatStart, DamageTaken, MonsterDamaged, MonsterKilled, PlayerBlocked, RoundEnd }