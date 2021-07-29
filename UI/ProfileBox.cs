using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileBox : MonoBehaviour
{
    public TextMeshProUGUI profileName;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI percentageText;

    public TextObject buttonText;
    public TextObject currentText;
    public SaveData saveData;
    public GameObject deleteButton;
    public int index;
    public MenuManager menuManager;

    public void Show(MenuManager menuManager, SaveData saveData, int index, bool isContinueButton = false){
        this.menuManager = menuManager;
        this.saveData = saveData;
        this.index = index;

        if (saveData != null){
            gameObject.SetActive(true);
            profileName.text = saveData.name;
            levelText.text = saveData.profileLevel.ToString();
            buttonText.SetText("play");
            string str = "inUnknown";
            switch (saveData.roomName){
                case "combat":
                str = "inCombat";
                break;

                case "reward":
                str = "inReward";
                break;

                case "map":
                str = "inMap";
                break;

                case "event":
                str = "inEvent";
                break;

                case "newGame":
                str = "inNewGame";
                break;
            }
            currentText.gameObject.SetActive(true);
            currentText.SetText(str, new Dictionary<string, string>{
                {"room", saveData.room.ToString()},
                {"floor", (saveData.floor + 1).ToString()}
            });
            percentageText.text = $"{saveData.GetPercentage()}%";
            deleteButton.SetActive(isContinueButton == false && true);
        }
        else{
            if (isContinueButton){
                gameObject.SetActive(false);
            }
            else{
                profileName.text = Translation.GetTranslation("profile", new Dictionary<string, string>(){{"index", (index + 1).ToString()}});
                levelText.text = "0";
                buttonText.SetText("play");
                currentText.gameObject.SetActive(false);
                percentageText.text = "";
                deleteButton.SetActive(false);
            }
        }
    }

    public void Play(){
        menuManager.StartGame(index);
    }

    public void Delete(){

    }

}
