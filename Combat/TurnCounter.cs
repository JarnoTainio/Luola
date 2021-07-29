using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCounter : MonoBehaviour
{
    public Image image;
    public Image backgroundImage;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite goldSprite;
    public Sprite powerSprite;
    public Sprite eliteSprite;
    public Sprite bossSprite;

    [Header("Colors")]
    public Color normalColor;
    public Color goldColor;
    public Color powerColor;
    public Color eliteColor;
    public Color bossColor;

    void Start()
    {
        switch (DataManager.instance.saveData.roomTag){
            case RoomTag.None:
                if (DataManager.instance.saveData.bossRoom){
                    SetSprite(bossSprite, bossColor);
                }
                else{
                    SetSprite(normalSprite, normalColor);
                }
                break;
            case RoomTag.Gold:
                SetSprite(goldSprite, goldColor);
                break;
            case RoomTag.Power:
                SetSprite(powerSprite, powerColor);
                break;
            case RoomTag.Elite:
                SetSprite(eliteSprite, eliteColor);
                break;
        }
    }

    public void SetSprite(Sprite sprite, Color color){
        backgroundImage.sprite = sprite;
        //backgroundImage.color = color;
    }
}
