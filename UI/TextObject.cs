using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextObject : MonoBehaviour
{
  public string translationKey;
  public bool updateOnAwake = true;
  public bool translate = true;
  public StringFormat stringFormat;
  public Dictionary<string, string> translationValues;
  public TextMeshProUGUI textObject;

  public void Awake()
  {
    if (updateOnAwake && translationKey != null || translationKey != "")
    {
      SetText(translationKey, translationValues);
    }
  }

  public void Start()
  {
    if (updateOnAwake && translationKey != null || translationKey != "")
    {
      SetText(translationKey, translationValues);
    }
  }

  public void SetRawText(string str)
  {
    textObject.text = str;
  }


  public void SetText(string key, Dictionary<string, string> options = null)
  {
    if (translate == false)
      return;

    translationKey = key;
    translationValues = options;
    string str = Translation.GetTranslation(key, options);
    switch (stringFormat)
    {
      case StringFormat.normal:
        break;
      case StringFormat.capitalized:
        str = Translation.Capitalize(str);
        break;
      case StringFormat.upcase:
        str = str.ToUpper();
        break;
    }
    textObject.text = str;
  }

  public void Reload()
  {
    SetText(translationKey, translationValues);
  }

}

public enum StringFormat { normal, capitalized, upcase }