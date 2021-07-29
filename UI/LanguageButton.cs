using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
  public Language language;
  public Image image;
  public void Start()
  {
    image.color = DataManager.instance.gameSettings.language == language ? Color.white : Color.gray;
  }


  public void SetLanguage()
  {
    if (DataManager.instance.gameSettings.language != language)
    {
      DataManager.instance.gameSettings.language = language;
      foreach (TextObject to in Resources.FindObjectsOfTypeAll(typeof(TextObject)))
      {
        to.Reload();
      }
      foreach (LanguageButton lb in Object.FindObjectsOfType<LanguageButton>())
      {
        lb.image.color = language == lb.language ? Color.white : Color.gray;
      }
    }

  }
}
