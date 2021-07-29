using UnityEngine;

public class InputController : MonoBehaviour
{
  public FadeIn fade;
  public bool active = false;
  public GameObject[] dialogs;

  void LateUpdate()
  {
    if (active == false && Input.GetKey(KeyCode.Escape))
    {
      if (dialogs != null){
        foreach(GameObject go in dialogs){
          if (go.activeSelf){
            return;
          }
        }
      }
      active = false;
      fade.StartFadeOut(LoadMenu);
    }
  }

  public void LoadMenu(bool useless)
  {
    DataManager.instance.LoadMenuScene();
  }
}
