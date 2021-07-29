using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
  [Header("References")]
  public GameManager gameManager;
  public RewardButton rewardButtonPrefab;
  public List<RewardButton> rewardButtons;
  public ItemInfo itemInfo;
  public Button continueButton;
  public AudioSource audioSource;
  public TextObject hintText;
  public Image powerImage;
  public TextMeshProUGUI powerText;

  [Header("GoldChest")]
  public Button chestButton;
  public Image chestImage;
  public TextMeshProUGUI chestCostText;
  public Sprite openedChest;
  public Sprite closedChest;
  public int chestCost;
  public int rewards = 1;
  public AudioClip openChestSound;
  public bool chestBought;

  [Header("Reroll")]
  public Button rerollButton;
  public Image rerollImage;
  public TextMeshProUGUI rerollText;
  public int rerollCost;
  public bool rerollBought;

  [Header("RewardList")]
  public List<Reward> rewardItems;
  public int rewardCount = 4;
  List<RewardButton> selectedRewards;
  public RewardType[] rewardGroup;
  private bool continueClicked = false;
  private List<Reward> fallbackRewards;
  public void Awake()
  {
    DataManager.instance.loading = false;
    SaveData saveData = DataManager.instance.saveData;
    int profileLevel = DataManager.instance.saveData.profileLevel;
    rewardItems = new List<Reward>();
    fallbackRewards = new List<Reward>();

    rewardCount += (saveData.floor +  profileLevel) / 5;
    bool eliteRewards = false;

    if (saveData.bossRoom || saveData.roomTag == RoomTag.Elite){
      rewardCount++;
      eliteRewards = true;
    }

    rewardCount = Mathf.Min(rewardCount, 8);
    // Adjust itemInfo position if only one row of rewards
    if (rewardCount <= 4){
      itemInfo.transform.localPosition += new Vector3(0, 60,0);
    }
    
    List<Object> loaded_rewards = new List<Object>();
    loaded_rewards.AddRange(Resources.LoadAll("ScriptableObjects/Rewards/Gems", typeof(RewardGem)));
    loaded_rewards.AddRange(Resources.LoadAll("ScriptableObjects/Rewards/Abilities", typeof(RewardAbility)));
    loaded_rewards.AddRange(Resources.LoadAll("ScriptableObjects/Rewards/Items", typeof(RewardItem)));
    int floorNumber = DataManager.instance.saveData.floor;
    foreach (Object obj in loaded_rewards)
    {
      Reward r = (Reward)obj;
      if (
        r.isReward
        && r.level <= profileLevel
        && r.minFloor <= floorNumber && r.maxFloor >= floorNumber
        && !saveData.seenRewards.Contains(r.name)
        && (r.GetRewardType() == RewardType.Gem || (r.eliteReward ? eliteRewards : true))
        )
      {
        rewardItems.Add(r);
      }
      if (r.GetRewardType() == RewardType.Gem){
        bool notDisabled = true;
        if (r.gameElementType != null){
          foreach(GameElement el in  r.gameElementType){
            if (saveData.disabledGameElements.Contains(el)){
              notDisabled = false;
            }

          }
        }
        if (notDisabled){
          fallbackRewards.Add(r);
        }
      }
    }

    rewards = 1;
    selectedRewards = new List<RewardButton>();
    rewardButtons = new List<RewardButton>();

    int roll = Random.Range(0, 5);
    bool roomForAbility = saveData.abilities.Length < 7;
    rewardGroup = new RewardType[rewardCount];
    int last = rewardGroup.Length - 1;
    int second = last - 1;
    for(int i = 0; i < second; i++){
      if (i < 2){
        rewardGroup[i] = RewardType.Gem;
      }else{
        int r = Random.Range(0,6);
        if (r < 3 ){
          rewardGroup[i] = RewardType.Gem;
        }
        else {
          rewardGroup[i] = RewardType.Item;
        }
      }
    }
    switch (roll) {
      case 0:
        rewardGroup[last] = roomForAbility ? RewardType.Ability : RewardType.Item;
        break;
      case 1:
        rewardGroup[last] = RewardType.Item;
        break;
      case 2:
        rewardGroup[second] = RewardType.Item;
        rewardGroup[last] = roomForAbility ? RewardType.Ability : RewardType.Item;
        break;
      case 3:
        rewardGroup[second] = RewardType.Item;
        rewardGroup[last] = RewardType.Item;
        break;
      case 4:
        rewardGroup[second] = RewardType.Item;
        rewardGroup[last] = roomForAbility ? RewardType.Ability : RewardType.Item;
        break;
    }
    List<Reward> legalRewards = rewardItems.FindAll(r => r.CanBeReward(gameManager));
    List<GemColor> colors = new List<GemColor>();
    for (int i = 0; i < rewardCount; i++)
    {
      List<Reward> rewards = legalRewards.FindAll(r =>
           r.GetRewardType() == rewardGroup[i]
        );
      if (rewards.Count == 0)
      {
        rewards = legalRewards;
      }
      RewardButton rb = Instantiate(rewardButtonPrefab, transform);
      rb.rewardManager = this;
      rewardButtons.Add(rb);

      // Get random reward
      Reward item = GetRandomReward(rewards);
      item.name = item.name.Replace("(Clone)", "");

      // Add reward to seen rewards if it is not a gem
      if (item.GetRewardType() != RewardType.Gem)
      {
        DataManager.instance.saveData.seenRewards.Add(item.name);
      
      // If reward is gem and color is already present, then reroll the gem once
      }else{
        GemColor color = ((RewardGem)item).gem.color;
        if (colors.Contains(color)){
          item = GetRandomReward(rewards);
          item.name = item.name.Replace("(Clone)", "");
        }
        colors.Add(color);
      }

      legalRewards.Remove(item);
      rb.SetItem(item);
    }

    SortRewards();

    chestImage.sprite = closedChest;
    chestCost = (int)Mathf.Floor(((saveData.floor + 1) * 3 + saveData.rewardsBought * 2) * saveData.rewardPriceModifier);
    chestCostText.text = chestCost.ToString();
    int gold = DataManager.instance.saveData.gold;
    chestButton.interactable = chestCost <= gold;
    rerollButton.interactable = rerollCost <= gold;
    if (rerollCost < gold)
      rerollImage.color = new Color(.5f, .5f, .5f);

    continueButton.interactable = false;
    UpdateHint();
    chestBought = false;
    rerollBought = false;

    powerText.text = saveData.powerLevel.ToString();
    powerImage.fillAmount = ((float)saveData.power) / saveData.GetRequiredPower();
  }

  public void BuyChest()
  {
    chestBought = true;
    gameManager.ModifyResource(Resource.Gold, -chestCost);
    DataManager.instance.saveData.rewardsBought++;
    rewards++;
    DisableChest();
    audioSource.clip = openChestSound;
    audioSource.Play();
    UpdateHint();
    hintText.textObject.color = Color.green;
    DataManager.instance.saveData.SaveReward(gameManager);
  }

  public void BuyReroll()
  {
    rerollBought = true;
    gameManager.ModifyResource(Resource.Gold, -rerollCost);
    DisableReroll();

    int roll = Random.Range(0, 3);
    List<Reward> legalRewards = rewardItems.FindAll(r => r.CanBeReward(gameManager));
    for (int i = 0; i < rewardCount; i++)
    {
      RewardButton rb = rewardButtons[i];
      List<Reward> rewards = legalRewards.FindAll(r => r.GetRewardType() == rewardGroup[i]);
      if (rewards.Count == 0)
      {
        rewards = legalRewards.FindAll(r => r.GetRewardType() == RewardType.Gem);
      }

      Reward item = GetRandomReward(rewards);
      legalRewards.Remove(item);
      rb.SetItem(item);
    }
    selectedRewards.Clear();
    itemInfo.Hide();
    DataManager.instance.saveData.SaveReward(gameManager);
  }

  public void DisableReroll()
  {
    rerollButton.interactable = false;
    rerollImage.color = new Color(.4f, .4f, .4f);
    selectedRewards.Clear();
    rerollText.gameObject.SetActive(false);
  }

  public void DisableChest()
  {
    chestImage.sprite = openedChest;
    chestCostText.gameObject.SetActive(false);
    chestButton.image.color = Color.green;
    continueButton.interactable = false;
    chestButton.enabled = false;
    UpdateHint();
  }

  public void UpdateHint()
  {
    hintText.textObject.color = Color.white;
    hintText.enabled = rewards > selectedRewards.Count;
    hintText.SetText(rewards == 1 ? "chooseReward" : "chooseTwoRewards");
  }

  public void RewardClicked(RewardButton reward)
  {
    if (continueClicked){
      return;
    }

    if (selectedRewards.Contains(reward))
    {
      reward.SetSelected(false);
      selectedRewards.Remove(reward);
      itemInfo.Hide();
    }
    else
    {
      RewardType type = reward.reward.GetRewardType();

      if (type == RewardType.Gem){
        itemInfo.SetGem(((RewardGem)reward.reward).gem);
      } else if (type == RewardType.Ability){
        itemInfo.SetAbility(((RewardAbility)reward.reward).ability);
      }
      else{
        itemInfo.SetItem((RewardItem)reward.reward);
      }
      selectedRewards.Add(reward);
      reward.SetSelected(true);
      if (selectedRewards.Count > rewards)
      {
        selectedRewards.Remove(selectedRewards[0]);
      }
    }

    foreach (RewardButton rb in rewardButtons)
    {
      rb.SetSelected(selectedRewards.Contains(rb));
    }

    continueButton.interactable = selectedRewards.Count == rewards;
    UpdateHint();
  }

  public void SelectionConfirmed()
  {
    continueClicked = true;
    foreach(RewardButton button in selectedRewards)
    {
      button.reward.Equip(gameManager);
    }
    chestButton.interactable = false;
    rerollButton.interactable = false;
    continueButton.interactable = false;
    StartCoroutine(ReturnToMap());
  }

  public IEnumerator ReturnToMap(){
    while(gameManager.announcement.IsActive()){
      yield return null;
    }
    DataManager.instance.ReturnToMap();
  }

  public Reward GetRandomReward(List<Reward> rewards)
  {
    int total = 0;
    for (int i = 0; i < rewards.Count; i++)
    {
      total += rewards[i].weight;
    }

    int roll = Random.Range(0, total);
    for (int i = 0; i < rewards.Count; i++)
    {
      roll -= rewards[i].weight;
      if (roll < 0)
      {
        return rewards[i];
      }
    }
    return fallbackRewards[Random.Range(0, fallbackRewards.Count)];
  }

  public void SortRewards(){
    // Sort rewards (very unoptimized sort)
    for(int i = 0; i < rewardCount -1; i++){
      if (rewardButtons[i].reward.GetSortValue() > rewardButtons[i + 1].reward.GetSortValue()){
        Reward temp = rewardButtons[i + 1].reward;
        rewardButtons[i + 1].SetItem(rewardButtons[i].reward);
        rewardButtons[i].SetItem(temp);
        i = 0;
      }
    }
  }


}
