using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButton : MonoBehaviour
{
  public AbilityManager abilityManager;
  public Ability ability;
  public Image frame;
  public Image icon;
  public TextMeshProUGUI cost;

  [Header("UseIndicator")]
  public Transform useContainer;
  public Image imagePrefab;
  public Image costImage;
  public Sprite useSprite;
  public Sprite useOnceSprite;
  public List<Image> useImages;

  public Color disabledColor;
  public Color costColor;
  public Sprite lifeCostSprite;
  public Sprite goldCostSprite;

  private bool activated = false;
  private bool growing = true;
  private float count = 0f;

  public void Awake(){
    costColor = cost.color;
  }

  public void Start()
  {
    icon.sprite = ability.icon;
    Refresh(GamePhase.EncounterStart);
    if (ability.numberOfUses > 1)
    {
      useImages = new List<Image>();
      for (int i = 0; i < ability.numberOfUses; i++)
      {
        Image img = Instantiate(imagePrefab, useContainer);
        img.sprite = (ability.refreshPhases[0] == GamePhase.EncounterStart) ? useOnceSprite : useSprite;
        useImages.Add(img);
      }
    }

    costImage.gameObject.SetActive(false);
    ResourceAmount[] costs = ability.GetCost();
    if (costs.Length > 0){
      if (costs[0].resource == Resource.Energy){
        SetCost(costs[0].amount, new Color(0,0,0,0), costImage.sprite, costImage.color);

      }else if (costs[0].resource == Resource.Gold){
        SetCost(-costs[0].amount, new Color(1,1,1,1), goldCostSprite, Color.white);

      }else if (costs[0].resource == Resource.Life){
      SetCost(-costs[0].amount, new Color(1,1,1,1), lifeCostSprite, Color.white);
    }
    }
    costColor = cost.color;
  }

  private void SetCost(int value, Color spriteColor, Sprite sprite, Color textColor){
    costImage.gameObject.SetActive(value != 0);
    if (value != 0){
      costImage.color = spriteColor;
      costImage.sprite = sprite;
      cost.text = value.ToString();
      cost.color = textColor;
    }
  }

  public void SetEnabled(bool enabled)
  {
    if (enabled)
    {
      icon.color = Color.white;
      cost.color = costColor;
    }
    else if (enabled == false)
    {
      icon.color = disabledColor;
      cost.color = disabledColor;
    }

    if (ability.IsActive(abilityManager.gameManager))
    {
      frame.color = Color.yellow;
      icon.color = Color.white;
      activated = true;
    }
    else
    {
      frame.color = Color.white;
      activated = false;
    }
  }

  private void Update()
  {
    if (activated)
    {
      if (growing){
        count += Time.deltaTime / 2f;
        if (count >= 1f)
        {
          count = 1f;
          growing = false;
        }
      }
      else
      {
        count -= Time.deltaTime;
        if (count <= 0)
        {
          count = 0;
          growing = true;
        }
      }
      float f = count / 2 + 0.5f;
      frame.color = new Color(1, f, f);
      icon.color = new Color(1, f, f);
    }
  }

  public void UpdateStatus()
  {
    SetEnabled(ability.CanBeUsed(abilityManager));
    if (ability.numberOfUses > 1)
    {
      for (int i = 0; i < useImages.Count; i++)
      {
        useImages[i].color = ability.usesLeft > i ? Color.white : new Color(0.25f, 0.25f, 0.25f);
      }
    }
  }

  public void Refresh(GamePhase gamePhase)
  {
    ability.Refresh(gamePhase);

  }

  public void OnClick()
  {
    abilityManager.AbilityClicked(ability);
  }
}
