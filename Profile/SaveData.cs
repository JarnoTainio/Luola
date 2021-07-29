using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
  [Header("Profile")]
  public string name;
  public int profileLevel = 1;
  public int profileExperience = 0;
  public int profileTime = 0;
  public int totalOverkill = 0;
  public int totalRuns = 0;
  public int victoryCount = 0;
  public int addsWatched = 0;
  public int[] unlockedHeroes = new int[32];
  public List<string> heroVictories = new List<string>();

  [Header("Current Run")]
  public int seed = 21 * 9 * 666;
  public int difficulty = 0;
  public int floorHeight = 8;
  public int room = 1;
  public int floor = 0;
  public int gold = 0;
  public int maxLife = 20;
  public int life = 20;
  public int gemBoardWidth = 6;
  public int gemBoardHeight = 6;
  public int matchRequired = 3;
  public int damageTaken = 0;
  public int rewardsBought = 0;
  public int power;
  public int powerLevel = 1;
  public int overkill = 0;
  public List<GameElement> disabledGameElements;
  public List<RoomVector> rooms;
  public List<string> seenEvents;
  public List<string> seenRewards;
  public int bossKills = 0;
  public int roomExtraGold = 0;
  public int roomExtraPower = 0;
  public int aggroModifier = 0;
  public int actionModifier = 0;
  public int requiredPowerModifier = 0;
  public float monsterLifeModifier = 1f;
  public float rewardPriceModifier = 1f;
  public float maxLifeGainModifier = 1f;
  public int defenceDecay = 0;
  public int attackDecay = 0;
  public bool energyShield = false;
  public bool lifeGrowthFromPower = false;
  public bool bloodEnergy = false;
  public bool damageTakenDoubleTrigger = false;
  public bool emptyPowerToPotential = false;
  public bool emptyGoldToPotential = false;
  public float totalTime = 0f;
  public string heroIndex = "";
  public int redGemWeight = 100;
  public int blueGemWeight = 100;
  public int purpleGemWeight = 0;
  public int noGoldGemWeight = 35;
  public int noPowerGemWeight = 35;

  [Header("Room")]
  public int roomX;
  public int roomY;
  public string eventName;
  public int roomDifficulty;
  public int roomGold;
  public int roomPower;
  public bool bossRoom;
  public int roomDanger;
  public RoomTag roomTag;

  [Header("Inventory")]
  public GemItem[] gems;
  public string[] items;
  public AbilitySave[] abilities;

  [Header("Rewards")]
  public bool chestBought;
  public bool rerollBought;
  public RewardSave[] rewardOptions;

  [Header("Combat")]
  public string roomName;
  public int roundNumber;
  public int movesRemaining;
  public GemItem[] gemBoard;
  public int remainingGold;
  public int remainingPower;
  public List<AbilityTrigger> abilityTriggers;

  [Header("Player")]
  public int playerAttack;
  public int playerDefence;
  public int playerEnergy;

  [Header("Monster")]
  public string monsterName;
  public int monsterMaxLife;
  public int monsterLife;
  public int monsterAttack;
  public int monsterDefence;
  public int monsterAggressionTimer;
  public int monsterAggressionGrowth;

  public void NewProfile(string name)
  {
    this.name = name;
    profileLevel = 1;
    profileExperience = 0;
    profileTime = 0;
    totalRuns = 0;
    victoryCount = 0;
    addsWatched = 0;
    for (int i = 0; i < unlockedHeroes.Length; i++)
    {
      unlockedHeroes[i] = i == 0 ? 1 : 0;
    }
    totalOverkill = 0;
    heroVictories = new List<string>();
    NewRun(true, 0);
  }

  public void NewRun(bool defaultValues, int difficulty)
  {
    this.difficulty = difficulty;
    rooms = new List<RoomVector>();
    rooms.Clear();
    rooms.Add(new RoomVector(0, 0, 0));
    seenEvents = new List<string>();
    seenRewards = new List<string>();
    monsterName = null;
    room = 1;
    floor = 0;
    rewardsBought = 0;

    damageTaken = 0;
    powerLevel = 1;
    power = 0;
    overkill = 0;
    roomName = defaultValues ? "newGame" : "combat";
    roomX = -1;
    roomY = -1;
    eventName = "";
    roomDifficulty = 0;
    bossRoom = false;
    bossKills = 0;
    monsterMaxLife = -1;

    aggroModifier = 0;
    defenceDecay = 0;
    redGemWeight = 100;
    blueGemWeight = 100;
    noGoldGemWeight = 35;
    noPowerGemWeight = 35;
    totalTime = 0f;

    if (defaultValues){
      maxLifeGainModifier = 1f;
      maxLife = life = 10;
      roomExtraPower = 0;
      roomExtraGold = 0;
      gold = 0;
      actionModifier = 0;
      attackDecay = 0;
      requiredPowerModifier = 0;
      rewardPriceModifier = 1f;
      monsterLifeModifier = 1f;
      energyShield = false;
      lifeGrowthFromPower = false;
      bloodEnergy = false;
      damageTakenDoubleTrigger = false;
      emptyPowerToPotential = false;
      emptyGoldToPotential = false;
      floorHeight = 8;
      disabledGameElements = new List<GameElement>();
      gemBoardHeight = 6;
      gemBoardWidth = 6;
      matchRequired = 3;
    }

    if (difficulty == 0){ // Easy
      aggroModifier -= 1;
      monsterLifeModifier *= 0.8f;

    }else if (difficulty == 2){ // Hard
      aggroModifier += 1;
      monsterLifeModifier *= 1.25f;
    }

    roomGold = Mathf.Max(0, 3 + roomExtraGold);
    roomPower = Mathf.Max(0, 3 + roomExtraPower);

    seed = (int)(DateTime.Now.Ticks* DateTime.Now.Ticks);
  }

  public void SaveVictory(){
    for(int i = 0; i <= difficulty; i++){
      string str = $"{heroIndex}_{i}";
      if (heroVictories.Contains(str) == false){
        heroVictories.Add(str);
      }
    }
  }

  public void SaveNewGame(){
    Save("newGame");
  }

  public void SaveGameOver(){
    Save("gameOver");
  }

  public void SaveMap(MapManager mapManager){
    Save("map");
  }

  public void SaveEvent(MapManager mapManager){
    Save("event");
  }

  public void SaveReward(GameManager gameManager){
    chestBought = gameManager.rewardManager.chestBought;
    rerollBought = gameManager.rewardManager.rerollBought;

    // Reward items
    List<RewardSave> rewardList = new List<RewardSave>();
    foreach(RewardButton rb in gameManager.rewardManager.rewardButtons)
    {
      RewardSave reward = new RewardSave();
      reward.name = rb.reward.name;
      reward.type = rb.reward.GetRewardType();
      rewardList.Add(reward);
    }
    rewardOptions = rewardList.ToArray();

    Save("reward");
  }

  public void SaveCombat(GameManager gameManager)
  {
    monsterName = gameManager.combatManager.monsterTemplate.name.Replace("(Clone)", "");
    roundNumber = gameManager.roundNumber;
    movesRemaining = gameManager.movesRemaining;
    gemBoard = gameManager.gemContainer.GetGems();

    remainingGold = gameManager.GetRemainingResource(GemColor.Yellow);
    remainingPower = gameManager.GetRemainingResource(GemColor.Gray);

    life = gameManager.combatManager.hero.life;
    playerAttack = gameManager.combatManager.hero.attack;
    playerDefence = gameManager.combatManager.hero.defence;
    playerEnergy = gameManager.combatManager.hero.energy;

    monsterAggressionTimer = gameManager.combatManager.monsterTemplate.aggressionTimer;
    monsterAggressionGrowth = gameManager.combatManager.monsterTemplate.aggressionGrowth;
    monsterMaxLife = gameManager.combatManager.monster.maxLife;
    monsterLife = gameManager.combatManager.monster.life;
    monsterAttack = gameManager.combatManager.monster.attack;
    monsterDefence = gameManager.combatManager.monster.defence;

    abilityTriggers = new List<AbilityTrigger>(gameManager.abilityManager.triggers.ToArray());

    Save("combat");
  }

  private void Save(string roomName){
    Debug.Log($"SAVE! ({roomName})");
    // Current run
    this.roomName = roomName;

    // Gems
    gems = DataManager.instance.gemBag.ToArray();

    // Items
    List<string> itemList = new List<string>();
    foreach (RewardItem item in DataManager.instance.items)
    {
      itemList.Add(item.name);
    }
    items = itemList.ToArray();

    // Abilities
    List<AbilitySave> abilityList = new List<AbilitySave>();
    foreach (Ability ability in DataManager.instance.abilities)
    {
      AbilitySave abilitySave = new AbilitySave();
      abilitySave.name = ability.name;
      abilitySave.exhausted = ability.exhausted;
      abilitySave.usesLeft = ability.usesLeft;
      abilityList.Add(abilitySave);
    }
    abilities = abilityList.ToArray();
    abilityTriggers = new List<AbilityTrigger>();

    // Write to file
    SaveManager.SaveGame(DataManager.instance.index, this);
  }

  public int GetRequiredPower()
  {
    return Mathf.Max(0, powerLevel + 4 + requiredPowerModifier);
  }

  public void Load(GameManager gameManager)
  {
    // Gems
    DataManager.instance.gemBag = new List<GemItem>(gems);

    // Items
    List<RewardItem> itemList = new List<RewardItem>();
    foreach (string itemName in items)
    {
      string path = $"ScriptableObjects/Rewards/Items/{itemName}";
      RewardItem item = Resources.Load<RewardItem>(path);
      itemList.Add(item);
    }
    DataManager.instance.items = itemList;

    // Abilities
    List<Ability> abilityList = new List<Ability>();
    foreach (AbilitySave abilitySave in abilities)
    {
      string path = $"ScriptableObjects/Abilities/{abilitySave.name}";
      Ability ability = Resources.Load<Ability>(path);
      ability.exhausted = abilitySave.exhausted;
      ability.usesLeft = abilitySave.usesLeft;
      abilityList.Add(ability);
    }
    DataManager.instance.abilities = abilityList;

    // Current combat
    if (roomName == "combat")
    {
      gameManager.roundNumber = roundNumber;
      gameManager.movesRemaining = movesRemaining;

      gameManager.combatManager.hero.AddAttack(playerAttack);
      gameManager.combatManager.hero.AddDefence(playerDefence);
      gameManager.combatManager.hero.AddEnergy(playerEnergy);
    }
    else if (roomName == "reward")
    {
      gameManager.rewardManager.rerollBought = rerollBought;
      if (rerollBought)
      {
        gameManager.rewardManager.DisableReroll();
      }
      gameManager.rewardManager.chestBought = chestBought;
      if (chestBought)
      {
        gameManager.rewardManager.rewards = 2;
        gameManager.rewardManager.DisableChest();
      }
      for (int i = 0; i < rewardOptions.Length; i++)
      {
        RewardButton rb = gameManager.rewardManager.rewardButtons[i];
        RewardSave rewardSave = rewardOptions[i];
        string path = $"ScriptableObjects/Rewards/{rewardSave.type}s/{rewardSave.name}";
        switch (rewardSave.type)
        {
          case RewardType.Gem:
            rb.SetItem(Resources.Load<RewardGem>(path));
            break;
          case RewardType.Ability:
            rb.SetItem(Resources.Load<RewardAbility>(path.Replace("Abilitys", "Abilities")));
            break;
          case RewardType.Item:
            rb.SetItem(Resources.Load<RewardItem>(path));
            break;
        }
      }
    }

    else if (roomName == "map")
    {
      eventName = "";
    }


  }

  public void UpdateSaveFormat(){
    // Fix old versions of save data
    if (unlockedHeroes.Length < 64){
      int[] tmp = new int[64];
      for(int i = 0; i < unlockedHeroes.Length; i++){
        tmp[i] = unlockedHeroes[i];
      }
      unlockedHeroes = tmp;
    }
  }

  public Monster GetMonster()
  {
    string path = $"ScriptableObjects/Monsters/{monsterName}";
    Monster monster = Resources.Load<Monster>(path);
    return monster;
  }

  public int GetPercentage(){
    int percentage = 0;
    for(int i = 1; i < unlockedHeroes.Length; i++){
      if (unlockedHeroes[i] > 0){
        percentage += 2;
        for(int d = 0; d < 3; d++){
          if (heroVictories.Contains($"{i}_{d}")){
            percentage += 1;
          }
        }
      }
    }
    return percentage;
  }

  public string TimeString(int time){
    string timeString = "";
    if (time > 3600 * 24 * 365)
    {
      timeString += (time / 3600 * 365) + "y ";
    }
    if (time > 3600 * 24 * 28)
    {
      timeString += ((time / 3600 * 24 * 28) % 28) + "m ";
    }
    if (time > 3600 * 24)
    {
      timeString += ((time / 60 * 60 * 24) % 28) + "d ";
    }
    if (time > 3600)
    {
      timeString += ((time / 3600) % 24) + "h ";
    }
    if (time > 60)
    {
      timeString += ((time / 60) % 60) + "m ";
    }
    timeString += (time % 60) + "s";
    return timeString;
  }

  public int GetRequiredProfileExperience()
  {
    return profileLevel * 100;
  }

  public HeroAsset GetHero(AssetList list, string heroName = null){
    foreach(UnityEngine.Object o in list.items){
      HeroAsset h = (HeroAsset)o;
      if (h.descriptionKey == (heroName == null ? heroIndex : heroName)){
        return h;
      }
    }
    return (HeroAsset)list.items[0];
  }
}

[System.Serializable]
public class GameSettings{
  public Language language = Language.english;
  public int lastIndex = -1;
  public float audioLevel = 1f;
  public float musicLevel = 1f;
  public float sfxLevel = 1f;
}

[System.Serializable]
public class AbilitySave
{
  public string name;
  public bool exhausted;
  public int usesLeft;
}

[System.Serializable]
public class RewardSave
{
  public string name;
  public RewardType type;
}