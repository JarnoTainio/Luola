using UnityEngine;
using TMPro;

public class ProfileButton : MonoBehaviour
{
  public int index;
  public TextObject text;
  public GameObject deleteButton;
  public SaveData saveData;

  public void Init(int index, SaveData data)
  {
    this.index = index;
    this.saveData = data;
    if (data != null)
    {
      text.translate = false;
      text.SetRawText(data.name.ToUpper());
      deleteButton.SetActive(true);
    }
    else
    {
      text.translate = true;
      text.SetText("newGame");
      deleteButton.SetActive(false);
    }
  }
}
