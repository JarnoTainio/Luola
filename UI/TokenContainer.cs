using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenContainer : MonoBehaviour
{
  public Sprite tokenSprite;
  public Image tokenPrefab;
  public float fadeTime = 0.5f;
  private List<Image> tokens;

  public void SetTokens(int count)
  {
    tokens = new List<Image>();
    for (int i = 0; i < count; i++)
    {
      Image token = Instantiate(tokenPrefab, transform);
      token.sprite = tokenSprite;
      tokens.Add(token);
    }

    GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
    RectTransform rect = GetComponent<RectTransform>();
    float tokenHeight = grid.cellSize.y;
    float size = rect.sizeDelta.y / count;
    grid.spacing = new Vector2(0, Mathf.Min(0, size - tokenHeight));
  }

  public void AddTokens(int count){
    for (int i = 0; i < count; i++)
    {
      Image token = Instantiate(tokenPrefab, transform);
      token.sprite = tokenSprite;
      tokens.Add(token);
    }

    GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
    RectTransform rect = GetComponent<RectTransform>();
    float tokenHeight = grid.cellSize.y;
    float size = rect.sizeDelta.y / tokens.Count;
    grid.spacing = new Vector2(0, Mathf.Min(0, size - tokenHeight));
  }

  public bool RemoveToken()
  {
    if (tokens.Count > 0)
    {
      Image token = tokens[tokens.Count - 1];
      tokens.Remove(token);
      StartCoroutine(FadeToken(token));
      return true;
    }
    return false;
  }

  public bool IsEmpty()
  {
    return tokens.Count == 0;
  }

  public int GetCount()
  {
    return tokens.Count;
  }

  public IEnumerator FadeToken(Image token)
  {

    float f = fadeTime;
    while (f > 0f)
    {
      token.color = new Color(1f, 1f, 1f, f / fadeTime);
      f -= Time.deltaTime;
      yield return null;
    }
    Destroy(token.gameObject);
  }
}
