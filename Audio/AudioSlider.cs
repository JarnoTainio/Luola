using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
  public AudioType target;
  public Slider slider;

  void Start()
  {
    if (target == AudioType.Master)
    {
      slider.value = DataManager.instance.gameSettings.audioLevel;
    }
    else if (target == AudioType.Music)
    {
      slider.value = DataManager.instance.gameSettings.musicLevel;
    }
    else if (target == AudioType.SFX)
    {
      slider.value = DataManager.instance.gameSettings.sfxLevel;
    }
  }

  public void SetValue(float f)
  {
    if (target == AudioType.Master)
    {
      DataManager.instance.gameSettings.audioLevel = f;
    }
    else if (target == AudioType.Music)
    {
      DataManager.instance.gameSettings.musicLevel = f;
    }
    else if (target == AudioType.SFX)
    {
      DataManager.instance.gameSettings.sfxLevel = f;
    }

    AudioObject[] audioObjects = Object.FindObjectsOfType<AudioObject>();
    foreach (AudioObject a in audioObjects)
    {
        a.UpdateLevel();
    }
  }

}

public enum AudioType { Master, Music, SFX};
