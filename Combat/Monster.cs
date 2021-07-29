using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Minion", menuName = "GemRPG/Monster/Minion")]
public class Monster : ScriptableObject
{
  public Sprite sprite;
  public int difficulty;
  public MonsterType type;
  public int maxLife;
  public int aggressionTimer;
  public int aggressionGrowth;
  public bool fixedActionRotation = true;
  public List<MonsterAction> actions;
  [HideInInspector]
  public MonsterAction lastAction;
  [Header("Visual")]
  public float scale = 1f;
  public Vector2 position;
  public Sprite backgroundSprite;
  [Header("RoomModifiers")]
  public int roomGold;
  public int roomPower;
  public int escapeOnTurn = 10;
  public int escapeCost = 1;

  public MonsterAction GetAction(int round)
  {
    MonsterAction currentAction = null;
    if (fixedActionRotation)
    {
      currentAction = actions[round % actions.Count];
    }
    else
    {
      // Calculate total rang
      int total = 0;
      foreach(MonsterAction action in actions)
      {
        if (action == lastAction)
        {
          total += action.weight / 4;
        }
        else
        {
          total += action.weight;
        }
      }

      // Select one from the range
      int roll = Random.Range(0, total);
      foreach (MonsterAction action in actions)
      {
        if (action == lastAction)
        {
          roll -= action.weight / 4;
        }
        else
        {
          roll -= action.weight;
        }
        if (roll <= 0)
        {
          currentAction = action;
          break;
        }
      }
    }
    lastAction = currentAction;
    MonsterAction actionInstance = Instantiate(currentAction);
    int aggression = GetAggresion(round);
    actionInstance.Boost(aggression);
    return actionInstance;
  }

  public int GetAggresion(int round) {
    
    return Mathf.Max(0, ((round + DataManager.instance.saveData.aggroModifier - 1) + aggressionTimer) / Mathf.Max(1, aggressionGrowth));
  }
}

public enum MonsterType { minion, boss };