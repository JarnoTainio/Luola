using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
  [Header("Attribute texts")]
  public AttributeText lifeText;
  public AttributeText attackText;
  public AttributeText defenceText;
  public AttributeText energyText;
  public AttributeText potentialText;
  public LifeBar lifeBar;

  public GameObject lifeGO;
  public GameObject attackGO;
  public GameObject defenceGO;
  public GameObject energyGO;
  public GameObject potentialGO;

  [Header("Values")]
  public int maxLife;
  public int life;
  public int attack;
  public int defence;
  public int energy;
  public int potential;
  public int defenceToDecay = -1;

  [Header("Other")]
  public CombatManager combatManager;
  public float attackSpeed = 5f;
  public float hurtTime = .3f;
  public float redTime = .1f;
  public float walkSpeed = 10f;
  SpriteRenderer spriteRenderer;

  [Header("Audio")]
  public AudioSource audioSource;
  public AudioClip damagedSound;
  public AudioClip attackSound;
  public AudioClip blockingSound;
  public AudioClip energyShieldBlockSound;
  public AudioClip cursedAttackSound;
  public AudioClip cursedDefencekSound;
  public AudioClip healingSound;

  private bool isPlayer = true;
  public float flySpeed = .6f;
  public bool moving = false;

  HeroAsset hero;

  public void InitHero(HeroAsset hero)
  {
    this.hero = hero;
    spriteRenderer = GetComponent<SpriteRenderer>();
    AddAttack(0);
    AddDefence(0);
    AddEnergy(0);
    AddLife(0);
    defenceToDecay = -1;

    transform.localScale += hero.scale;
    transform.localPosition += hero.position;
    if (hero.flying){
      StartCoroutine(Fly(transform.localPosition));
    }
    if (hero.ghost){
      StartCoroutine(FadeIn());
    }
    attackSpeed *= hero.speedModifier;
    walkSpeed *= hero.speedModifier;
  }

  public IEnumerator FadeIn(){
    float f = 0f;
    Color c = spriteRenderer.color;
    while(f < 1f){
      f += Time.deltaTime * 0.3f;
      c = spriteRenderer.color;
      spriteRenderer.color = new Color(c.r, c.g, c.g, f);
      yield return null;
    }
    spriteRenderer.color = new Color(c.r, c.g, c.g, 1);
  }

  public IEnumerator Fly(Vector3 basePosition){
    float f = 0;
    while (f < 0.5f){
      if (moving == false){
        f += Time.deltaTime * flySpeed;
        transform.position = basePosition + new Vector3(0, f, 0);
      }
      yield return null;
    }
    f = 0.5f;
    while (f > 0f){
      if (moving == false){
        f -= Time.deltaTime * flySpeed;
        transform.position = basePosition + new Vector3(0, f, 0);
      }
      yield return null;
    }
    f = 0f;
    transform.position = basePosition;
    StartCoroutine(Fly(basePosition));
  }

  public void SetMonster(Monster monster, bool loading)
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    isPlayer = false;
    spriteRenderer.sprite = monster.sprite;
    transform.localScale = new Vector3(monster.scale, monster.scale, 1f);
    transform.localPosition += new Vector3(monster.position.x, monster.position.y, 0f);
    //if (monster.type == MonsterType.boss)
    //{
      //transform.localScale = new Vector3(2f, 2f, 1f);
      //transform.localPosition += new Vector3(-0.5f, 0.25f, 0);
    //}
    if (loading)
    {
      SaveData data = DataManager.instance.saveData;
      maxLife = data.monsterMaxLife;
      life = data.monsterLife;
      name = data.monsterName;
      AddAttack(data.monsterAttack);
      AddDefence(data.monsterDefence);

      int aggression = monster.GetAggresion(data.roundNumber);
      if (aggression > 0 && combatManager.monster.energy != aggression){
        energy = (aggression - combatManager.monster.energy);
      }
    }
    else
    {
      maxLife = life = monster.maxLife;
      AddAttack(0);
      AddDefence(0);
      AddAction(combatManager.monsterTemplate.GetAction(combatManager.gameManager.roundNumber));
      combatManager.gameManager.coinContainer.AddTokens(monster.roomGold);
      combatManager.gameManager.powerContainer.AddTokens(monster.roomPower);
      monster.aggressionTimer += combatManager.hero.hero.floorAggression[DataManager.instance.saveData.floor];
    }
    AddEnergy(0);
    AddLife(0);
  }

  public int AddLife(int amount, bool ignoreDefence = false)
  {
    if (amount == 0)
    {
      lifeGO.SetActive(life > 0);
      lifeBar.SetLifeRatio(life / (float)maxLife);
      lifeText.Set(life.ToString(), false);
      return 0;
    }

    // Damage
    if (amount < 0)
    {
      //Block
      if (ignoreDefence == false && defence > 0)
      {
        // Full block
        if (defence + amount >= 0)
        {
          AddDefence(amount);
          audioSource.PlayOneShot(blockingSound);
          return 0;
        }
        // Partial block
        amount += defence;
        AddDefence(-defence);
      }

      // Energy shield
      if (isPlayer && DataManager.instance.saveData.energyShield && amount < 0){
        int energy = combatManager.gameManager.GetResource(Resource.Energy, true);
        if (energy + amount <= 0){
          combatManager.gameManager.ModifyResource(Resource.Energy, -energy);
          amount += energy;
        }else{
          combatManager.gameManager.ModifyResource(Resource.Energy, amount);
          amount = 0;
          audioSource.PlayOneShot(energyShieldBlockSound);
          StartCoroutine(Hurt(false, false));
          return 0;
        }
      }
    }

    life += amount;
    life = Mathf.Clamp(life, 0, maxLife);

    if (isPlayer && amount < 0){
      DataManager.instance.saveData.damageTaken -= amount;
    }

    lifeBar.SetLifeRatio(life / (float)maxLife);
    lifeText.Set(life.ToString(), true);
    lifeGO.SetActive(life > 0);
    if (amount < 0)
    {
      StartCoroutine(Hurt(life <= 0, true));
      return amount;
    }
    else
    {
      audioSource.clip = healingSound;
      audioSource.Play();
    }
    return 0;
  }

  public void AddAction(MonsterAction action)
  {
    foreach(ResourceAmount act in action.resources)
    {
      switch (act.resource)
      {
        case Resource.Attack:
          AddAttack(act.amount);
          break;
        case Resource.Defence:
          AddDefence(act.amount);
          break;
      }
    }
    foreach(EffectAmount eff in action.effects)
    {
      combatManager.gameManager.gemContainer.AddEffect(eff);
    }
  }

  public void AddAttack(int amount, bool cursed = false)
  {
    if (cursed && amount != 0){
      audioSource.clip = cursedAttackSound;
      audioSource.Play();
    }
    attack += amount;
    attack = Mathf.Clamp(attack, 0, 99);
    attackGO.gameObject.SetActive(attack != 0);
    if (amount != 0)
    {
        attackText.Set(attack.ToString(), amount > 0);
    }
  }

  public void AddDefence(int amount, bool cursed = false)
  {
    if (cursed && amount != 0){
      audioSource.clip = cursedDefencekSound;
      audioSource.Play();
    }
    defence += amount;
    defence = Mathf.Clamp(defence, 0, 99);
    defenceGO.gameObject.SetActive(defence != 0);
    if (amount != 0)
    {
      defenceText.Set(defence.ToString(), amount > 0);
    }
  }

  public void AddEnergy(int amount)
  {
    if (energyText == null)
    {
        return;
    }
    energy += amount;
    if (isPlayer && DataManager.instance.saveData.bloodEnergy && energy < 0){
      AddLife(energy, true);
      energy = 0;
    }
    energy = Mathf.Clamp(energy, 0, 99);
    energyGO.gameObject.SetActive(energy > 0);

    string text = energy > 0 ? energy.ToString() : "0";
    energyText.Set(text, amount > 0);
  }

  public void AddMaxLife(int amount)
  {
    maxLife += amount;
    if (maxLife < 1)
    {
      maxLife = 1;
    }
    if (life > maxLife)
    {
      life = maxLife;
      lifeText.Set(life.ToString(), false);
    }
    else if (life < maxLife)
    {
      AddLife(amount);
    }

    //string text = energy > 0 ? energy.ToString() : "0";
    //energyText.Set(text, amount > 0);
  }

    public void AddPotential(int amount)
  {
    if (potentialText == null)
    {
        return;
    }
    potential += amount;

    potential = Mathf.Clamp(potential, 0, 99);
    potentialGO.gameObject.SetActive(potential > 0);

    string text = potential > 0 ? potential.ToString() : "0";
    potentialText.Set(text, amount > 0);
  }

  public IEnumerator Attack(bool reversedDirection = false)
  {
    if (combatManager.gameManager.triggerQueue.IsReady() == false)
    {
      while (combatManager.gameManager.triggerQueue.IsReady() == false)
      {
        yield return null;
      }
      yield return new WaitForSeconds(0.5f);
    }

    if (attack > 0)
      {
        Vector3 defaultPosition = transform.localPosition;
        Vector3 target = defaultPosition + new Vector3(1, 0) * (reversedDirection ? -1 : 1);
        moving = true;
        while (Vector3.Distance(transform.localPosition, target) > 0.05f)
        {
          transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, Time.deltaTime * attackSpeed);
          yield return null;
        }

        audioSource.PlayOneShot(attackSound);
        combatManager.Attack(this);
        if (isPlayer && DataManager.instance.saveData.attackDecay != 0){
          attack = Mathf.Max(0, attack - DataManager.instance.saveData.attackDecay);
        }
        else{
          attack = 0;
        }
        attackText.Set(attack.ToString());
        attackGO.gameObject.SetActive(attack != 0);

        while (Vector3.Distance(transform.localPosition, defaultPosition) > 0.05f)
        {
          transform.localPosition = Vector3.MoveTowards(transform.localPosition, defaultPosition, Time.deltaTime * attackSpeed);
          yield return null;
        }
        moving = false;
        yield return new WaitForSeconds(1f);
      }

      combatManager.AttackCompleted(this);
    }

    public IEnumerator WalkTo(Vector3 target, float waitTime = 1f)
    {
        yield return new WaitForSeconds(waitTime);
        if (hero.ghost){
          float f = 1f;
          while(f > 0f){
            f -= Time.deltaTime * 0.75f;
            Color c = spriteRenderer.color;
            spriteRenderer.color = new Color(c.r, c.g, c.g, f);
            yield return null;
          }
        }else{
          moving = true;
          while (Vector3.Distance(transform.localPosition, target) > 0.05f)
          {
              transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, Time.deltaTime * walkSpeed);
              yield return null;
          }
        }
    }

    public IEnumerator Hurt(bool die, bool blood)
    {
      float duration = hurtTime;
      float time = 0;
      //audioSource.PlayOneShot(damagedSound);

      if (die == false)
      {
        while (time < duration)
        {
          time += Time.deltaTime;
          float f = time / duration;
          spriteRenderer.color = GetHurtColor(f, blood);
          yield return null;
        }

        yield return new WaitForSeconds(redTime);

        time = duration;
        while (time > 0f)
        {
          time -= Time.deltaTime;
          float f = time / duration;
          spriteRenderer.color = GetHurtColor(f, blood);
          yield return null;
        }

        spriteRenderer.color = Color.white;

      } else {
        lifeGO.gameObject.SetActive(false);
        attackGO.gameObject.SetActive(false);
        defenceGO.gameObject.SetActive(false);

        if (energyGO != null){
          energyGO.gameObject.SetActive(false);
          while (time < duration)
          {
            time += Time.deltaTime;
            float f = time / duration;

            spriteRenderer.color = new Color(1f, 1f - f, 1f - f, 1f - f);
            yield return null;
          }
        }
      }
  }

  private Color GetHurtColor(float f, bool blood){
    if (blood){
      return new Color(1f, 1f - f, 1f - f);
    }else{ // Energy shield
      return new Color(1f - f, 1f - f, 1f);
    }
  }

  public void ResetDefence()
  {
    int reduceAmount = -defence;
    if (isPlayer)
    {
      // Personal defence decay is set to N
      if (defenceToDecay >= 0){
        reduceAmount = defenceToDecay;
      }
      // Defence decay is limited to N
      else if (DataManager.instance.saveData.defenceDecay != 0)
      {
        reduceAmount = DataManager.instance.saveData.defenceDecay;
      }
    }
    AddDefence(reduceAmount);
    defenceToDecay = -1;
  }

}
