using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameManager : MonoBehaviour
{
  public Image image;
  public Button continueButton;
  public GameObject lockedMessageObject;
  public TextObject description;
  public TextObject heroesUnlockedText;
  public TextObject unlockedText;
  public Button adButton;
  public List<Hero> heroes;
  public AssetList heroList;
  private Dictionary<string, string> translationValues;
  private int currentIndex;
  public FadeIn fadeIn;
  public Color textColor;
  public Sprite[] difficultyAwards;
  public Image[] stars;
  public ToggleButton[] difficultyButtons;
  public InfoDialog infoDialog;
  public bool unlockAllHeroes = false;

  public int[] requiredAdCount;

  private int unlockedCount = 0;
  private void Start()
  {
    DataManager.instance.saveData.UpdateSaveFormat();
    fadeIn.StartFadeIn();
    HeroAsset lastHero = DataManager.instance.saveData.GetHero(heroList);
    if (lastHero == null){
      currentIndex = 0;
    }else{
      for(int i = 0; i < heroList.items.Length; i++){
        HeroAsset h = (HeroAsset)heroList.items[i];
        if (lastHero == h){
          currentIndex = i;
          break;
        }
      }
    }

    unlockedCount = 0;
    for(int i = 0; i < difficultyButtons.Length; i++){
      difficultyButtons[i].Toggle(i == 1);
    }
    SetDifficulty(1);
    for (int i = 0; i < heroList.items.Length; i++)
    {
      HeroAsset h = (HeroAsset)(heroList.items[i]);
      h.unlocked = unlockAllHeroes ? 1 : DataManager.instance.saveData.unlockedHeroes[i];
      unlockedCount += DataManager.instance.saveData.unlockedHeroes[i] > 0 ? 1 : 0;
      //heroList[i] = h;
      if (currentIndex == 0 && h.unlocked == 2)
      {
        currentIndex = i;
      }
    }
    heroesUnlockedText.SetText("heroesUnlocked", new Dictionary<string, string>(){
      {"count", unlockedCount.ToString()}, {"total", heroList.items.Length.ToString()}
    });
    translationValues = new Dictionary<string, string>();
    translationValues["overkill"] = DataManager.instance.saveData.totalOverkill.ToString();
    translationValues["adds"] = DataManager.instance.saveData.addsWatched.ToString();
    ShowHero();
  }

  public void NextSprite(bool forward)
  {
    currentIndex += forward ? 1 : -1;
    if (currentIndex >= heroList.items.Length)
    {
      currentIndex = 0;
    }
    else if (currentIndex < 0)
    {
      currentIndex = heroList.items.Length- 1;
    }
    ShowHero();
  }

  private void ShowHero()
  {
    description.textObject.color = Color.yellow;
    adButton.gameObject.SetActive(false);
    HeroAsset currentHero = (HeroAsset)heroList.items[currentIndex];

    image.sprite = currentHero.sprite;

    continueButton.gameObject.SetActive(currentHero.isPlayable);
    lockedMessageObject.gameObject.SetActive(currentHero.isPlayable == false);

    string[] parts = currentHero.descriptionKey.Split();
    if (parts.Length > 1 && currentHero.unlocked == 0){
      adButton.gameObject.SetActive(true);
      if (parts[1] == "0"){
        parts = new string[]{"heroUnlockSingleAdd"};
      }else{
        translationValues["required"] = requiredAdCount[int.Parse(parts[1])].ToString();
      }
    }
    string text = "";

    // Unlocked hero
    if (currentHero.unlocked > 0)
    {
      for(int i = 0; i < stars.Length; i++){
        stars[i].gameObject.SetActive(true);
        stars[i].color = Color.black;

        if (DataManager.instance.saveData.heroVictories.Contains($"{currentHero.descriptionKey}_{i}")){
          stars[i].color = Color.white;
        }
      }

      image.color = Color.white;
      continueButton.interactable = true;
      string s = parts.Length > 1 ? parts[0] + parts[1] : parts[0];
      text = Translation.GetTranslation(s + "Description", translationValues);
      // New hero
      if (currentHero.unlocked == 2)
      {
        unlockedText.SetText("unlocked");
        DataManager.instance.saveData.unlockedHeroes[currentIndex] = 1;
      }
      // Old hero
      else
      {
        unlockedText.SetRawText("");
      }
    }
    // Locked hero
    else
    {
      for(int i = 0; i < stars.Length; i++){
        stars[i].gameObject.SetActive(false);
      }
      image.color = Color.black;
      unlockedText.SetRawText("");
      text = Translation.GetTranslation(parts[0], translationValues);
      continueButton.interactable = false;
      description.textObject.color = textColor;
    }

    description.SetRawText(text);
  }

  public void Continue()
  {
    HeroAsset currentHero = (HeroAsset)heroList.items[currentIndex];
    if (currentHero.unlocked > 0)
    {
      DataManager.instance.saveData.heroIndex = currentHero.descriptionKey;
      SetupHero();
      fadeIn.StartFadeOut(DataManager.instance.StartNewRun);
    }
  }

  public void AddWatched(){
    List<int> unlocked = DataManager.instance.TestHeroUnlocking(UnlockKey.AddsWatched, ++DataManager.instance.saveData.addsWatched);
    DataManager.instance.saveData.SaveNewGame();
    
    if (unlocked.Count > 0){
      unlockedCount++;
      heroesUnlockedText.SetText("heroesUnlocked", new Dictionary<string, string>(){
      {"count", unlockedCount.ToString()}, {"total", heroList.items.Length.ToString()}
    });
    }
    translationValues["adds"] = DataManager.instance.saveData.addsWatched.ToString();
    for (int i = 0; i < heroList.items.Length; i++)
    {
      HeroAsset h = (HeroAsset)heroList.items[i];
      h.unlocked = DataManager.instance.saveData.unlockedHeroes[i];
      //heroes[i] = h;
    }
    ShowHero();
  }

  public void SetupHero(){
    DataManager dataManager = DataManager.instance;
    SaveData saveData = dataManager.saveData;
    HeroAsset data = (HeroAsset)heroList.items[currentIndex];

    saveData.maxLife = saveData.life = data.life;
    saveData.gold = data.gold;
    saveData.actionModifier = data.actionModifier;
    saveData.roomExtraGold = data.roomGold;
    saveData.roomExtraPower = data.roomPower;
    saveData.maxLifeGainModifier = data.maxLifeGainModifier;
    saveData.requiredPowerModifier = data.requiredPowerModifier;
    saveData.rewardPriceModifier = data.rewardPriceModifier;
    saveData.attackDecay = data.attackDecay;
    saveData.monsterLifeModifier = data.monsterLifeModifier;

    saveData.energyShield = data.energyShield;
    saveData.lifeGrowthFromPower = data.lifeGrowthFromPower;
    saveData.bloodEnergy = data.bloodEnergy;
    saveData.damageTakenDoubleTrigger = data.damageTakenDoubleTrigger;
    saveData.emptyPowerToPotential = data.emptyPowerToPotential;
    saveData.emptyGoldToPotential = data.emptyGoldToPotential;

    saveData.floorHeight = data.floorHeight;
    saveData.gemBoardHeight = data.gemBoardHeight;
    saveData.gemBoardWidth = data.gemBoardWidth;
    saveData.matchRequired = data.matchRequired;
    saveData.disabledGameElements = data.disabledGameElements;

    saveData.purpleGemWeight = data.purpleGemWeight;
    saveData.redGemWeight = data.redGemWeight;
    saveData.blueGemWeight = data.blueGemWeight;

    dataManager.startingAbilities = data.abilities;
    dataManager.startingItems = data.items;
    dataManager.startingGems = data.gems;
  }

  public void SetDifficulty(int difficulty){
    DataManager.instance.difficulty = difficulty;
    //string[] str = new string[]{"Easy", "Medium", "Hard"};
    //infoDialog.title.SetText($"difficulty{str[difficulty]}Title");
    //infoDialog.text.SetText($"difficulty{str[difficulty]}Text");
  }

}

[System.Serializable]
public struct Hero
{
  public int unlocked;
  public string descriptionKey;
  public int life;
  public int gold;
  public int roomGold;
  public int roomPower;

  [HeaderAttribute("Modifiers")]
  public int actionModifier;
  public int attackDecay;
  public float maxLifeGainModifier;
  public int requiredPowerModifier;
  public float rewardPriceModifier;
  public float monsterLifeModifier;
  public int floorHeight;
  public List<GameElement> disabledGameElements;

  [HeaderAttribute("Triggers")]
  public bool energyShield;
  public bool lifeGrowthFromPower;
  public bool bloodEnergy;

  [HeaderAttribute("Starting gear")]
  public Ability[] abilities;
  public RewardItem[] items;
  public GemItem[] gems;
}
