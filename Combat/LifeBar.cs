using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
  public Image lifeImage;
  
  public void SetLifeRatio(float ratio)
  {
    lifeImage.fillAmount = ratio;
  }
  
}
