using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
  public GameManager gameManager;
  public AbilityButton abilityButtonPrefab;
  public List<AbilityButton> buttons = new List<AbilityButton>();
  public List<AbilityTrigger> triggers = new List<AbilityTrigger>();

  public void Init(bool loading)
  {
    List<Ability> abilities = new List<Ability>(DataManager.instance.abilities.ToArray());
    abilities.Sort((x, y) =>
      x.GetSortValue() - y.GetSortValue()
    /*
     (
      (x.sortOrder * 100 + x.numberOfUses + (x.endless ? 20 : 0)) -
      (y.sortOrder * 100 + y.numberOfUses + (y.endless ? 20 : 0))
     )
     */
    );
    foreach(Ability ability in abilities)
    {
      AbilityButton button = Instantiate(abilityButtonPrefab, transform);
      button.abilityManager = this;
      button.ability = ability;
      buttons.Add(button);
    }
    triggers = new List<AbilityTrigger>();
    if (loading)
    {
      triggers = DataManager.instance.saveData.abilityTriggers;
    }
    UpdateButtons();
  }

  public void DisableButtons()
  {
    foreach(AbilityButton button in buttons)
    {
      button.SetEnabled(false);
    }
  }

  public void UpdateButtons()
  {
    foreach(AbilityButton button in buttons)
    {
      button.UpdateStatus();
    }
  }

  public bool AbilityClicked(Ability ability)
  {
    if (ability.CanBeUsed(this) && gameManager.PlayerControl(false))
    {
      ability.Use(this);
      UpdateButtons();
      return true;
    }
    return false;
  }

  public Vector3 GetAbilityButtonPosition(Ability ability){
    foreach(AbilityButton ab in buttons){
      if (ab.ability == ability){
        return ab.transform.position;
      }
    }
    return Vector3.zero;
  }

  public void AbilityCompleted()
  {
    // TODO: Notify managers
  }

  public void AddTrigger(AbilityTrigger trigger)
  {
    triggers.Add(trigger);
  }

  public bool UseTrigger(AbilityTrigger trigger)
  {
    return triggers.Remove(trigger);
  }

  public void TestTriggers()
  {

  }

}
