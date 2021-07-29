using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
  [Header("References")]
  public GameManager gameManager;

  public Image introImage;

  public void Start()
  {
      StartCoroutine(FadeIn());
        
  }
  public IEnumerator FadeIn()
  {
    /*
    float time = 0f;
    float duration = 1f;
    introImage.gameObject.SetActive(true);

    while (time < duration)
    {
        time += Time.deltaTime;
        float f = 1f - time / duration;
        introImage.color = new Color(0, 0, 0, f);
        yield return null;
    }
    introImage.gameObject.SetActive(false);
*/
yield return null;

    StartCoroutine(gameManager.Init());
  }
}
