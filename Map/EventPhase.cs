using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Event", menuName = "GemRPG/Event")]
public class EventPhase : ScriptableObject
{
  [Header("Conditions")]
  [Range(0, 2)]
  public int minFloor = 0;
  [Range(0, 2)]
  public int maxFloor = 2;
  public GameElement[] gameElementType;

  [Header("Button")]
  public string buttonText;
  public Sprite eventSprite;

  [Header("Texts")]
  public string title;
  public string description;

  [Header("Effects")]
  public List<ResourceAmount> triggers = new List<ResourceAmount>();
  public RewardItem item;
  public RewardAbility ability;
  [Range(-1, 100)]
  public int combatDifficulty = -1;
  public List<ResourceAmount> requirements = new List<ResourceAmount>();

  [Header("Linked events")]
  public List<EventPhase> events = new List<EventPhase>();

  public bool CanBeEvent(int floor){
    if (minFloor > floor && maxFloor < floor){
      return false;
    }
    if (gameElementType != null && gameElementType.Length > 0){
        foreach(GameElement e in gameElementType){
        if (DataManager.instance.saveData.disabledGameElements.Contains(e)){
          return false;
        }
      }
    }
    return DataManager.instance.saveData.seenEvents.Contains(name) == false;
  }
}
