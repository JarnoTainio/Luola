using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventButton : MonoBehaviour
{
  public RoomManager roomManager;
  public TextObject text;
  public Button button;
  public int index;

  public void Clicked()
  {
    roomManager.OptionClicked(index);
  }

  public void Selected(bool selected)
  {
    if (selected)
    {
      GetComponent<Image>().color = Color.green;
    }
    else
    {
      GetComponent<Image>().color = Color.white;
    }
    //roomManager.Hovering(index);
  }

  public void EndHover()
  {
    //roomManager.ExitHovering(index);
  }

  public void SetText(string str, bool selectable)
  {
    text.SetText(str);
    button.interactable = selectable;
    text.textObject.color = selectable ? Color.black : new Color(0, 0, 0, 0.5f);
  }

  public void SetText(ResourceAmount[] resources, bool selectable)
  {
    text.SetRawText(Translation.TranslateResouce(resources));
    button.interactable = selectable;
    text.textObject.color = selectable ? Color.black : new Color(0, 0, 0, 0.5f);
  }
}
