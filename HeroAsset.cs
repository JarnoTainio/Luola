using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "GemRPG/Hero")]
public class HeroAsset : ScriptableObject
{
  public int unlocked;
  public bool isPlayable = false;

  public string descriptionKey;
  public Sprite sprite;
  public Vector3 scale;
  public Vector3 position;
  public bool flying = false;
  public bool ghost = false;
  public float speedModifier = 1f;

  [HeaderAttribute("Stats")]
  public int life = 20;
  public int gold = 0;
  public int roomGold = 0;
  public int roomPower = 0;

  [HeaderAttribute("Modifiers")]
  public int actionModifier = 0;
  public int attackDecay = 0;
  public float maxLifeGainModifier = 1f;
  public int requiredPowerModifier = 0;
  public float rewardPriceModifier = 1f;
  public float monsterLifeModifier = 1f;
  public int floorHeight = 8;
  public int gemBoardHeight = 6;
  public int gemBoardWidth = 6;
  public int matchRequired = 3;  
  public List<GameElement> disabledGameElements;
  public int[] floorAggression = {0,0,0};

  [HeaderAttribute("Triggers")]
  public bool energyShield = false;
  public bool lifeGrowthFromPower = false;
  public bool bloodEnergy = false;
  public bool damageTakenDoubleTrigger = false;
  public bool emptyPowerToPotential = false;
  public bool emptyGoldToPotential = false;
  [HeaderAttribute("Gem weights")]
  public int redGemWeight = 100;
  public int blueGemWeight = 100;
  public int purpleGemWeight = 0;

  [HeaderAttribute("Starting gear")]
  public Ability[] abilities;
  public RewardItem[] items;
  public GemItem[] gems;
}
