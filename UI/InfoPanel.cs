using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public TextMeshProUGUI infoText;

    public void Open(){
        gameObject.SetActive(true);
        infoText.text = "";
        SaveData data = DataManager.instance.saveData;

        // Run details
        infoText.text += $"{Translation.GetTranslation("timePlayed")}: {data.TimeString((int)data.totalTime)}";
        infoText.text += $"\n{Translation.GetTranslation("difficulty")}: {Translation.GetTranslation("difficulty" + data.difficulty)}";
        infoText.text += $"\n{Translation.GetTranslation("damageTaken", StringFormat.capitalized)}: {data.damageTaken}";
        if (data.overkill > 0){
            infoText.text += $"\n{Translation.GetTranslation("overkill", StringFormat.capitalized)}: {data.overkill}";
        }
        if (data.rewardsBought > 0){
            infoText.text += $"\n{Translation.GetTranslation("rewardsBought")}: {data.rewardsBought}";
        }
        infoText.text += "\n";

        infoText.text += $"\n{Translation.GetTranslation("heroLevel", StringFormat.capitalized)}: {data.powerLevel} xp: {data.power} / {data.GetRequiredPower()}";
        if (data.maxLifeGainModifier != 1f){
            infoText.text += $"\n{Translation.GetTranslation("maximumLifeModifiers", StringFormat.capitalized)}: {FrontChar(data.maxLifeGainModifier - 1)}{ToPercentage(data.maxLifeGainModifier  )}%";
        }
        if (data.roomExtraGold != 0){
            infoText.text += $"\n{Translation.GetTranslation("roomGold")}: {FrontChar(data.roomExtraGold)}{data.roomExtraGold}";
        }
        if (data.roomExtraPower != 0){
            infoText.text += $"\n{Translation.GetTranslation("roomPower")}: {FrontChar(data.roomExtraPower)}{data.roomExtraPower}";
        }
        if (data.requiredPowerModifier != 0){
            infoText.text += $"\n{Translation.GetTranslation("powerRequiredModifier")}: {FrontChar(data.requiredPowerModifier)}{data.requiredPowerModifier}";
        }
        if (data.rewardPriceModifier != 1f){
            infoText.text += $"\n{Translation.GetTranslation("chestPriceModifier")}: {FrontChar(data.rewardPriceModifier - 1)}{ToPercentage(data.rewardPriceModifier)}%";
        }
        if (data.defenceDecay != 0){
            infoText.text += $"\n{Translation.GetTranslation("defenceDecay")}: {data.defenceDecay}";
        }
        if (data.attackDecay != 0){
            infoText.text += $"\n{Translation.GetTranslation("attackDecay")}: {data.attackDecay}";
        }
        if (data.redGemWeight != 100){
            infoText.text += $"\n{Translation.GetTranslation("redGemsModifier")}:  {FrontChar(data.redGemWeight - 100)}{data.redGemWeight - 100}%";
        }
        if (data.blueGemWeight != 100){
            infoText.text += $"\n{Translation.GetTranslation("blueGemsModifier")}:  {FrontChar(data.blueGemWeight - 100)}{data.blueGemWeight - 100}%";
        }
        infoText.text += $"\n{Translation.GetTranslation("yellowGemsNoTokens")}: {FrontChar(data.noGoldGemWeight - 100)}{data.noGoldGemWeight - 100}%";
        infoText.text += $"\n{Translation.GetTranslation("grayGemsNoTokens")}: {FrontChar(data.noPowerGemWeight - 100)}{data.noPowerGemWeight -100 }%";
        infoText.text += "\n";

        if (data.monsterLifeModifier != 1.0f){
            infoText.text += $"\n{Translation.GetTranslation("monsterLife")}: {FrontChar(data.monsterLifeModifier - 1)}{ToPercentage(data.monsterLifeModifier)}%";
        }
        if (data.aggroModifier != 0){
            infoText.text += $"\n{Translation.GetTranslation("monsterAgro")}: {FrontChar(data.aggroModifier)}{data.aggroModifier}";
        }
        if (data.bloodEnergy){
            infoText.text += $"\n{Translation.GetTranslation("bloodMagic")}";
        }
        if (data.energyShield){
            infoText.text += $"\n{Translation.GetTranslation("energyShield")}";
        }
        if (data.lifeGrowthFromPower){
            infoText.text += $"\n{Translation.GetTranslation("lifeGrowth")}";
        }

    }

    private string FrontChar(float value){
        return value > 0f ? "+" : "";
    }

    private int ToPercentage(float value){
        return (int)((value - 1f) * 100);
    }
}

