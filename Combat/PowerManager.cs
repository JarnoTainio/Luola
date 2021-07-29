using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerManager : MonoBehaviour
{
  public GameManager gameManager;
  public PowerBar powerBarPrefab;
  public Sprite powerSprite;
  List<PowerBar> powerBars;
  public int requiredPower;
  public int incomingPower;

  public GameObject powerLevelOrb;
  public TextMeshProUGUI powerLevelText;

  public void Init(bool loaded)
  {
    incomingPower = 0;
    powerBars = new List<PowerBar>();
    requiredPower = GetRequiredPower();

    for (int i = 0; i < requiredPower; i++)
    {
      PowerBar bar = Instantiate(powerBarPrefab, transform);
      powerBars.Add(bar);
    }
    UpdateBars();
    int powerLevel = DataManager.instance.saveData.powerLevel;
    if (!loaded)
    {
      for (int i = 1; i <= powerLevel / (DataManager.instance.saveData.lifeGrowthFromPower ? 2 : 1); i++)
      {
        gameManager.SendSphereFromZero(GemColor.Green,  (3f / powerLevel) * i,
        Camera.main.ScreenToWorldPoint(powerLevelOrb.transform.position) + new Vector3(0.5f, -0.5f)
        , false);
      }
    }
    powerLevelText.text = powerLevel.ToString();
    powerLevelOrb.SetActive(powerLevel > 1);
  }

  public Vector3 GetCurrentPosition()
  {
    if (DataManager.instance.saveData.emptyPowerToPotential && gameManager.GetRemainingResource(GemColor.Gray) <= 0){
      return gameManager.combatManager.hero.potentialGO.transform.position;
      //return Camera.main.ScreenToWorldPoint(gameManager.combatManager.hero.potentialGO.transform.position);
    }
    Vector3 offset = new Vector3(20, 10);
    int index = DataManager.instance.saveData.power + incomingPower;
    incomingPower++;
    return powerBars[index % powerBars.Count].transform.position - offset;
  }

  public bool ActivateBar(bool useToken)
  {
    if (DataManager.instance.saveData.emptyPowerToPotential && gameManager.GetRemainingResource(GemColor.Gray) <= 0){
      gameManager.ModifyResource(Resource.Potential, 1);
      //return Camera.main.ScreenToWorldPoint(gameManager.combatManager.hero.potentialGO.transform.position);
      return false;
    }
    if (useToken)
    {
      gameManager.powerContainer.RemoveToken();
    }
    for (int i = 0; i < requiredPower; i++)
    {
      powerBars[i].SetVisible(true);
    }
    powerBars[DataManager.instance.saveData.power].Activate(powerSprite);
    incomingPower--;
    DataManager.instance.saveData.power++;
    if (DataManager.instance.saveData.power == requiredPower)
    {
      LevelUp();
      return true;
    }
    return false;
  }

  public void UpdateBars()
  {
    bool noPower = DataManager.instance.saveData.powerLevel == 1 && DataManager.instance.saveData.power == 0;
    powerLevelOrb.SetActive(noPower == false);

    for (int i = 0; i < requiredPower; i++)
    {
      powerBars[i].SetActive(i < DataManager.instance.saveData.power);
      powerBars[i].SetVisible(noPower == false);
    }
  }

  public void LevelUp()
  {
    DataManager.instance.saveData.powerLevel++;
    if (DataManager.instance.saveData.lifeGrowthFromPower){
      gameManager.ModifyResource(Resource.MaxLife, 2);
    }
    gameManager.TestUnlocking(UnlockKey.PowerLevel, DataManager.instance.saveData.powerLevel);
    DataManager.instance.saveData.power = 0;
    int barCount = requiredPower;
    requiredPower = GetRequiredPower();
    if (barCount < requiredPower)
    {
      PowerBar bar = Instantiate(powerBarPrefab, transform);
      powerBars.Add(bar);
    }

    foreach (PowerBar bar in powerBars)
    {
      bar.Deactivate();
    }
    powerLevelText.text = DataManager.instance.saveData.powerLevel.ToString();
    powerLevelOrb.SetActive(true);
  }

  public int GetRequiredPower()
  {
    return DataManager.instance.saveData.GetRequiredPower();
  }
}
