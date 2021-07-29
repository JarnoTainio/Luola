using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LifeCounter : MonoBehaviour
{
    public TextMeshProUGUI lifeText;
    public Image lifeImage;

    void Start()
    {
        UpdateLife();
    }

    public void UpdateLife(){
        SaveData saveData = DataManager.instance.saveData;
        lifeText.text = saveData.life + " / " + saveData.maxLife;
        lifeImage.fillAmount = (float) saveData.life / saveData.maxLife;
    }
}

