using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
  public AttributeText turnText;
  public AttributeText goldText;
  public AttributeText actionText;
  public Image fadeInImage;
  public TextMeshProUGUI levelText;

  public GameObject rewardScreen;
  public Button replayButton;

  public AssetList heroList;
  public SpriteRenderer heroImage;

  public Image turnCounterBackground;
  public bool turnCounterAlert;
  public float alertCounter;
  public EscapeButton escapeButton;
  private void Awake()
  {
    fadeInImage.gameObject.SetActive(true);
  }

  private void Update()
  {
    if (turnCounterAlert)
    {
      alertCounter += Time.deltaTime * 0.75f;
      if (alertCounter >= 2f)
        alertCounter -= 2f;
      //float c = Mathf.Abs((alertCounter - 1f));
      float c = Mathf.Abs((alertCounter - 1f)) * 0.5f + 0.5f;
      if (c < 0.35f){
        c = 0.35f;
      }
      turnCounterBackground.color = new Color(c, c, c);
      turnText.text.color = new Color(c, c, c);
    }
  }

  public void Init()
  {
    StartCoroutine(FadeIn(DataManager.instance.saveData.roomName == "reward" ? 1.5f : 3f));

    heroImage.sprite = DataManager.instance.saveData.GetHero(heroList).sprite;
    //((HeroAsset)heroList.items[DataManager.instance.saveData.heroIndex]).sprite; //heroSprites[DataManager.instance.saveData.heroIndex];
  }

  public IEnumerator FadeIn(float duration)
  {
    float f = duration;
    while (f > 0)
    {
      f -= Time.deltaTime;
      float value = f / duration;
      fadeInImage.color = new Color(0, 0, 0, value);
      levelText.color = new Color(1, 1, 1, value);
      yield return null;
    }
    fadeInImage.gameObject.SetActive(false);
  }

  public void SetMessage(string message, StringFormat format)
  {
    string str = Translation.GetTranslation(message, format);
    actionText.Set(str);
  }

  public void SetMessage(string message, Dictionary<string, string> options = null)
  {
    string str = Translation.GetTranslation(message, options);
    actionText.Set(str);
  }

  public void SetMovesRemaining(int amount)
  {
    TurnCounterAlert(amount == 1);
    turnText.Set(amount.ToString());
  }

  public void TurnCounterAlert(bool active)
  {
    turnCounterAlert = active;
    turnCounterBackground.color = Color.white;
  }

}
