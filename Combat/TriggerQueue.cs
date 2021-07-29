using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerQueue : MonoBehaviour
{
  public GameManager gameManager;
  public Image image;
  public float startingSize = .5f;
  public float targetSize = 1f;
  public float duration = .5f;
  private List<TriggerEvent> queue = new List<TriggerEvent>();
  private TriggerEvent currentEvent;
  
  public bool IsReady()
  {
    return queue.Count == 0;
  }

  public void AddTrigger(RewardItem item)
  {
    queue.Add(new TriggerEvent(item));
    if (currentEvent == null && gameManager.combatManager.hero.life > 0)
    {
      StartCoroutine(Trigger());
    }
  }

  public IEnumerator Trigger()
  {
    // Start event
    currentEvent = queue[0];
    image.sprite = currentEvent.item.sprite;
    image.color = new Color(1, 1, 1, 0);
    image.gameObject.SetActive(true);
    int amount = 0;
    Resource resource = Resource.Attack;
    bool usesToken = false;
    if (currentEvent.item is RewardItemAttribute item)
    {
      if (DataManager.instance.saveData.damageTakenDoubleTrigger && item.trigger == global::Trigger.DamageTaken){
        amount = item.amount * 2;
      }else{
        amount = item.amount;
      }
      resource = item.resource;
      usesToken = item.triggerUsesTokens;
    }
    if (IsUsefull(resource, amount)){
      float spawn = duration / 4f;
      float step = duration / (4 * amount);
      float nextSpawn = spawn;

      float f = 0f;
      while (f < duration)
      {
        f += Time.deltaTime;
        float s = f / duration;
        if (s >  nextSpawn && amount  > 0)
        {
          nextSpawn += step;
          gameManager.ModifyResource(resource, 1, usesToken, image.transform.position);
          amount--;
        }
        image.color = new Color(1, 1, 1, 1f - Mathf.Abs(s - .5f) * 3f);
        float sc = startingSize + s * (targetSize - startingSize);
        image.transform.localScale = new Vector3(sc, sc, sc);
        yield return null;
      }
      image.gameObject.SetActive(false);


    }
    // End of event
    queue.Remove(currentEvent);
    currentEvent = null;
    
    if (queue.Count > 0 && gameManager.combatManager.hero.life > 0)
    {
      StartCoroutine(Trigger());
    }
  }

  public bool IsUsefull(Resource resource, int amount){
    if (resource == Resource.Life && amount > 0){
      return DataManager.instance.saveData.life < DataManager.instance.saveData.maxLife;
    }
    return true;
  }

  public void Stop()
  {
    StopAllCoroutines();
  }
  
}

public class TriggerEvent
{
  public RewardItem item;

  public TriggerEvent(RewardItem item)
  {
    this.item = item;
  }
}

