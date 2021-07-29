using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI textObject;
    public Vector3 vector;
    public float lifeTime;
    public float totalTime;
    public bool fading;
    public float fadingDelay;

    private void Update()
    {
        transform.localPosition += vector * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (fading)
        {
            Color c = textObject.color;
            textObject.color = new Color(c.r, c.g, c.b, lifeTime / totalTime + fadingDelay);
        }
        if (lifeTime < 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Set(string text, int fontSize, Color color, Vector2 vector, float duration, bool fading = false, float fadeDelay = 0f)
    {
        SetText(text);
        SetFont(fontSize);
        SetColor(color);
        SetVelocity(vector);
        SetLifeTime(duration);
        SetFading(fading, fadeDelay);
    }

    public void SetText(string text)
    {
        textObject.text = text;
    }

    public void SetColor(Color color)
    {
        textObject.color = color;
    }

    public void SetFont(int fontSize)
    {
        textObject.fontSize = fontSize;
    }

    public void SetVelocity(Vector2 vec)
    {
        vector = vec;
    }

    public void SetLifeTime(float time)
    {
        totalTime = lifeTime = time;
    }

    public void SetPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void SetFading(bool fading, float fadingDelay = 0f)
    {
        this.fading = fading;
        this.fadingDelay = fadingDelay;
    }

}
