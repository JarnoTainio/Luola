using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Announcement : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image background;
    public Image image;

    public List<string> queue = new List<string>();

    public void ShowHeroUnlock(int index){
        List<string> heroUnlockTexts = new List<string>(){
            "", "heroUnlockWin", "heroUnlockGold", "heroUnlockPower", "heroUnlockLife",
            "heroUnlockDamage", "heroUnlockOverkill", "heroUnlockNoDamage"
        };
        Dictionary<string, string> values = new Dictionary<string, string>();
        if (index == 6){
            values["overkill"] = DataManager.instance.saveData.totalOverkill.ToString();
        }
        string str = "";
        if (index < heroUnlockTexts.Count){
            str = heroUnlockTexts[index];
        }else{
            str = "heroUnlockAdd";
        }
        Show(Translation.GetTranslation("heroUnlocked") + "\n" + Translation.GetTranslation(str, values));
    }
    public void Show(string str, float wait = 3f, float duration = 1f){
        text.text = str;
        gameObject.SetActive(true);
        queue.Add(str);
        if (queue.Count == 1){
            StartCoroutine(FadeOut(wait, duration));
        }
    }

    private IEnumerator FadeOut(float wait, float duration){
        background.color = Color.white;
        text.color = Color.white;
        image.color = Color.white;

        float f = 0f;
        while (f < wait){
            f += Time.deltaTime;
            yield return null;
        }

        f = 0f;
        while (f < duration){
            f += Time.deltaTime;
            Color c = new Color(1,1,1, 1f - (f / duration));
            text.color = new Color(text.color.r, text.color.g, text.color.b, c.a);
            background.color = c;
            image.color = c;
            yield return null;
        }
        queue.Remove(queue[queue.Count - 1]);
        if (queue.Count > 0){
            text.text = queue[queue.Count - 1];
            StartCoroutine(FadeOut(wait, duration));
        }
        else{
            gameObject.SetActive(false);
        }
    }

    public bool IsActive(){
        return queue.Count > 0;
    }
}
