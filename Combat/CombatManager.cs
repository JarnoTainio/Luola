
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
  [Header("References")]
  public Transform messageContainer;
  public GameManager gameManager;

  [Header("Prefabs")]
  public Sphere spherePrefab;
  public CursedSphere cursedSpherePrefab;
  public FloatingText floatingTextPrefab;

  [Header("Creatures")]
  public Unit hero;
  public Unit monster;
  public Monster monsterTemplate;

  [Header("Values")]
  public int fontSize = 32;

  public List<Sphere> spheres;
  public List<CursedSphere> curses;

  private void Start()
  {
      spheres = new List<Sphere>();
      curses = new List<CursedSphere>();
  }

  public void Init(bool loading)
  {
    hero.life = DataManager.instance.saveData.life;
    hero.maxLife = DataManager.instance.saveData.maxLife;
    hero.InitHero(DataManager.instance.saveData.GetHero(gameManager.uiManager.heroList));

    monsterTemplate = DataManager.instance.GetMonster(loading);
    monster.SetMonster(monsterTemplate, loading);
  }

  public void NewRound()
  {
    monster.AddAction(monsterTemplate.GetAction(gameManager.roundNumber));
    gameManager.NewRound();
  }

  public bool IsReady()
  {
    return spheres.Count == 0 && curses.Count == 0;
  }

  public void StartCombat()
  {
    StartCoroutine(hero.Attack());
  }

  public void CreateSphere(Gem gem, int value = 1, bool useToken = true, bool addPotentialInstead = false)
  {
    GemColor gemColor = addPotentialInstead ? GemColor.Purple : gem.gemItem.color;
    Vector3 target = GetSphereTarget(gemColor);
    while(value > 0)
    {
      value--;
      Vector3 position = gem.transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));

      Color sphereColor = gameManager.gemContainer.gemPrefab.GetSpriteColor(gemColor);
      Sphere sphere = CreateSphere(position, target, sphereColor, gemColor, value, useToken);
    }
  }

  public void CreatePotentialSphere(GemColor targetGemColor){
    Vector3 target = GetSphereTarget(targetGemColor);
    Vector3 position =   Camera.main.ScreenToWorldPoint(hero.potentialGO.transform.position + new Vector3(30, 30, 0 ));
    Color sphereColor = gameManager.gemContainer.gemPrefab.GetSpriteColor(targetGemColor);
    Sphere sphere = CreateSphere(position, target, sphereColor, targetGemColor, 1, true);
    sphere.speed *= .75f;
    float r = Random.Range(-0.5f, 0.5f);
    sphere.SetFirstTarget(new Vector3(r , sphere.transform.position.y + 3, 0));
  }

  public void CreateCurse(Gem gem){
      CursedSphere sphere = Instantiate(cursedSpherePrefab, gameManager.transform);
      sphere.combatManager = this;
      curses.Add(sphere);
      sphere.transform.position = gem.transform.position;

      if (gem.gemItem.effect == GemEffect.Damage)
      {
        sphere.SetTarget(GetSphereTarget(GemColor.Purple), gem.gemItem.effect, true);
      }
      else if (gem.gemItem.effect == GemEffect.Attack)
      {
        sphere.SetTarget(monster.attackText.transform.position, gem.gemItem.effect, true);
      }
      else if (gem.gemItem.effect == GemEffect.Defence)
      {
        sphere.SetTarget(monster.defenceText.transform.position, gem.gemItem.effect, true);
      }
  }

  public Vector3 GetSphereTarget(GemColor gemColor)
  {
    switch (gemColor)
    {
      case GemColor.Blue:
        {
          return hero.defenceText.transform.position + new Vector3(-50, -30, 0 );
        }
      case GemColor.Gray:
        {
          return gameManager.powerManager.GetCurrentPosition();
        }
      case GemColor.Green:
        {
          return hero.energyText.transform.position + new Vector3(-50, -30, 0 );
        }
      case GemColor.Red:
        {
          return hero.attackText.transform.position + new Vector3(-50, -30, 0 );
        }
      case GemColor.Yellow:
        {
          return gameManager.uiManager.goldText.transform.position + new Vector3(-50, -30, 0 );
        }
      case GemColor.Purple:
        {
          return hero.potentialText.transform.position + new Vector3(-50, -30, 0 );
        }
      case GemColor.Crimsom:
        {
          return hero.lifeText.transform.position + new Vector3(-50, -30, 0 );
        }
      default:
        {
          return new Vector3(900, 900);
        }
    }
  }

  public Vector3 GetSphereTarget(Resource resource)
  {
    switch (resource)
    {
      case Resource.Defence:
        {
          return hero.defenceText.transform.position;
        }
      case Resource.Power:
        {
          return gameManager.powerManager.GetCurrentPosition();
        }
      case Resource.Energy:
        {
          return hero.energyText.transform.position;
        }
      case Resource.Attack:
        {
          return hero.attackText.transform.position;
        }
      case Resource.Gold:
        {
          return gameManager.uiManager.goldText.transform.position;
        }
      case Resource.Life:
        {
          return hero.lifeText.transform.position;
        }
      default:
        {
          return new Vector3(900, 900);
        }
    }
  }

    public Sphere CreateSphere(Vector3 position, Vector3 targetPosition, Color color, GemColor gemColor, float waitTime, bool useToken)
  {
    Sphere sphere = Instantiate(spherePrefab, transform);
    sphere.transform.position = new Vector3(position.x, position.y, 0);
    sphere.combatManager = this;

    spheres.Add(sphere);
    StartCoroutine(SendSphere(sphere, targetPosition, color, gemColor, waitTime, useToken));
    return sphere;
  }

  IEnumerator SendSphere(Sphere sphere, Vector3 target, Color color, GemColor gemColor, float waitTime, bool useToken)
  {
    yield return new WaitForSeconds(waitTime);
    target.z = 0;
    target.x += 50;
    target.y += 25;
    sphere.Set(target, color, gemColor, 1, useToken);
  }

  public void SphereArrived(Sphere sphere, GemColor gemColor, int value, bool useToken)
  {
    spheres.Remove(sphere);

    string message = "";
    Color color = Color.white;

    switch (gemColor)
    {
      case GemColor.Blue:
        {
          gameManager.ModifyResource(Resource.Defence, value, useToken);
          color = Color.blue;
          message = "+" + value + " " + Translation.GetTranslation("defence");
          break;
        }
      case GemColor.Gray:
        {
          if (gameManager.powerManager.ActivateBar(useToken))
          {
            message = Translation.GetTranslation("level", StringFormat.capitalized) + " " + DataManager.instance.saveData.powerLevel + "!";
          }
          color = Color.magenta;
          break;
        }
      case GemColor.Green:
        {
          gameManager.ModifyResource(Resource.Energy, value);
          color = Color.green;
          message = "+" + value + " " + Translation.GetTranslation("energy");
          break;
        }
      case GemColor.Red:
        {
          gameManager.ModifyResource(Resource.Attack, value);
          color = Color.red;
          message = "+" + value + " " + Translation.GetTranslation("attack");
          break;
        }
      case GemColor.Yellow:
        {
          gameManager.ModifyResource(Resource.Gold, value, useToken);
          message = "+" + value + " " + Translation.GetTranslation("gold");
          color = Color.yellow;
          break;
        }
      case GemColor.Purple:
        {
          gameManager.ModifyResource(Resource.Potential, value);
          message = "+" + value + " " + Translation.GetTranslation("potential");
          color = Color.magenta;
          break;
        }
      case GemColor.Crimsom:
        {
          gameManager.ModifyResource(Resource.Life, value);
          message = "+" + value + " " + Translation.GetTranslation("life");
          color = Color.red;
          break;
        }
      default:
        {
          break;
        }
    }
    CreateFloatingText(message, color);
  }

  public void SphereArrived(CursedSphere sphere, GemEffect curse, int value){
    curses.Remove(sphere);
    switch(curse){
      case GemEffect.Attack:
      {
        monster.AddAttack(value, true);
        break;
      }
      case GemEffect.Defence:
      {
        monster.AddDefence(value, true);
        break;
      }
    }

  }

  public void CreateFloatingText(string message, Color color){
    FloatingText text = Instantiate(floatingTextPrefab, messageContainer);
    text.Set(message, 32, color, new Vector2(0, 50f), 1f, true, .1f);
  }

  public void Attack(Unit unit)
  {
    Vector3 target;
    string message;
    Color color = Color.red;
    Unit targetUnit = unit == hero ? monster : hero;

    int startingLife = targetUnit.life;
    int damage = -targetUnit.AddLife(-unit.attack);

    if (damage > 0)
    {
      if (targetUnit == hero){
        int decay = DataManager.instance.saveData.defenceDecay;
        hero.defenceToDecay =  decay != 0 ? Mathf.Min(decay, hero.defence): hero.defence;
        gameManager.Trigger(Trigger.DamageTaken, damage);
      } else {
        gameManager.Trigger(Trigger.MonsterDamaged, damage);
      }
    }else{
      if (targetUnit == hero){
        gameManager.Trigger(Trigger.PlayerBlocked, damage);
      }
    }

    message = damage.ToString();
    target = targetUnit.transform.position;
    if (targetUnit.life <= 0)
    {
      if (targetUnit == monster)
      {
        int overkill = damage - startingLife;
        DataManager.instance.saveData.totalOverkill += overkill;
        DataManager.instance.saveData.overkill += overkill;
        gameManager.Trigger(Trigger.MonsterKilled, startingLife);
        gameManager.TestUnlocking(UnlockKey.Overkill, DataManager.instance.saveData.totalOverkill);
      }
      GameOver(targetUnit == monster);
    }
    else
    {
      if (damage == 0)
      {
        color = Color.gray;
      }
    }

    FloatingText text = Instantiate(floatingTextPrefab, messageContainer);
    text.transform.position = Camera.main.WorldToScreenPoint(target);
    text.Set(message, Mathf.Min(72, 38 + damage), color, new Vector2(0, 50f), 1f, true, .1f);
  }

  public void AttackCompleted(Unit unit)
  {
    if (gameManager.gameOver)
    {
      return;
    }

    if (unit == hero)
    {
      monster.ResetDefence();
      StartCoroutine(monster.Attack(true));
    }
    else
    {
      hero.ResetDefence();
      NewRound();
    }
  }

  public void GameOver(bool victory)
  {
    DataManager.instance.saveData.life = hero.life;
    if (victory)
    {
      monster.gameObject.SetActive(false);
      if (DataManager.instance.IsLastBoss() == false)
      {
        hero.StartCoroutine(hero.WalkTo(new Vector3(15, hero.transform.localPosition.y)));
      }
    }
    else
    {
        hero.gameObject.SetActive(false);
    }
    gameManager.GameOver(victory);
  }

}
