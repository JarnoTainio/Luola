using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
  public static DataManager instance;

  [Header("SaveData")]

  public bool loading;
  public int index;
  public int difficulty;
  public SaveData saveData;
  public GameSettings gameSettings;

  public int gemChance = 15;
  public bool victory = false;
  public bool testMode = true;

  public Ability[] startingAbilities;
  public GemItem[] startingGems;
  public RewardItem[] startingItems;

  public List<Ability> abilities = new List<Ability>();
  public List<GemItem> gemBag = new List<GemItem>();
  public List<RewardItem> items = new List<RewardItem>();

  public Scene currentScene = Scene.MenuScene;
    // Start is called before the first frame update
  void Awake()
  {
    if (instance != null)
    {
      Destroy(gameObject);
    }
    else
    {
      Screen.sleepTimeout = SleepTimeout.NeverSleep;
      DontDestroyOnLoad(gameObject);
      instance = this;
      abilities = new List<Ability>(startingAbilities);
      gemBag = new List<GemItem>(startingGems);
      items = new List<RewardItem>(startingItems);

      gameSettings = SaveManager.LoadSettings();

      foreach (TextObject to in Resources.FindObjectsOfTypeAll(typeof(TextObject)))
      {
        to.Reload();
      }
      gemChance = 15;
    }
  }

  public void Update()
  {
    if (currentScene == Scene.CombatScene || currentScene == Scene.MapScene)
    {
      saveData.totalTime += Time.deltaTime;
    }
  }

  public void StartNewRun(bool useless = true){
    loading = false;
    abilities = new List<Ability>(startingAbilities);
    gemBag = new List<GemItem>(startingGems);
    items = new List<RewardItem>(startingItems);
    saveData.NewRun(false, difficulty);
    foreach(Ability a in abilities){ saveData.seenRewards.Add(a.name); }
    foreach(RewardItem i in items){ saveData.seenRewards.Add(i.name); }
    StartCombat();
  }

  // Load game
  public void LoadGame()
  {
    loading = true;
    if (saveData.unlockedHeroes[0] == 0)
    {
      saveData.unlockedHeroes[0] = 1;
    }

    if (saveData.roomName == "gameOver")
    {
      GameOver();
    }
    else if (saveData.roomName == "newGame")
    {
      SceneManager.LoadScene("NewGameScene");
    }
    else if (saveData.roomName == "map" || saveData.roomName == "event")
    {
      currentScene = Scene.MapScene;
      SceneManager.LoadScene("MapScene");
    }
    else
    {
      currentScene = Scene.CombatScene;
      SceneManager.LoadScene("CombatScene");
    }
  }

  // Start new combat from newGameScene or from mapScene
  public void StartCombat(bool useless = true)
  {
    ResetGems();
    ResetAbilities(GamePhase.EncounterStart);
    currentScene = Scene.CombatScene;
    SceneManager.LoadScene("CombatScene");
  }

  // Return to main menu from menuDialog or by pressing back in newGameScene
  public void LoadMenuScene(bool useless = true)
  {
    saveData = null;
    currentScene = Scene.MenuScene;
    SceneManager.LoadScene("MenuScene");
  }

  // Move to character selection from new profile or from gameOverScene
  public void ToCharacterSelection(bool useless = false){
    currentScene = Scene.NewGameScene;
    SceneManager.LoadScene("NewGameScene");
  }

  // Return to map from combat scene
  public void ReturnToMap()
  {
    if (saveData.bossRoom)
    {
      saveData.bossKills++;
      saveData.floor++;
      saveData.roomX = -1;
      saveData.roomY = -1;
      saveData.rooms.Add(new RoomVector(0, 0, saveData.floor));
      saveData.bossRoom = false;
    }
    currentScene = Scene.MapScene;

    SaveManager.SaveGame(index, saveData);
    SceneManager.LoadScene("MapScene");
  }

  // Defeated in combat or abandoned run
  public void GameOver(bool useless = true)
  {
    currentScene = Scene.GameOverScene;
    SceneManager.LoadScene("GameOverScene");
  }

  // Can be reached only from combat scene
  public void Victory()
  {
    victory = true;
    saveData.bossKills++;
    GameOver();
  }

  private void ResetGems()
  {
   foreach(GemItem gem in gemBag)
    {
      gem.isReady = true;
    }
  }

  public void ResetAbilities(GamePhase gamePhase)
  {
    foreach(Ability ability in abilities)
    {
      ability.Refresh(gamePhase);
    }
  }

  public GemItem GetGem(GemColor color, bool quality)
  {
    if (quality)
    {
      List<GemItem> legalGems = gemBag.FindAll(g => {
        return g.isReady && color == g.color;
      });

      if (legalGems.Count > 0)
        return legalGems[Random.Range(0, legalGems.Count)];
    }
    return new GemItem(color, GemType.Normal);
  }

  public Monster GetMonster(bool loading) {
    Monster monster = null;
    if (loading){
      monster = saveData.GetMonster();
      if (monster != null){
        monster = Instantiate(monster);
        monster.aggressionGrowth = saveData.monsterAggressionGrowth;
        monster.aggressionTimer = saveData.monsterAggressionTimer;
        return monster;
      }
    }

    // Get new monster
    string lastMonster = saveData.monsterName;
    List<Monster> monsters = new List<Monster>();
    foreach(Object obj in Resources.LoadAll("ScriptableObjects/Monsters/", typeof(Monster)))
    {
      Monster m = (Monster)obj;
      if (lastMonster != obj.name)
      {
        monsters.Add((Monster)obj);
      }
    }
    if (saveData.bossRoom){
      saveData.roomDifficulty = saveData.floor + 1;
    }

    monsters = monsters.FindAll(m => {
      if (saveData.bossRoom)
      {
        return m.type == MonsterType.boss;
      }
      return m.type == MonsterType.minion;
    }).FindAll(m => m.difficulty == saveData.roomDifficulty);
    Monster template = monsters[Random.Range(0, monsters.Count)];
    monster = Instantiate(template);

    // Boss enrages faster on larger dungeons
    if (saveData.bossRoom){
      monster.aggressionTimer -= (saveData.floorHeight - 8) / 2;
    }
    monster.name = template.name;

    if (saveData.roomDanger > 0)
    {
      monster.maxLife = (int)(monster.maxLife * 1.4f);
    }
    float f = DataManager.instance.saveData.monsterLifeModifier;
    if (f != 1.0f){
      monster.maxLife = (int)((float)monster.maxLife * f);
      if (monster.maxLife > 50){
        monster.maxLife = (int)(Mathf.Round(monster.maxLife / 10f) * 10);
      }else if (monster.maxLife > 30){
        monster.maxLife = (int)(Mathf.Round(monster.maxLife / 5f) * 5);
      }
    }
    return monster;
  }

  public int GetResource(Resource resource)
  {
    switch (resource)
    {
      case Resource.Gold:
        return saveData.gold;
      case Resource.Life:
        return saveData.life;
      case Resource.MaxLife:
        return saveData.maxLife;
      case Resource.Power:
        return saveData.power;
      case Resource.FullLife:
        return saveData.life == saveData.maxLife ? 1 : 0;
    }
    return 0;
  }

  public void AddGem(GemItem gem)
  {
    gemBag.Add(gem.Copy());
  }

  public int GetQuality()
  {
    int count = 0;
    foreach (GemItem g in gemBag)
    {
      if (g.isReady)
      {
        count += 1;
      }
    }
    return Mathf.Min(gemChance + count, 50);
  }

  public void Trigger(GameManager gameManager, Trigger trigger, object data = null)
  {
    foreach(RewardItem item in items)
    {
      item.Trigger(gameManager, trigger, data);
    }
  }

  public List<int> TestHeroUnlocking(UnlockKey context, int value)
  {
    List<int> unlocked = new List<int>();
    switch (context){
      case UnlockKey.Victory:
        if (value == 1)
        {
          if (saveData.unlockedHeroes[7] == 0 && saveData.damageTaken == 0){
            saveData.unlockedHeroes[7] = 2;
            unlocked.Add(7);
          }
          if (saveData.unlockedHeroes[1] == 0){
            saveData.unlockedHeroes[1] = 2;
            unlocked.Add(1);
          }
        }
        break;
      case UnlockKey.Gold:
        if (value >= 100)
        {
          if (saveData.unlockedHeroes[2] == 0){
            saveData.unlockedHeroes[2] = 2;
            unlocked.Add(2);
          }
        }
        break;
      case UnlockKey.PowerLevel:
        if (value >= 15)
        {
          if (saveData.unlockedHeroes[3] == 0){
            saveData.unlockedHeroes[3] = 2;
            unlocked.Add(3);
          }
        }
        break;
      case UnlockKey.MaxLife:
        if (value >= 40)
        {
          if (saveData.unlockedHeroes[4] == 0){
            saveData.unlockedHeroes[4] = 2;
            unlocked.Add(4);
          }
        }
        break;
      case UnlockKey.Attack:
        if (value >= 20)
        {
          if (saveData.unlockedHeroes[5] == 0){
            saveData.unlockedHeroes[5] = 2;
            unlocked.Add(5);
          }
        }
        break;
      case UnlockKey.Overkill:
        if (value > 1000)
        {
          if (saveData.unlockedHeroes[6] == 0){
            saveData.unlockedHeroes[6] = 2;
            unlocked.Add(6);
          }
        }
        break;
      case UnlockKey.AddsWatched:
        for(int i = 1; i <= 8; i++){
          if (saveData.unlockedHeroes[7 + i] == 0 && value >= requiredAdds[i - 1])
          {
            saveData.unlockedHeroes[7 + i] = 2;
            unlocked.Add(7 + i);
          }
        }
        break;
    }
    return unlocked;
  }

  int[] requiredAdds = {1, 5, 10, 15, 20, 25, 30, 40, 50, 60, 75, 100};

  public bool IsLastBoss()
  {
    return saveData.bossRoom && saveData.floor == 2;
  }

  public int GetAmount(ResourceAmount ra){

    switch (ra.resource){
      case Resource.MaxLife:
        int value = ra.amount < 0 ? 
        RoundValue(ra.amount, 1f - (saveData.maxLifeGainModifier / 2))
        : RoundValue(ra.amount, saveData.maxLifeGainModifier);
        return value;
    }
    return ra.amount;
  }

  private int RoundValue(int value, float modifier){
    if (value == 0){ return 0; }
    int v = (int)(value * saveData.maxLifeGainModifier);
    if (v == 0){
      v = value < 0 ? -1 : 1;
    }
    return v;
  }

  public void SetAudioLevel(float f) { gameSettings.audioLevel = f; }
  public void SetMusicLevel(float f) { gameSettings.musicLevel = f; }
  public void SetSFXLevel(float f) { gameSettings.sfxLevel = f; }

  public float GetMusicLevel() { return gameSettings.audioLevel * gameSettings.musicLevel; }
  public float GetSFXLevel() { return gameSettings.audioLevel * gameSettings.sfxLevel; }

}
public enum Scene { MenuScene, CombatScene, MapScene, GameOverScene, NewGameScene}
public enum UnlockKey { Victory, Gold, PowerLevel, Attack, MaxLife, Overkill, AddsWatched}
public enum GameElement { GoldTokens, PowerTokens, DamageTaken, MaxLifeGain, MaxLifeLoss }
