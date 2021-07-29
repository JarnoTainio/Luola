using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttributeText : MonoBehaviour
{
    public float maxSize = 1.25f;
    public float duration = 1f;
    public float baseFont = 32;
    public bool growAndFade;
    public TextMeshProUGUI text;
    AttributeState state;
    float counter;

    [Header("Audio")]
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        text = GetComponent<TextMeshProUGUI>();
        state = AttributeState.idle;
    }

    private void Update()
    {
        if (state != AttributeState.idle)
        {
            counter += Time.deltaTime;
            if (state == AttributeState.growing)
            {
                if (growAndFade)
                {
                    Color c = text.color;
                    text.color = new Color(c.r, c.g, c.b, (1f - (counter / duration)));
                    float f = counter * 2 / duration;
                    float size = 1f + (maxSize - 1f) * f;
                    text.fontSize = baseFont * size;

                    if (counter  >= duration)
                    {
                        text.color = new Color(c.r, c.g, c.b, 0f);
                        state = AttributeState.idle;
                    }
                }
                else if (counter >= duration / 2)
                {
                    state = AttributeState.shrinking;
                    text.fontSize = baseFont * maxSize;
                }
                else
                {
                    float f = counter * 2 / duration;
                    float size = 1f + (maxSize - 1f) * f;
                    text.fontSize = baseFont * size;
                }
            }
            else
            {

                if (counter >= duration)
                {
                    if (growAndFade)
                    {
                        Color c = text.color;
                        text.color = new Color(c.r, c.g, c.b, 0f);
                    }
                    else
                    {
                        text.fontSize = baseFont;
                    }
                    state = AttributeState.idle;
                }
                else
                {

                    float f = 1f - (counter - duration / 2) / (duration / 2);
                    float size = 1f + (maxSize - 1f) * f;
                    text.fontSize = baseFont * size;
                }
            }
        }
    }

    public void Set(string message, bool playSound = false)
    {
        text.text = message;
        state = AttributeState.growing;
        counter = 0f;
        if (playSound && audioSource != null)
        {
            audioSource.Play();
        }
    }


}

public enum AttributeState { idle, growing, shrinking}