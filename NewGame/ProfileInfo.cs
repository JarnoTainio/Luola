using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileInfo : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI text;

    public void Start(){
        SaveData data = DataManager.instance.saveData;
        title.text = data.name;
        StringFormat capitalize = StringFormat.capitalized;
        
        // Level
        text.text = $"{Translation.GetTranslation("profile_level", capitalize)}: {data.profileLevel}";
        text.text += $"\n{Translation.GetTranslation("experience", capitalize)}: {data.profileExperience} / {data.GetRequiredProfileExperience()}";
        text.text += "\n";

        // Runs
        text.text += $"\n{Translation.GetTranslation("play_time_total", capitalize)}: {data.TimeString(data.profileTime)}";
        text.text += $"\n{Translation.GetTranslation("runs", capitalize)}: {data.totalRuns}";
        if (data.totalRuns > 0){
            text.text += $"\n{Translation.GetTranslation("victories", capitalize)}: {data.victoryCount} ({(int)(100 * (float)data.victoryCount / data.totalRuns)}%)";
        }
        text.text += "\n";

        // Stats
        text.text += $"\n{Translation.GetTranslation("overkill", capitalize)}: {data.totalOverkill}";

        // Adds
        text.text += $"\n{Translation.GetTranslation("ads_watched", capitalize)}: {data.addsWatched}";
    }
}
