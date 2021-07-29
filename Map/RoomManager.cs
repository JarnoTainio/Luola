using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class RoomManager : MonoBehaviour
{
  public MapManager mapManager;
  public EventButton buttonPrefab;
  public Image eventImage;
  public TextObject title;
  public TextObject description;
  public Transform buttonContainer;
  public List<EventButton> buttons = new List<EventButton>();
  public List<EventPhase> events;
  public EventPhase restEvent;
  public EventPhase currentEvent;
  public InventoryManager inventory;
  public ItemInfo itemInfo;
  public Button confirmSelectionButton;
  public int testingIndex = -1;
  public AssetList eventList;

  private void Awake()
  {
    for (int i = 0; i < 5; i++)
    {
      EventButton button = Instantiate(buttonPrefab, buttonContainer);
      button.GetComponent<ButtonAudio>().audioSource = mapManager.audioSource;
      button.index = i;
      button.roomManager = this;
      buttons.Add(button);
      button.gameObject.SetActive(false);
    }
  }

  public void StartRestEvent()
  {
    gameObject.SetActive(true);
    SetEvent(restEvent);
  }

  public void StartEvent()
  {
    gameObject.SetActive(true);
    SaveData saveData = DataManager.instance.saveData;
    int floor = saveData.floor;
    List<EventPhase> filteredEvents = events.FindAll(e => e.CanBeEvent(floor) );

    if (filteredEvents.Count == 0)
    {
      filteredEvents = events;
    }
    EventPhase eventPhase = filteredEvents[Random.Range(0, filteredEvents.Count)];
    saveData.seenEvents.Add(eventPhase.name);
    if (testingIndex < 0){
      SetEvent(eventPhase);
    }else{
      SetEvent(events[testingIndex]);
    }
  }

  public void EndEvent()
  {
    gameObject.SetActive(false);
    DataManager.instance.saveData.SaveMap(mapManager);
  }

  public void SetEvent(string eventName){
    foreach(Object o in eventList.items){
      if (o.name == eventName){
        SetEvent((EventPhase)o);
      }
    }
  }

  public void SetEvent(EventPhase eventPhase)
  {
    gameObject.SetActive(true);
    confirmSelectionButton.interactable = false;
    currentIndex = -1;
    currentEvent = eventPhase;

    foreach (ResourceAmount res in eventPhase.triggers)
    {
      mapManager.ModifyResource(res);
    }
    bool reloadInventory = false;
    if (currentEvent.item != null)
    {
      DataManager.instance.items.Add(eventPhase.item);
      RewardItemAttribute ita = (RewardItemAttribute)currentEvent.item;
      ResourceAmount ra = new ResourceAmount();
      ra.resource = ita.equipResource;
      ra.amount = ita.equipAmount;
      mapManager.ModifyResource(ra, true);
      reloadInventory = true;
    }
    if (currentEvent.ability != null)
    {
      DataManager.instance.abilities.Add(eventPhase.ability.ability);
      reloadInventory = true;
    }

    if (reloadInventory)
    {
      inventory.Reload();
    }

    if (eventPhase.combatDifficulty >= 0){
      DataManager.instance.saveData.roomDifficulty = eventPhase.combatDifficulty;
      mapManager.fadePanel.StartFadeOut(DataManager.instance.StartCombat);
      return;
    }

    if (eventPhase.events.Count == 0)
    {
      EndEvent();
      return;
    }

    if (eventPhase.eventSprite)
    {
      eventImage.sprite = eventPhase.eventSprite;
    }
    description.SetText(eventPhase.description);
    if (eventPhase.title != null)
    {
      title.SetText(eventPhase.title);
    }

    if (eventPhase.events.Count == 1){
      SetEvent(eventPhase.events[0]);
      return;
    }
    for (int i = 0; i < buttons.Count; i++)
    {
      if (i < eventPhase.events.Count)
      {
        EventPhase e = eventPhase.events[i];
        buttons[i].gameObject.SetActive(true);
        buttons[i].Selected(false);
        bool selectable = true;
        if (e.requirements != null){
          foreach(ResourceAmount t in e.requirements){
            selectable = selectable && t.Required(DataManager.instance.GetResource(t.resource));
          }
        }
        foreach (ResourceAmount res in e.triggers)
        {
          if (res.amount < 0){

            int v = DataManager.instance.GetAmount(res);
            if (res.resource == Resource.Life){
              selectable &= v + DataManager.instance.saveData.life > 0;
            }else if (res.resource == Resource.MaxLife){
              selectable &= v + DataManager.instance.saveData.maxLife > 0;
            }else if (res.resource == Resource.Gold){
              selectable &= v + DataManager.instance.saveData.gold >= 0;
            }
          }
          else if (res.resource == Resource.Life){
            selectable &= DataManager.instance.saveData.life < DataManager.instance.saveData.maxLife;
          }
          Debug.Log(selectable);
        }
        if (e.buttonText != null && e.buttonText != "")
        {
          buttons[i].SetText(e.buttonText, selectable);
        }
        else
        {
          buttons[i].SetText(e.triggers.ToArray(), selectable);
        }
      }
      else
      {
        buttons[i].gameObject.SetActive(false);
      }
    }
    DataManager.instance.saveData.eventName = eventPhase.name;
    DataManager.instance.saveData.SaveEvent(mapManager);
  }
  public int currentIndex = -1;
  public void OptionClicked(int index)
  {
    currentIndex = Hovering(index);
    for (int i = 0; i < buttons.Count; i++)
    {
      buttons[i].Selected(i == currentIndex);
    }
    confirmSelectionButton.interactable = currentIndex > -1;

  }

  public void ConfirmSelection()
  {
    itemInfo.gameObject.SetActive(false);
    SetEvent(currentEvent.events[currentIndex]);
  }

  public int Hovering(int index)
  {
    ExitHovering(index);
    if (index == currentIndex)
    {
      return -1;
    }
    EventPhase eventPhase = currentEvent.events[index];
    if (eventPhase.item != null)
    {
      itemInfo.gameObject.SetActive(true);
      itemInfo.SetItem(eventPhase.item);
    }
    else if (eventPhase.ability != null)
    {
      itemInfo.gameObject.SetActive(true);
      itemInfo.SetItem(eventPhase.ability);
    }
    return index;
  }

  public void ExitHovering(int index)
  {
    itemInfo.gameObject.SetActive(false);
  }
}
