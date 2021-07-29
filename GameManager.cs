using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  [Header("References")]
  public GemContainer gemContainer;
  public CombatManager combatManager;
  public AbilityManager abilityManager;
  public PowerManager powerManager;
  public UIManager uiManager;
  public GameObject goldGO;
  public RewardManager rewardManager;
  public GameObject zeroObject;
  public TriggerQueue triggerQueue;
  public Announcement announcement;
  public FadeIn fadeIn;

  [Header("Tokens")]
  public TokenContainer coinContainer;
  public TokenContainer powerContainer;

  [Header("Game")]
  public int roundNumber;

  [Header("Attributes")]
  public bool gameOver;
  public bool playerControl;
  public int movesPerTurn;
  public int movesRemaining;

  [Header("Audio")]
  public AudioSource musicSource;
  public AudioClip victoryStinger;
  public AudioClip defeatedStinger;

  public void Start(){
    StartCoroutine(Init());
  }

  public void InitCombat(bool useless = true){
    StartCoroutine(Init());
  }

  public IEnumerator Init()
  {
    bool loaded = false;
    bool startCombat = true;
    gameOver = false;
    SaveData saveData = DataManager.instance.saveData;
    UnityEngine.Random.InitState((int)(saveData.seed * (saveData.room + 1)));
    if (DataManager.instance.loading)
    {
      loaded = true;
      coinContainer.SetTokens(saveData.remainingGold);
      powerContainer.SetTokens(saveData.remainingPower);
      if (DataManager.instance.saveData.roomName == "reward")
      {
        gameOver = true;
        uiManager.rewardScreen.gameObject.SetActive(true);
        startCombat = false;
      }
      saveData.Load(this);
    }
    else
    {
      coinContainer.SetTokens(saveData.roomGold);
      powerContainer.SetTokens(saveData.roomPower);
      roundNumber = 0;
    }
    ModifyResource(Resource.Gold, 0);
    abilityManager.Init(loaded);
    uiManager.Init();

    if (startCombat)
    {
      combatManager.Init(loaded);
      powerManager.Init(loaded);
      NewRound(loaded == false);

      if (loaded)
      {
        gemContainer.Init(true);
      }
      yield return new WaitForSeconds(1.5f);
      if (loaded == false )
      {
        gemContainer.Init(false);
      }
      DataManager.instance.loading = false;
    }
    abilityManager.UpdateButtons();
  }

  public void NewRound(bool nextRound = true)
  {
    if (nextRound){
      if (roundNumber == 0)
      {
        Trigger(global::Trigger.RoomStart);
      }else {
        Trigger(global::Trigger.RoundEnd);
      }
      Trigger(global::Trigger.RoundStart);
    }

    // Update monster agression
    int aggression = combatManager.monsterTemplate.GetAggresion(roundNumber);
    if (aggression > 0 && combatManager.monster.energy != aggression){
      combatManager.monster.energy = aggression;
      combatManager.monster.AddEnergy(0);
    }

    if (nextRound)
    {
      roundNumber++;
      movesRemaining += movesPerTurn + DataManager.instance.saveData.actionModifier;
      DataManager.instance.ResetAbilities(GamePhase.RoundStart);
      uiManager.escapeButton.UpdateState(this);
    }

    if (nextRound)
    {
      StartCoroutine(SaveCombat());
    }

    uiManager.SetMovesRemaining(movesRemaining);
    //combatManager.NewRound();
    gemContainer.SetGemsUsable(true);
    SetPlayerControl(true);
  }

  public void NextTurn(bool gemsDestroyed)
  {
    Trigger(global::Trigger.TurnStart);
    if (gemsDestroyed == true || abilityManager.UseTrigger(AbilityTrigger.FreeMove) == false)
    {
      movesRemaining--;
      uiManager.SetMovesRemaining(movesRemaining);
    }

    DataManager.instance.ResetAbilities(GamePhase.TurnStart);
    if (movesRemaining == 0)
    {
      gemContainer.SetGemsUsable(false);
      SetPlayerControl(false);
      abilityManager.DisableButtons();
      StartCoroutine(EndRound());
    }
    else
    {
      StartCoroutine(SaveCombat());
      SetPlayerControl(true);
    }
  }

  public void GameOver(bool victory)
  {
    SetPlayerControl(false);
    gameOver = true;
    if (victory)
    {
      uiManager.SetMessage("victory", StringFormat.upcase);
      musicSource.PlayOneShot(victoryStinger);
      if (DataManager.instance.IsLastBoss())
      {
        TestUnlocking(UnlockKey.Victory, 1);
        StartCoroutine(VictoryDance());
      }
      else
      {
        StartCoroutine(ShowReward());
      }
    }
    else
    {
      triggerQueue.Stop();
      uiManager.SetMessage("gameOver", StringFormat.upcase);
      musicSource.PlayOneShot(defeatedStinger);
      uiManager.replayButton.gameObject.SetActive(true);
      DataManager.instance.saveData.SaveGameOver();
    }
  }

  public void ShuffleBoard()
  {
    StartCoroutine(gemContainer.ShuffleBoard());
  }

  public bool PlayerControl(bool waitForSpheres = true)
  {
    return playerControl && gemContainer.playerControl 
    && (waitForSpheres == false || (triggerQueue.IsReady() && combatManager.spheres.Count == 0));
  }

  /*
   * IENUMERATORS
   */
  IEnumerator ShowReward()
  {
    yield return new WaitForSeconds(2f);
    while(triggerQueue.IsReady() == false){
      yield return null;
    }
    uiManager.rewardScreen.gameObject.SetActive(true);
    DataManager.instance.saveData.SaveReward(this);
  }

  IEnumerator VictoryDance()
  {
    yield return new WaitForSeconds(3f);
    while(triggerQueue.IsReady() == false){
      yield return null;
    }

    // Wait for announcements
    if (announcement.IsActive()){
      while (announcement.IsActive()){
        yield return null;
      }
      yield return new WaitForSeconds(.5f);
    }
    DataManager.instance.Victory();
  }

  public IEnumerator EndRound()
  {
    while (!combatManager.IsReady())
    {
      yield return null;
    }
    uiManager.SetMessage("fight", StringFormat.upcase);
    Trigger(global::Trigger.CombatStart);
    yield return new WaitForSeconds(1f);
    combatManager.StartCombat();
  }

  /*
   * GETTERS AND SETTERS
   */

  public int GetResource(Resource resource, bool pureResource = false)
  {
    switch(resource){
      case Resource.Attack:
      {
        return combatManager.hero.attack;
      }
      case Resource.Defence:
      {
        return combatManager.hero.defence;
      }
      case Resource.Energy:
      {
        if (pureResource){
          return combatManager.hero.energy;
        }
        return DataManager.instance.saveData.bloodEnergy ? (combatManager.hero.energy + combatManager.hero.life - 1) : combatManager.hero.energy;
      }
      case Resource.Life:
      {
        return combatManager.hero.life;
      }
      case Resource.Gold:
      {
        return DataManager.instance.GetResource(resource);
      }
      case Resource.FullLife:
      {
        return DataManager.instance.GetResource(resource);
      }
      case Resource.RoomGold:
      {
        return coinContainer.GetCount();
      }
      case Resource.RoomPower:
      {
        return powerContainer.GetCount();
      }
      case Resource.Potential:
      {
        return combatManager.hero.potential;
      }
      default: return 0;
    }
  }

  public void ModifyResource(Resource resource, int amount, bool useTokens, Vector3 startingPosition){
    GemColor gc = Gem.ResourceToColor(resource);
    if (amount > 0 && gc != GemColor.None){
      StartCoroutine(SendSpheres(gc, amount, useTokens, startingPosition));
    } else{
      ModifyResource(resource, amount, useTokens);
    }
  }

  public IEnumerator SendSpheres(GemColor gemColor, int amount, bool useTokens, Vector3 startingPosition){
    for(int i = 0; i < amount; i++){
      SendSphereFromZero(gemColor, 0f, Camera.main.ScreenToWorldPoint(startingPosition), useTokens);
      if (i < amount -1){
        yield return new WaitForSeconds(0.1f);
      }
    }
  }

  public void ModifyResource(Resource resource, int amount, bool useTokens = true)
  {
    switch (resource)
    {
      case Resource.Attack:
        {
          combatManager.hero.AddAttack(amount);
          TestUnlocking(UnlockKey.Attack, combatManager.hero.attack);
          break;
        }
      case Resource.Defence:
        {
          combatManager.hero.AddDefence(amount);
          break;
        }
      case Resource.Energy:
        {
          combatManager.hero.AddEnergy(amount);
          uiManager.escapeButton.EnergyModified(combatManager.hero.energy);
          break;
        }
      case Resource.Life:
        {
          combatManager.hero.AddLife(amount);
          DataManager.instance.saveData.life = combatManager.hero.life;
          break;
        }
      case Resource.Gold:
        {
            if (amount <= 0 || useTokens == false || coinContainer.RemoveToken()){

            int gold = Mathf.Max(0, DataManager.instance.saveData.gold + amount);
            DataManager.instance.saveData.gold = Mathf.Max(0 , gold);
            TestUnlocking(UnlockKey.Gold, gold);
            goldGO.SetActive(gold > 0);
            if (gold > 0){
              uiManager.goldText.Set(gold.ToString(), true);
            }
          }
          break;
        }
      case Resource.Power:
        {
          for (int i = 0; i < amount; i++)
          {
            powerManager.ActivateBar(useTokens);
          }
          TestUnlocking(UnlockKey.PowerLevel, DataManager.instance.saveData.powerLevel);
          break;
        }
      case Resource.Action:
        {
          ModifyTurn(amount);
          break;
        }
      case Resource.MaxLife:
        {
          DataManager.instance.saveData.maxLife += amount;

          combatManager.hero.maxLife = DataManager.instance.saveData.maxLife;
          if (amount > 0){
            if (DataManager.instance.saveData.maxLife + amount >= 999){
              amount = DataManager.instance.saveData.maxLife - 999;
              DataManager.instance.saveData.maxLife = 999;
            }
              combatManager.hero.AddLife(amount);
          }
          DataManager.instance.saveData.life = combatManager.hero.life;

          TestUnlocking(UnlockKey.MaxLife, DataManager.instance.saveData.maxLife);
          break;
        }
      case Resource.Potential:
        {
          if (gameOver){
            DataManager.instance.saveData.purpleGemWeight += amount;
          }else{
            combatManager.hero.AddPotential(amount);
          }
          break;
        }
      case Resource.RoomGold:
        {
          if (gameOver) {
            DataManager.instance.saveData.roomExtraGold += amount;
          }
          else
          {
            if (amount < 0)
            {
              for (int i = 0; i < -amount; i++)
              {
                coinContainer.RemoveToken();
              }
            }
            else if (amount > 0){
              coinContainer.AddTokens(amount);
            }
          }
          break;
        }
      case Resource.RoomPower:
        {
          if (gameOver)
          {
            DataManager.instance.saveData.roomExtraPower += amount;
          }
          else
          {
            if (amount < 0)
            {
              for (int i = 0; i < -amount; i++)
              {
                powerContainer.RemoveToken();
              }
            }
          }
          break;
        }
      case Resource.Aggro:
        {
          DataManager.instance.saveData.aggroModifier += amount;
          break;
        }
      case Resource.ShopPrice:
        {
          DataManager.instance.saveData.rewardPriceModifier += amount / 100f;
          if (DataManager.instance.saveData.rewardPriceModifier <= .25f)
          {
            DataManager.instance.saveData.rewardPriceModifier = .25f;
          }
          break;
        }
      case Resource.DefenceDecay:
        {
          DataManager.instance.saveData.defenceDecay = amount;
          break;
        }
      case Resource.AttackDecay:
        {
          DataManager.instance.saveData.attackDecay = amount;
          break;
        }
      case Resource.noGoldGemWeight:
        {
          DataManager.instance.saveData.noGoldGemWeight = amount;
          break;
        }
      case Resource.noPowerGemWeight:
        {
          DataManager.instance.saveData.noPowerGemWeight = amount;
          break;
        }
      case Resource.redGemWeight:
        {
          DataManager.instance.saveData.redGemWeight += amount;
          break;
        }
      case Resource.blueGemWeight:
        {
          DataManager.instance.saveData.blueGemWeight += amount;
          break;
        }
      case Resource.GemBoardWidth:
        {
          DataManager.instance.saveData.gemBoardWidth += amount;
          break;
        }
      case Resource.GemBoardHeight:
        {
          DataManager.instance.saveData.gemBoardHeight += amount;
          break;
        }
      case Resource.EnergyShield:
        {
          DataManager.instance.saveData.energyShield = amount == 1;
          break;
        }
      case Resource.BloodEnergy:
        {
          DataManager.instance.saveData.bloodEnergy = amount == 1;
          break;
        }
      case Resource.DoubleDamageTakenTrigger:
        {
          DataManager.instance.saveData.damageTakenDoubleTrigger = amount == 1;
          break;
        }
      case Resource.LifeGrowth:
        {
          DataManager.instance.saveData.lifeGrowthFromPower = amount == 1;
          break;
        }
      case Resource.PotentialFromGray:
        {
          DataManager.instance.saveData.emptyPowerToPotential = amount == 1;
          break;
        }
      case Resource.PotentialFromYellow:
        {
          DataManager.instance.saveData.emptyGoldToPotential = amount == 1;
          break;
        }
    }
    if (amount != 0){
      abilityManager.UpdateButtons();
    }
  }

  public void ModifyTurn(int amount)
  {
    movesRemaining = Mathf.Max(0, movesRemaining + amount);
    uiManager.SetMovesRemaining(movesRemaining);
    combatManager.CreateFloatingText((amount > 0 ? "+" : "") + amount.ToString() + " " + Translation.GetTranslation("action"), Color.white);
    SetPlayerControl(true);
  }

  public void Trigger(Trigger trigger, object data = null)
  {
    DataManager.instance.Trigger(this, trigger, data);
  }

  public void GameOverButtonClicked()
  {
    DataManager.instance.GameOver();
  }
  public void SendSphereFromZero(GemColor gemColor, float delay, Vector3 origin, bool useToken)
  {
    Color color = gemContainer.gemPrefab.GetSpriteColor(gemColor);
    Vector3 position = origin != Vector3.zero ? new Vector3(origin.x, origin.y, 0) : zeroObject.transform.position;
    Vector3 target = combatManager.GetSphereTarget(gemColor);
    combatManager.CreateSphere(position, target, color, gemColor, delay, useToken);
  }

  public void AddTrigger(RewardItemAttribute item)
  {
    triggerQueue.AddTrigger(item);
  }

  public void SetPlayerControl(bool control)
  {
    if (control)
    {
      playerControl = true;
    }
    else
    {
      playerControl = false;
    }
    abilityManager.UpdateButtons();
  }

  public int GetRemainingResource(GemColor gemColor)
  {
    if (gemColor == GemColor.Yellow)
    {
      return coinContainer.GetCount() - combatManager.spheres.FindAll(s => s.gem == gemColor).Count;
    }
    else if (gemColor == GemColor.Gray)
    {
      return powerContainer.GetCount() - combatManager.spheres.FindAll(s => s.gem == gemColor).Count;
    }
    return 999;
  }

  public void TestUnlocking(UnlockKey target, int value){
    List<int> unlocks = DataManager.instance.TestHeroUnlocking(target, value);
    foreach(int i in unlocks){
      announcement.ShowHeroUnlock(i);
    }
  }

  bool saving = false;

  public IEnumerator SaveCombat(){
    if (saving == false){
      saving = true;
      while(PlayerControl() == false){
        yield return null;
      }
      abilityManager.UpdateButtons();
      DataManager.instance.saveData.SaveCombat(this);
      saving = false;
    }
  }

  public void Escape(){
    if (PlayerControl(false) && uiManager.escapeButton.canEscape){
      uiManager.escapeButton.gameObject.SetActive(false);
      SetPlayerControl(false);
      Unit hero = combatManager.hero;
      hero.walkSpeed *= 1.5f;
      hero.StartCoroutine(hero.WalkTo(new Vector3(-15, hero.transform.localPosition.y, 0f)));
      ModifyResource(Resource.Energy, -combatManager.monsterTemplate.escapeCost);
      StartCoroutine(WaitForEscape());
    }
  }

  public IEnumerator WaitForEscape(){
    yield return new WaitForSeconds(2f);
    DataManager.instance.ReturnToMap();
  }
}

public enum Resource {
  Energy, Gold, Attack, Defence, Life, MaxLife, Power,
  FullLife, RoomGold, RoomPower, Action, Aggro,
  ShopPrice, DefenceDecay, AttackDecay, noGoldGemWeight, noPowerGemWeight, redGemWeight, blueGemWeight,
  EnergyShield, LifeGrowth, BloodEnergy, GemBoardWidth, GemBoardHeight, Potential, DoubleDamageTakenTrigger, 
  PotentialFromGray, PotentialFromYellow
  }
public enum GamePhase { TurnStart, RoundStart, RoundEnd, CombatStart, CombatEnd, EncounterStart, EncounterEnd  }