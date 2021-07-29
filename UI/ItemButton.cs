using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
  public InventoryManager inventoryManager;
  public Image itemImage;
  public Image background;
  public Reward reward;
  public object item;
  public TextMeshProUGUI counText;

  public void SetSelected(bool isSelected)
  {
    background.gameObject.SetActive(isSelected);
  }

  public void OnMouseDown()
  {
    inventoryManager.ItemClicked(this);
  }

  public void SetItem(object item, Sprite sprite, int count = 1)
  {
    this.item = item;
    itemImage.sprite = sprite;
    counText.text = count > 1 ? count.ToString() : "";
    SetSelected(false);
  }
}
