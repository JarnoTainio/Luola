using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
  public RewardManager rewardManager;
  public Image item;
  public Image background;
  public Image frameImage;
  public Sprite selectedBackground;
  public Sprite unselectedBackground;

  public Reward reward;

  public void SetSelected(bool isSelected)
  {
    background.sprite = isSelected ? selectedBackground : unselectedBackground;
    frameImage.color = isSelected ? Color.green : Color.white;
  }

  public void OnMouseDown()
  {
     rewardManager.RewardClicked(this);
  }

  public void SetItem(Reward reward)
  {
    this.reward = reward;
    item.sprite = reward.GetSprite();
    SetSelected(false);
  }
}
