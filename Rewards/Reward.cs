using UnityEngine;

public abstract class Reward : ScriptableObject
{
  public int level;
  public bool isReward = true;
  public bool eliteReward = false;
  public int minFloor = 0;
  public int maxFloor = 999;
  public GameElement[] gameElementType;
  public int weight;
  public Condition[] requirements;

  public abstract void Equip(GameManager gameManager);
  public virtual bool CanBeReward(GameManager gameManager){
    if (gameElementType != null && gameElementType.Length > 0){
      foreach(GameElement e in gameElementType){
        if (DataManager.instance.saveData.disabledGameElements.Contains(e)){
          return false;
        }
      }
    }
    return true;
  }
  public abstract Sprite GetSprite();
  public abstract string GetName();

  public string GetDescription() { return GetName() + "Description";  }

  public abstract RewardType GetRewardType();

  public abstract int GetSortValue();
}

[System.Serializable]
public enum RewardType { Gem, Ability, Item }