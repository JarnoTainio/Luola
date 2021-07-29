using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gem : MonoBehaviour
{
  public float speed = 5f;
  public Vector2Int position;
  public SpriteRenderer effectSprite;
  public GameObject effectObject;

  [Header("Colors")]
  public Color redGemColor;
  public Color blueGemColor;
  public Color greenGemColor;
  public Color yellowGemColor;
  public Color grayGemColor;
  public Color purpleGemColor;
  public Color crimsomGemColor;

  SpriteRenderer spriteRenderer;

  [HideInInspector]
  public GemContainer gemContainer;

  public Color color;
  public GemItem gemItem;
  private bool moving;
  public bool destroyed = false;

  public ParticleSystem explosion;

  [Header("Audio")]
  public AudioSource audioSource;
  public AudioClip breakSound;

  private void Awake()
  {
      spriteRenderer = GetComponent<SpriteRenderer>();
  }
  void Update()
  {
      if (moving)
      {
          transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(position.x, position.y, transform.localPosition.z), speed * Time.deltaTime);
          if (Vector2.Distance(transform.localPosition, position) < 0.01f)
          {
              moving = false;
              gemContainer.DoneMoving(this);
          }
      }
  }


  public void SetColor(GemItem gemItem, bool hasResourceLeft)
  {
    this.gemItem = gemItem;
    this.color = GetSpriteColor(gemItem.color);
    //GetComponent<SpriteRenderer>().color = GetSpriteColor(color);
    UpdateSprite();
    if (!hasResourceLeft)
    {
      //GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f);
    }
  }

  public void UpdateSprite(){
    Sprite[] spriteArray = new Sprite[0];
    switch (gemItem.color)
    {
      case GemColor.Red: spriteArray = gemContainer.redGems; break;
      case GemColor.Blue: spriteArray = gemContainer.blueGems; break;
      case GemColor.Green: spriteArray = gemContainer.greenGems; break;
      case GemColor.Yellow: spriteArray = gemContainer.yellowGems; break;
      case GemColor.Gray: spriteArray = gemContainer.grayGems; break;
      case GemColor.Purple: spriteArray = gemContainer.purpleGems; break;
      case GemColor.None: spriteArray = new Sprite[]{gemContainer.rockGem}; break;
    }
    GetComponent<SpriteRenderer>().sprite = spriteArray[(int)gemItem.type];
  }

  public void SetEffect(GemEffect effect, Sprite sprite, int value = 1)
  {
    gemItem.effect = effect;
    gemItem.effectValue = value;
    effectSprite.sprite = sprite;
    effectObject.SetActive(sprite != null);
  }

  public Vector2Int Move(Vector2Int newPosition, bool emptyOldPosition = true)
  {
    if (emptyOldPosition)
    {
      gemContainer.gemGrid[position.x, position.y] = null;
    }
    gemContainer.gemGrid[newPosition.x, newPosition.y] = this;

    moving = true;
    Vector2Int direction = newPosition - position;
    position = newPosition;
    gemContainer.Moving(this, direction);
    return direction;
  }

  public void Destroy(float delay, bool playEffect = true)
  {
    gemItem.isReady = true;
    if (playEffect)
    {
      StartCoroutine(DestroyEffect(delay));
    }
    else
    {
      effectSprite.sprite = null;
      effectObject.SetActive(false);
      gemContainer.Remove(this);
      Destroy(gameObject);
    }
  }

  private IEnumerator DestroyEffect(float delay)
  {
    gemContainer.Remove(this);
    yield return new WaitForSeconds(delay);
    ParticleSystem exp = Instantiate(explosion, transform);
    ParticleSystem.MainModule ma = exp.main;
    ma.startColor = color;
    exp.Play();
    GetComponent<SpriteRenderer>().sprite = null;
    effectSprite.sprite = null;
    effectObject.SetActive(false);

    audioSource.clip = breakSound;
    audioSource.Play();

    Destroy(gameObject, 1);
  }

  public void OnMouseDown()
  {
    if (EventSystem.current.IsPointerOverGameObject() == false)
    {
      gemContainer.GemClicked(this);
    }
  }

  public void OnMouseEnter()
  {
    if (EventSystem.current.IsPointerOverGameObject() == false)
    {
      gemContainer.GemHovering(this);
    }
  }

  public bool IsMatch(Gem gem)
  {
      if (gem == null)
      {
          return false;
      }
      return gemItem.color == gem.gemItem.color;
  }

  public bool IsMatch(GemColor otherColor)
  {
      return gemItem.color != GemColor.None && otherColor != GemColor.None && otherColor == gemItem.color;
  }

  public void SetUsable(bool isUsable)
  {
      spriteRenderer.color = isUsable ? UnityEngine.Color.white : UnityEngine.Color.grey;
  }

  public void Trigger()
  {
    switch (gemItem.type)
    {
      case GemType.Normal:
        {
           break;
        }
      case GemType.Pure:
        {
           break;
        }
    }
  }

  public Color GetSpriteColor(GemColor color)
  {
    switch (color)
    {
      case GemColor.Red: return redGemColor;
      case GemColor.Blue: return blueGemColor;
      case GemColor.Green: return greenGemColor;
      case GemColor.Yellow: return yellowGemColor;
      case GemColor.Gray: return grayGemColor;
      case GemColor.Purple: return purpleGemColor;
      case GemColor.Crimsom: return crimsomGemColor;
      default: return UnityEngine.Color.white;
    }
  }

  public GemColor Color() { return gemItem.color; }
  public GemType Type() { return gemItem.type; }

  public static GemColor ResourceToColor(Resource resource)
  {
    switch (resource)
    {
      case Resource.Attack:
        return GemColor.Red;
      case Resource.Defence:
        return GemColor.Blue;
      case Resource.Energy:
        return GemColor.Green;
      case Resource.Gold:
        return GemColor.Yellow;
      case Resource.Life:
        return GemColor.Crimsom;
      case Resource.Power:
        return GemColor.Gray;
      default:
        return GemColor.None;
    }
  }

  public override string ToString()
  {
    return $"{gemItem.color} {gemItem.type} {position}";
  }
}

