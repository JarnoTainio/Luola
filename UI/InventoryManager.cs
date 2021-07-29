using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
  public Transform itemContainer;
  public ItemInfo itemInfo;
  public ItemButton itemButtonPrefab;
  public ItemButton currentItemButton;
  public GemContainer gemContainer;
  public List<GameObject> items;
  public Button nextPageButton;
  public Button lastPageButton;
  private int page;
  public int itemsPerPage = 20;
  [Header("Audio")]
  public AudioSource audioSource;
  public AudioClip itemInfoSound;

  private bool first = true;

  private void Awake()
  {
    first = true;
  }

  private void Start()
  {
    if (first)
    {
      Reload();
    }
  }

  public void ChangePage(bool forward){
    if (forward){
      lastPageButton.interactable = true;
      page +=1;
     
    }else{
      nextPageButton.interactable = true;
      page -= 1;
    }
    ChangePage(page);
  }

  public void ChangePage(int page){
    this.page = page;
    int start = itemsPerPage * page;
    int end = itemsPerPage * (page + 1);

    for(int i = 0; i < items.Count; i++){
      items[i].SetActive(i >= start && i < end);
    }
    if (page == Mathf.Ceil(items.Count / itemsPerPage)){
      nextPageButton.interactable = false;
    }
    if (page == 0){
      lastPageButton.interactable = false;
    }
  }

  public void ItemClicked(ItemButton itemButton)
  {
    if (currentItemButton == itemButton)
    {
      itemButton.SetSelected(false);
      currentItemButton = null;
      itemInfo.Hide();
    }
    else
    {
      audioSource.clip = itemInfoSound;
      audioSource.Play();

      if (currentItemButton)
      {
        currentItemButton.SetSelected(false);
      }
      currentItemButton = itemButton;
      currentItemButton.SetSelected(true);
      itemInfo.SetItem(currentItemButton.item);
    }
  }

  public void SetOpen(bool open)
  {
    if (open)
    {
      Reload();
      gameObject.SetActive(true);
    }
    else
    {
      itemInfo.Hide();
      if (currentItemButton != null){
        currentItemButton.SetSelected(false);
        currentItemButton = null;
      }
      gameObject.SetActive(false);
    }
  }

  public void Reload()
  {
    first = false;
    foreach (GameObject item in items)
    {
      Destroy(item);
    }
    items.Clear();

    DataManager.instance.gemBag.Sort((x, y) =>
      x.color != y.color ? x.color.CompareTo(y.color) : x.type.CompareTo(y.type)
    );
    Dictionary<string, int> gemCount = new Dictionary<string, int>();
    foreach (GemItem gem in DataManager.instance.gemBag){
      string s = gem.type.ToString() + gem.color.ToString();
      if (gemCount.ContainsKey(s) == false){
        gemCount[s] = DataManager.instance.gemBag.FindAll(x => x.color == gem.color && x.type == gem.type).Count;
      }
    }

    foreach (string key in gemCount.Keys)
    {

      GemItem gem = DataManager.instance.gemBag.Find(x => x.type.ToString() + x.color.ToString() == key);
      ItemButton itemButton = Instantiate(itemButtonPrefab, itemContainer);
      Sprite[] spriteArray = { };
      switch (gem.color)
      {
        case GemColor.Red: spriteArray = gemContainer.redGems; break;
        case GemColor.Blue: spriteArray = gemContainer.blueGems; break;
        case GemColor.Green: spriteArray = gemContainer.greenGems; break;
        case GemColor.Yellow: spriteArray = gemContainer.yellowGems; break;
        case GemColor.Gray: spriteArray = gemContainer.grayGems; break;
      }
      Debug.Log(gem.color);
      Sprite sprite = spriteArray[(int)gem.type];
      itemButton.inventoryManager = this;
      itemButton.SetItem(gem, sprite, gemCount[key]);
      items.Add(itemButton.gameObject);
    }
    foreach (RewardItem item in DataManager.instance.items)
    {
      ItemButton itemButton = Instantiate(itemButtonPrefab, itemContainer);
      itemButton.inventoryManager = this;
      itemButton.SetItem(item, item.sprite);
      items.Add(itemButton.gameObject);
    }
    List<Ability> sortedAbilities = new List<Ability>(DataManager.instance.abilities.ToArray());
    sortedAbilities.Sort((x, y) => x.GetSortValue() - y.GetSortValue());
    foreach (Ability ability in sortedAbilities)
    {
      ItemButton itemButton = Instantiate(itemButtonPrefab, itemContainer);
      itemButton.inventoryManager = this;
      itemButton.SetItem(ability, ability.icon);
      items.Add(itemButton.gameObject);
    }
    nextPageButton.gameObject.SetActive(items.Count > itemsPerPage);
    lastPageButton.gameObject.SetActive(items.Count > itemsPerPage);
    ChangePage(0);
  }
}
