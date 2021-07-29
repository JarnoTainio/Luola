using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
  public TextObject nameText;
  public TextObject descriptionText;
  public TextObject itemTypeText;
  public GameObject energyContainer;
  public TextMeshProUGUI energyText;
  public List<Image> useImages;
  public Sprite singleUseImage;
  public Sprite useImage;
  public bool showFirstAbility = false;
  public Image image;

  public void Start(){
    if (showFirstAbility && DataManager.instance.abilities.Count > 0){
      image.gameObject.SetActive(true);
      image.sprite = DataManager.instance.abilities[0].icon;
      SetAbility(DataManager.instance.abilities[0]);
    }else{
      image.gameObject.SetActive(false);
    }
  }
  public void Hide()
  {
    gameObject.SetActive(false);
  }

  public void SetItem(object item)
  {
    GemItem gem = item as GemItem;
    if (gem != null)
    {
      SetGem(gem);
      return;
    }

    RewardItem rewardItem = item as RewardItem;
    if (rewardItem != null)
    {
      SetItem(rewardItem);
      return;
    }

    Ability ability = item as Ability;
    if (ability != null)
    {
      SetAbility(ability);
      return;
    }
  }

  public void SetGem(GemItem gem)
  {
    nameText.SetText(gem.GetName());
    descriptionText.SetText(gem.GetName() + "Description");
    itemTypeText.SetText("gem");
    energyContainer.SetActive(false);
    foreach(Image img in useImages){
      img.gameObject.SetActive(false);
    }
    gameObject.SetActive(true);
  }

  public void SetItem(RewardItem item)
  {
    nameText.SetText(item.GetName());
    descriptionText.SetText(item.GetDescription());
    itemTypeText.SetText("item");
    energyContainer.SetActive(false);
    foreach(Image img in useImages){
      img.gameObject.SetActive(false);
    }
    gameObject.SetActive(true);
  }

  public void SetAbility(Ability ability)
  {
    nameText.SetText(ability.abilityName);
    descriptionText.SetText(ability.abilityName + "Description");
    itemTypeText.SetText("ability");

    energyContainer.SetActive(true);
    energyText.text = ability.energyCost.ToString();

    for(int i = 0; i < useImages.Count; i++){
      Image img = useImages[i];
      bool singleUse = ability.refreshPhases[0] == GamePhase.EncounterStart && ability.endless == false;
      img.gameObject.SetActive(ability.endless == false && i < ability.numberOfUses);
      img.sprite = singleUse ? singleUseImage : useImage;
    }

    gameObject.SetActive(true);
  }
}
