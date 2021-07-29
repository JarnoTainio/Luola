using UnityEngine;

public class AudioObject : MonoBehaviour
{
  public AudioSource audioSource;
  public AudioType audioType;
  public float volumeAdjust = 1f;
  void Start()
  {
    UpdateLevel();
  }

  public void UpdateLevel(){
    if (audioType == AudioType.Music)
    {
      audioSource.volume = DataManager.instance.GetMusicLevel() * volumeAdjust;
    }
    else if (audioType == AudioType.SFX)
    {
      audioSource.volume = DataManager.instance.GetSFXLevel() * volumeAdjust;
    }
  }
}
