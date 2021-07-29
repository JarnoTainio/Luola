using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
  public Image image;
  public Image background;
  public Image frame;

  public void SetVisible(bool visible){
    Color c = visible ? Color.white : new Color(1,1,1,0);
    image.color = c;
    frame.color = c;
    background.color = visible ? Color.black : c;
  }
  public void SetActive(bool active)
  {
    if (active)
    {
      image.gameObject.SetActive(true);
    }
    else
    {
      image.gameObject.SetActive(false);
    }
  }

  public void Activate(Sprite sprite)
  {
    image.sprite = sprite;
    image.gameObject.SetActive(true);
  }

  public void Deactivate()
  {
    image.gameObject.SetActive(false);
  }
}
