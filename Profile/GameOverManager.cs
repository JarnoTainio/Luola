using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
  public TextObject title;
  public TextMeshProUGUI scoreName;
  public TextMeshProUGUI scorePoints;
  public TextMeshProUGUI timeText;

  public TextMeshProUGUI levelText;
  public TextMeshProUGUI experienceText;
  public Image experienceBar;

  public GameObject levelUpContainer;
  public TextMeshProUGUI levelUpText;
  public GameObject rewardContainer;
  public Image rewardItemPrefab;

  public Button continueButton;
  public Button adButton;
  public FadeIn fader;

  private SaveData saveData;

  [Header("Audio")]
  public AudioSource sfxSource;
  public AudioClip levelUpSound;

  [Header("Setting")]
  public float timeBetweenScores = 1f;
  public float scoreDuration = 1f;
  public float experienceDuration = 3f;
  public float levelUpDelay = 0.1f;

  [Header("ScoreValue")]
  public int scoreFromRooms = 5;
  public int scoreFromPowerLevel = 5;
  public int scoreFromGold = 1;
  public int scoreFromOverkill = 1;
  public int[] scoreFromBoss = { 50, 100, 150, 200, 250 };

  public float speed;
  private float delaySpeed;

  [Header("LevelUpRewards")]
  public int maxLevel = 10;

  private void Awake()
  {
    fader.StartFadeIn();
    saveData = DataManager.instance.saveData;
    continueButton.interactable = false;
    adButton.gameObject.SetActive(false);
    scoreName.text = "";
    scorePoints.text = "";
    levelUpContainer.SetActive(false);
    levelText.text = saveData.profileLevel.ToString();
    title.SetText(DataManager.instance.victory ? "victory" : "gameOver");

    UpdateExperienceBar(saveData.profileExperience, saveData.GetRequiredProfileExperience());
    StartCoroutine(ScoreCounter());
    timeText.text = saveData.TimeString((int)DataManager.instance.saveData.totalTime);
  }

  public void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      speed = .25f;
      delaySpeed = .25f;
    }
  }

  public void UpdateExperienceBar(int experience, int requiredExperience)
  {
    experienceBar.transform.localScale = new Vector3((float)experience / requiredExperience, 1, 1);
    experienceText.text = $"{experience} / {requiredExperience}";
  }

  private IEnumerator ScoreCounter()
  {
    // Init
    speed = 4f;
    delaySpeed = 1f;
    int totalScore = 0;
    string text = "";
    float ticks = 120;
    int current;
    float s;
    int lastInt = 0;
    scoreName.text = "";
    scorePoints.text = "";
    float maxWait = .01f;

    // Levels
    if (saveData.room > 0)
    {
      scoreName.text += $"{Translation.GetTranslation("rooms", StringFormat.capitalized)} x{saveData.room}\n";
      current = saveData.room * scoreFromRooms;
      s = 0;
      for (int i = 0; i < ticks; i++)
      {
        s += current / ticks;
        int v = (int)s;
        scorePoints.text = v.ToString();
        if (v != lastInt)
          yield return new WaitForSeconds(Mathf.Min(maxWait, speed * Time.deltaTime));
        lastInt = v;
      }
      totalScore += current;
      text = current.ToString() + "\n";
      scorePoints.text = text;
      lastInt = 0;
      yield return new WaitForSeconds(timeBetweenScores * delaySpeed);
    }

    // Boss
    if (saveData.bossKills > 0)
    {
      int count = saveData.bossKills;
      scoreName.text += $"{Translation.GetTranslation("bossKills", StringFormat.capitalized)} x{count}\n";
      current = 0;
      for (int i = 0; i < count; i++)
      {
        current += i < scoreFromBoss.Length ? scoreFromBoss[i] : scoreFromBoss[scoreFromBoss.Length - 1];
      };

      s = 0;
      for (int i = 0; i < ticks; i++)
      {
        s += Mathf.Max(1, current / ticks);
        if (s > current)
        {
          break;
        }
        scorePoints.text = text + ((int)s).ToString();
        yield return new WaitForSeconds(Mathf.Min(maxWait, speed / ticks));
      }
      totalScore += current;
      text += current.ToString() + "\n";
      scorePoints.text = text;
      lastInt = 0;
      yield return new WaitForSeconds(timeBetweenScores * delaySpeed);
    }


    // PowerLevel
    if (saveData.powerLevel > 0)
    {
      scoreName.text += $"{Translation.GetTranslation("powerLevel", StringFormat.capitalized)} x{saveData.powerLevel}\n";
      current = saveData.powerLevel * scoreFromPowerLevel;
      s = 0;
      for (int i = 0; i < ticks; i++)
      {
        s += Mathf.Max(1, current / ticks);
        if (s > current)
        {
          break;
        }
        int v = (int)s;
        scorePoints.text = text + v.ToString();
        if (v != lastInt)
          yield return new WaitForSeconds(Mathf.Min(maxWait, speed / ticks));
        lastInt = v;
      }
      totalScore += current;
      text += current.ToString() + "\n";
      scorePoints.text = text;
      lastInt = 0;
      yield return new WaitForSeconds(timeBetweenScores * delaySpeed);
    }

    // Gold
    if (saveData.gold > 0)
    {
      scoreName.text += $"{Translation.GetTranslation("gold", StringFormat.capitalized)} x{saveData.gold}\n";
      current = saveData.gold * scoreFromGold;
      s = 0;
      for (int i = 0; i < ticks; i++)
      {
        s += Mathf.Max(1, current / ticks);
        if (s > current)
        {
          break;
        }
        int v = (int)s;
        scorePoints.text = text + v.ToString();
        if (v != lastInt)
          yield return new WaitForSeconds(Mathf.Min(maxWait, speed / ticks));
        lastInt = v;
      }
      totalScore += current;
      text += current.ToString() + "\n";
      scorePoints.text = text;
      lastInt = 0;
      yield return new WaitForSeconds(timeBetweenScores * delaySpeed);
    }

    // Overkill
    if (saveData.overkill > 0)
    {
      scoreName.text += $"{Translation.GetTranslation("overkill", StringFormat.capitalized)} x{saveData.overkill}\n";
      current = saveData.overkill * scoreFromOverkill;
      s = 0;
      for (int i = 0; i < ticks; i++)
      {
        s += Mathf.Max(1, current / ticks);
        if (s > current)
        {
          break;
        }
        int v = (int)s;
        scorePoints.text = text + v.ToString();
        if (v != lastInt)
          yield return new WaitForSeconds(Mathf.Min(maxWait, speed / ticks));
        lastInt = v;
      }
      totalScore += current;
      text += current.ToString() + "\n";
      scorePoints.text = text;
      yield return new WaitForSeconds(timeBetweenScores * delaySpeed);
    }

    // Total
    scoreName.text += $"<b>{Translation.GetTranslation("total", StringFormat.capitalized)}:</b>";

    current = totalScore;
    s = 0;
    for (int i = 0; i < ticks; i++)
    {
      s += Mathf.Max(1, current / ticks);
      if (s > current)
      {
        break;
      }
      int v = (int)s;
      scorePoints.text = text + v.ToString();
      if (v != lastInt)
        yield return new WaitForSeconds(Mathf.Min(maxWait, speed / ticks));
      lastInt = v;
    }
    scorePoints.text = text + current.ToString();
    yield return new WaitForSeconds(timeBetweenScores * delaySpeed);


    // Experience
    int requiredExperience = saveData.GetRequiredProfileExperience();
    s = saveData.profileExperience;

    float total = 0;
    bool running = true;
    float f = (float)requiredExperience / 120f;
    while (running){
      s += f;
      total += f;
      int v = (int)s;

      if (total >= totalScore){
        s = v = (int)(s + (totalScore - total));
      }

      if (s >= requiredExperience)
      {
        s -= requiredExperience;
        v = (int)s;
        StartCoroutine(LevelUp());
        UpdateExperienceBar(requiredExperience, requiredExperience);
        yield return new WaitForSeconds(levelUpDelay);

        if (saveData.profileLevel == maxLevel)
        {
          break;
        }
        requiredExperience = saveData.GetRequiredProfileExperience();
        f = (float)requiredExperience / 120f;
      }

      UpdateExperienceBar(v, requiredExperience);
      if (total >= totalScore){
        break;
      }
      if (v != lastInt){
        Debug.Log(2 * requiredExperience * speed * Time.deltaTime);
        yield return new WaitForSeconds(Mathf.Min(0.1f, 2 * speed * Time.deltaTime));
        lastInt = v;
      }
    }

    saveData.profileExperience = (int)s;
    saveData.profileTime += (int)saveData.totalTime;
    saveData.totalRuns++;

    if (DataManager.instance.victory ){
      saveData.victoryCount++;
      saveData.SaveVictory();
    }
    saveData.SaveNewGame();

    adButton.gameObject.SetActive(true);
    continueButton.interactable = true;
  }

  public IEnumerator LevelUp()
  {
    if (saveData.profileLevel < maxLevel)
    {
      sfxSource.clip = levelUpSound;
      sfxSource.Play();

      saveData.profileLevel++;
      saveData.profileExperience = 0;

      // Get all rewards and find the ones that are opened at the current level
      List<Reward> rewardItems = new List<Reward>();
      List<Object> loaded_rewards = new List<Object>();
      loaded_rewards.AddRange(Resources.LoadAll("ScriptableObjects/Rewards/Gems", typeof(RewardGem)));
      loaded_rewards.AddRange(Resources.LoadAll("ScriptableObjects/Rewards/Abilities", typeof(RewardAbility)));
      loaded_rewards.AddRange(Resources.LoadAll("ScriptableObjects/Rewards/Items", typeof(RewardItem)));
      foreach (Object obj in loaded_rewards)
      {
        Reward r = (Reward)obj;
        if (r.level == saveData.profileLevel)
        {
          rewardItems.Add((Reward)obj);
          Debug.Log(r.level +" " + obj.name);
        }
      }
      Debug.Log("=======================");

      var children = new List<GameObject>();
      foreach (Transform child in rewardContainer.transform) children.Add(child.gameObject);
      children.ForEach(child => Destroy(child));


      levelUpContainer.SetActive(true);
      levelUpText.text = $"{Translation.GetTranslation("level", StringFormat.capitalized)} " + saveData.profileLevel.ToString() + "!";
      levelText.text = saveData.profileLevel.ToString();
      int index = saveData.profileLevel - 2;
      for (int i = 0; i < rewardItems.Count; i++)
      {
        yield return new WaitForSeconds(0f);
        Image rewardObject = Instantiate(rewardItemPrefab, rewardContainer.transform);
        rewardObject.sprite = rewardItems[i].GetSprite();
        rewardObject.GetComponent<AnimatedObject>().delay = i * .2f;
      }
    }
  }

  public void ContinueButtonClicked()
  {
    fader.StartFadeOut(DataManager.instance.ToCharacterSelection);
  }

  public int GetStep(float tick)
  {
    return (int)Mathf.Max(1, 1 / tick);
  }
}

[System.Serializable]
public class LevelRewards
{
  public Sprite[] rewards;
}
