using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDialogManager : MonoBehaviour
{
    public FadeIn fadeIn;
    public void AbandonRun(){
        DataManager.instance.victory = false;
        fadeIn.StartFadeOut(DataManager.instance.GameOver);
    }

    public void ReturnToMenu(){
        fadeIn.StartFadeOut(DataManager.instance.LoadMenuScene);
    }
}
