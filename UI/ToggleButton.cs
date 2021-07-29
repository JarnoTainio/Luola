using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToggleButton : MonoBehaviour
{
    public Image image;
    public Color disabledColor;

    public List<ToggleButton> buttonGroup;

    public void Toggle(bool active){
        if (active){
            foreach(ToggleButton button in buttonGroup){
                button.Toggle(false);
            }
            image.color = Color.white;
        }else{
            image.color = disabledColor;
        }
    }
}