[System.Serializable]
public class GemItem
{
  public GemColor color;
  public GemType type;
  public GemEffect effect;
  public int effectValue;
  public bool isReady;

  public GemItem()
  {

  }

  public GemItem(GemColor color, GemType type)
  {
    this.color = color;
    this.type = type;
    effect = GemEffect.None;
    effectValue = 0;
  }

  public GemItem Copy()
  {
    return new GemItem(color, type);
  }

  public string GetName()
  {
    string str = "";
    switch (type)
    {
      case GemType.Bomb:
        str += "bomb";
        break;
      case GemType.Throw:
        str += "slider";
        break;
      case GemType.Pure:
        str += "pure";
        break;
      case GemType.Glow:
        str += "glow";
        break;
      case GemType.Egg:
        str += "egg";
        break;
    }
    switch (color)
    {
      case GemColor.Red:
        str += "Red";
        break;
      case GemColor.Blue:
        str += "Blue";
        break;
      case GemColor.Green:
        str += "Green";
        break;
      case GemColor.Gray:
        str += "Gray";
        break;
      case GemColor.Yellow:
        str += "Yellow";
        break;
      case GemColor.Purple:
        str += "Purple";
        break;
    }

    return str;
  }
}

[System.Serializable]
public class SlidingGem
{
  public Gem gem;
  public Vector2Int direction;
  public int distanceTraveled;
  public SlidingGem(Gem gem, Vector2Int direction)
  {
    this.gem = gem;
    this.direction = direction;
  }
}

public enum GemColor { Red = 0, Blue, Green, Yellow, Gray, Purple, None, Crimsom}
public enum GemType { Normal = 0, Bomb, Pure, Throw, Glow, Egg, Double }

public enum GemEffect { None = 0, Damage, Attack, Defence, Rock }