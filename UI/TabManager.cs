using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Color unselectedColor;
    public List<Button> buttons;

    public void Start(){
        SelectTab(buttons[0]);
    }

    public void SelectTab(Button button){
        foreach(Button b in buttons){
            b.image.color = (b == button ? Color.white : unselectedColor);
        }
    }
}
