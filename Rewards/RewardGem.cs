using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gem", menuName = "GemRPG/Reward/Gem")]
public class RewardGem : Reward
{
  public string gemName;
  public GemItem gem;
  public Sprite sprite;
  public override void Equip(GameManager gameManager)
  {
    DataManager.instance.AddGem(gem);
  }

  public override bool CanBeReward(GameManager gameManager)
  {
    if (base.CanBeReward(gameManager) == false){
      return false;
    }
    return true;
    /*
    int count = 0;
    foreach (GemItem g in DataManager.instance.gemBag)
    {
      if (g.color == gem.color && g.type == gem.type)
      {
        count++;
      }
    }
    return count < 10;
    */
  }

  public override Sprite GetSprite()
  {
    return sprite;
  }

  public override string GetName()
  {
    return gem.GetName();
  }

  public override RewardType GetRewardType()
  {
    return RewardType.Gem;
  }

  public override int GetSortValue(){
    return -(int)gem.color;
  }
}
