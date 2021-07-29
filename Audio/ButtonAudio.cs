using UnityEngine;
using UnityEngine.UI;


public class ButtonAudio : AudioObject
{
    public AudioClip enabledSound;
    public AudioClip disabledSound;

    Button button;

    public void Start(){
        audioType = AudioType.SFX;
        UpdateLevel();
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }
    public void PlaySound(){
        audioSource.clip = button.interactable ? enabledSound : disabledSound;
        audioSource.Play();
    }
}
