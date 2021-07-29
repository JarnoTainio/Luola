using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemContainer : MonoBehaviour
{
  [Header("References")]
  public GameManager gameManager;
  public CursedSphere cursedSpherePrefab;

  [Header("Container")]
  public int width = 5;
  public int height = 5;
  public int matchN = 3;

  public float waitTime = .75f;
  public float playerWaitTime;

  public float gemChainDelay = 0.25f;
  public float explosionAfterDelay = 0.5f;

  public Gem gemPrefab;
  public GemColor[] colors;

  public Gem[,] gemGrid;

  public bool playerControl;
  public List<Gem> movingGems;
  public List<SlidingGem> slidingGems;

  Gem selectedGem;
  float wait;
  bool nextTurn;
  Gem first, second;

  [Header("Gems")]
  public Sprite[] redGems;
  public Sprite[] blueGems;
  public Sprite[] greenGems;
  public Sprite[] yellowGems;
  public Sprite[] purpleGems;
  public Sprite[] grayGems;

  [Header("Effects")]
  public Sprite damageEffect;
  public Sprite attackEffect;
  public Sprite defenceEffect;
  public Sprite rockGem;

  [Header("Audio")]
  AudioSource audioSource;
  public AudioClip moveGemSound;

  private bool gemsDestroyed;

  void Awake()
  {
    if (gameManager != null)
    {
      audioSource = GetComponent<AudioSource>();
      movingGems = new List<Gem>();
      slidingGems = new List<SlidingGem>();
      playerControl = false;
      potentialGems = new List<Gem>();
      width = DataManager.instance.saveData.gemBoardWidth;
      height = DataManager.instance.saveData.gemBoardHeight;
      matchN = DataManager.instance.saveData.matchRequired;
      gemGrid = new Gem[width, height];
    }
  }

  public void Init(bool isLoaded)
  {
    if (height == 7 || width == 7){
      transform.localScale = new Vector3(0.85f, 0.85f, 1f );
      if (width == 6){
        transform.localPosition += new Vector3(0.5f, 0, 0);
      }
      if (height == 6){
        transform.localPosition += new Vector3(0, 0.5f, 0);
      }
    }
    if (isLoaded)
    {
      LoadBoard(DataManager.instance.saveData.gemBoard);
      nextTurn = true;
      playerControl = true;
    }
    else
    {
      nextTurn = false;
      CreateGems(true);
    }
  }

  private void Update()
  {
    if(wait > 0f)
    {
      wait -= Time.deltaTime;
      if (wait <= 0)
      {
          DoneMoving(null);
      }
    }

    if (playerWaitTime > 0f)
    {
       playerWaitTime -= Time.deltaTime;
    }
  }


    /*=======================================
     *  GEM CREATING
     *=====================================*/

  private int GetColorWeight(GemColor color){
    switch (color){
      case GemColor.Green:
        int e = gameManager.GetResource(Resource.Energy);
        return Mathf.Max(40, 100 - e * 5);
      case GemColor.Gray:
        return gameManager.GetRemainingResource(color) == 0 
        ? (DataManager.instance.saveData.emptyPowerToPotential ? 80 : DataManager.instance.saveData.noPowerGemWeight)
        : 100;
      case GemColor.Yellow:
        return gameManager.GetRemainingResource(color) == 0 
        ? (DataManager.instance.saveData.emptyGoldToPotential ? 80 : DataManager.instance.saveData.noGoldGemWeight)
        : 100;
      case GemColor.Red:
        return DataManager.instance.saveData.redGemWeight;
      case GemColor.Blue:
        return DataManager.instance.saveData.blueGemWeight;
      case GemColor.Purple:
        return DataManager.instance.saveData.purpleGemWeight;
      default:
        return 100;
    }
  }

  private void CreateGems(bool save = false)
  {
    var colorCount = new Dictionary<GemColor, int>();
    foreach(GemColor color in colors)
    {
      int weight = GetColorWeight(color);
      colorCount.Add(color, weight);
    }
    for (int y = height - 1; y >= 0; y--)
    {
      for (int x = 0; x < width; x++)
      {
        if (gemGrid[x, y] != null)
        {
          GemColor color = gemGrid[x, y].Color();
          if (color != GemColor.None){
            colorCount[color]--;
          }
        }
      }
    }

    bool newGems= false;
    float i = 0f;
    for (int y = height - 1; y >= 0; y--)
    {
      for (int x = 0; x < width; x++)
      {
        if (gemGrid[x,y] == null)
        {
          i++;
          newGems = true;
          // Create gem
          Gem gem = Instantiate(gemPrefab, transform);
          gem.gemContainer = this;
          gem.transform.localPosition = new Vector3(x, y - height - i / 2f, 10);

          // Color & position
          List<GemColor> legalColors = GetLegalColors(new Vector2Int(x, y));
          foreach (GemColor color in legalColors)
          {
            if (!colorCount.ContainsKey(color) && color != GemColor.None)
            {
              colorCount.Add(color, 1);
            }
          }

          int totalWeight = 0;
          foreach(GemColor color in legalColors)
          {
            if (colorCount.ContainsKey(color)) {
              totalWeight += colorCount[color];
            }
          }
          int roll = Random.Range(0, totalWeight);
          foreach (GemColor color in legalColors)
          {
            if (colorCount.ContainsKey(color))
            {
              roll -= colorCount[color];
              if (roll <= 0)
              {
                bool quality = Random.Range(0, 100) < DataManager.instance.GetQuality();
                GemItem gemItem = DataManager.instance.GetGem(color, quality);
                gemItem.isReady = false;
                gem.SetColor(gemItem, gameManager.GetRemainingResource(gemItem.color) > 0);
                gem.Move(new Vector2Int(x, y), false);
                break;
              }
            }
          }

          // Add to grid
          gemGrid[x, y] = gem;
          //yield return new WaitForSeconds(.05f);
        }
      }
    }
    if (save)
    {
      StartCoroutine(gameManager.SaveCombat());
    }
    if (!newGems)
    {
      if (nextTurn)
      {
        gameManager.NextTurn(gemsDestroyed);
        nextTurn = false;
      }
      else{
        gameManager.abilityManager.UpdateButtons();
      }
    }
  }

  public void AddEffect(EffectAmount effectAmount)
  {
    // Get gem where to add the effect
    Gem gem = null;
    List<Gem> gems = new List<Gem>();
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        Gem g = gemGrid[x, y];
        if (g != null && g.Type() == GemType.Normal && g.gemItem.effect == GemEffect.None)
        {
          gems.Add(g);
        }
      }
    }
    for (int i = 0; i < effectAmount.amount; i++)
    {
      if (gems.Count > 0)
      {
        Sprite sprite = null;
        gem = gems[Random.Range(0, gems.Count)];
        switch (effectAmount.effect)
        {
          case GemEffect.Damage:
            sprite = damageEffect;
            break;
          case GemEffect.Attack:
            sprite = attackEffect;
            break;
          case GemEffect.Defence:
            sprite = defenceEffect;
            break;
          case GemEffect.Rock:
            sprite = null;
            gem.gemItem = new GemItem(GemColor.None, GemType.Normal);
            gem.UpdateSprite();
            break;
        }
        gem.SetEffect(effectAmount.effect, sprite, effectAmount.value);
        CursedSphere sphere = Instantiate(cursedSpherePrefab, gameManager.combatManager.monster.transform);
        sphere.combatManager = gameManager.combatManager;
        gameManager.combatManager.curses.Add(sphere);
        sphere.SetTarget(gem.transform.position);
        gems.Remove(gem);
      }
    }
  }

  public int GetEffectCount()
  {
    List<Gem> gems = new List<Gem>();
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        Gem g = gemGrid[x, y];
        if (g != null && g.Type() == GemType.Normal && g.gemItem.effect == GemEffect.None)
        {
          gems.Add(g);
        }
      }
    }
    return gems.Count;
  }


  /*=======================================
   *  GEM EVENTS
   *=====================================*/

  public void Moving(Gem gem, Vector2Int direction)
  {
    playerControl = false;
    if (!movingGems.Contains(gem))
    {
      movingGems.Add(gem);
    }
   }

  public void DoneMoving(Gem gem)
  {
    if (gem != null)
    {
      movingGems.Remove(gem);

      // Keep sliding
      SlidingGem sliding = GetSlidingGem(gem);
      if (sliding != null)
      {
        sliding.distanceTraveled++;
        // Explode if there are matches
        List<Gem> destoyedGems = CheckGems(false);

        // Sliding gem was not destroyed
        if (!destoyedGems.Contains(gem))
        {

          // Keep sliding if within bounds
          Vector2Int target = gem.position + sliding.direction;
          if (IsWithinBounds(target))
          {
            SlideGem(sliding, GetGem(target));
            return;
          }
          slidingGems.Remove(sliding);
        }
        else
        {
          slidingGems.Remove(sliding);
          // TODO: Sliding gems gain power based on distance traveled
          //Debug.Log("Traveled "+sliding.distanceTraveled);
        }
      }

       // Fall down
      else
      {
        Vector2Int below = gem.position + new Vector2Int(0, 1);
        if (IsWithinBounds(below) && GetGem(below) == null)
        {
          gem.Move(below);
          return;
        }
      }
    }

    // Reached solid position
    if (movingGems.Count == 0 && slidingGems.Count == 0)
    {
       playerControl = true;

      // Nothing is exploding
      if (CheckGems().Count == 0)
      {
          // Nothing is falling
        if (!Shake())
        {
            // Create gems if necessary
            CreateGems();
        }
        else
        {
           wait = waitTime;
        }
      }
      else
      {
         wait = waitTime;
      }
    }
  }

    public void GemClicked(Gem gem)
    {
      if (gameManager.PlayerControl(false))
        {
          if (gameManager.abilityManager.UseTrigger(AbilityTrigger.DestroyGem)){
            gem.destroyed = true;
            gem.Destroy(0);
            RemoveSlidingGem(gem);
            selectedGem = null;
            Shake();
          }else{
            selectedGem = gem;
          }
        }
        else{
          selectedGem = null;
        }
    }

  public void GemHovering(Gem gem)
  {
    if (playerControl && selectedGem != null     // Null check
        && selectedGem != gem   // Self check
        && (Vector2Int.Distance(gem.position, selectedGem.position) == 1))  // Adjacent check
    {
      // Swap gem positions
      Vector2Int pos = selectedGem.position;
      Vector2Int directionOther = selectedGem.Move(gem.position, false);
      Vector2Int direction = gem.Move(pos, false);

      playerControl = false;
      gemsDestroyed = false;
      playerWaitTime = .25f;

      // Check selected gem
      if (selectedGem.Type() == GemType.Throw || gameManager.abilityManager.UseTrigger(AbilityTrigger.SliderGem))
      {
        SlidingGem gemSlide = GetSlidingGem(selectedGem);
        if (gemSlide == null)
          slidingGems.Add(new SlidingGem(selectedGem, directionOther));
      }

      // Check hovered gem
      if (gem.Type() == GemType.Throw)
      {
        SlidingGem gemSlide = GetSlidingGem(gem);
        if (gemSlide == null)
          slidingGems.Add(new SlidingGem(gem, direction));
      }
      first = selectedGem;
      second = gem;
      selectedGem = null;
      nextTurn = true;
      audioSource.clip = moveGemSound;
      audioSource.Play();
    }
  }

  public void SlideGem(SlidingGem slidingGem, Gem other)
  {
    slidingGem.gem.Move(slidingGem.gem.position + slidingGem.direction, false);
    other.Move(other.position - slidingGem.direction, false);
  }

  public void Remove(Gem gem)
  {
    gemGrid[gem.position.x, gem.position.y] = null;
  }

  /*=======================================
   *  GEM COMBOS
   *=====================================*/

  private bool Shake()
  {
    bool falling = false;
    for(int y = height - 1; y >= 0 ; y--)
    {
      for(int x = 0; x < width; x++)
      {
        Gem gem = GetGem(new Vector2Int(x, y));
        if (gem != null)
        {
          Vector2Int below = gem.position + new Vector2Int(0, 1);
          bool fall = false;

          while(GetGem(below) == null && below.y < height)
          {
            fall = true;
            below.y++;
          }

          if (fall)
          {
            gem.Move(below + new Vector2Int(0, -1));
            falling = true;
          }
        }
      }
    }
    return falling;
  }

  private List<Gem> CheckGems(bool destroyGems = true)
  {
    List<Gem> destroyedGems = GridCheck(destroyGems);
      if (destroyedGems.Count > 0)
      {
          playerControl = false;
      }
      return destroyedGems;
   }

  private List<Gem> GridCheck(bool destroyGems = true)
  {
    GemColor currentColor = GemColor.Red;
    int gemCount = 0;
    Gem[,] grid = new Gem[width, height];
    List<Gem> gemList = new List<Gem>();

    for (int y = height - 1; y >= 0; y--)
    {
      for (int x = 0; x < width; x++)
      {
        CheckPosition(gemList, grid, x, y, ref gemCount, ref currentColor, new Vector2Int(1, 0));
      }
      gemCount = 0;
    }

    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
          CheckPosition(gemList, grid, x, y, ref gemCount, ref currentColor, new Vector2Int(0, 1));
      }
      gemCount = 0;
    }
    gemList.Sort((a, b) =>
    {
      if (a == first)
      {
        return -1;
      }
      else if (b == first)
      {
        return 1;
      }
      else if (b == second)
      {
        return 1;
      }
      else if (a == second)
      {
        return -1;
      }
      return 0;
    });

    if (gemList.Count > 0)
    {
      Vector2Int[] directions = new Vector2Int[4];
      directions[0] = new Vector2Int(1, 0);
      directions[1] = new Vector2Int(-1, 0);
      directions[2] = new Vector2Int(0, 1);
      directions[3] = new Vector2Int(0, -1);

      List<Gem> closedList = new List<Gem>();
      List<Gem> openList = new List<Gem>();
      openList.Add(gemList[0]);

      var i = 0;
      while (openList.Count > 0)
      {
        i += 1;
        Gem current = openList[0];
        openList.Remove(current);
        closedList.Add(current);
        foreach(Vector2Int direction in directions)
        {
          Vector2Int pos = current.position + direction;
          if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
          {
            Gem neighbor = grid[pos.x, pos.y];
            if (neighbor != null && !closedList.Contains(neighbor) && current.IsMatch(neighbor))
            {
                openList.Add(neighbor);
            }
          }
        }
        if (i > 1000)
        {
           break;
        }
      }

      if (closedList.Count > 0)
      {
        if (destroyGems)
        {
          float f = 0f;
          int num = 3 - closedList.Count;
          int resourceCount = gameManager.GetRemainingResource(closedList[0].Color());

          bool gemAsPurple = false;
          if (resourceCount <= 0){
            if (DataManager.instance.saveData.emptyPowerToPotential && closedList[0].Color() == GemColor.Gray){
              resourceCount = 999;
              gemAsPurple = true;
            }
            else if (DataManager.instance.saveData.emptyGoldToPotential && closedList[0].Color() == GemColor.Yellow){
              resourceCount = 999;
              gemAsPurple = true;
            }
          }
          List<Vector2Int> bombs = new List<Vector2Int>();

          // Pure gems cleanse curses
          foreach (Gem g in closedList){
            if (g.Type() == GemType.Pure)
            {
              for (int xx = -1; xx <= 1; xx++)
              {
                for (int yy = -1; yy <= 1; yy++)
                {
                  Gem n = GetGem(new Vector2Int(g.position.x + xx, g.position.y + yy));
                  if (n != null && n.gemItem.effect != GemEffect.None && n.gemItem.effect != GemEffect.Rock)
                  {
                    n.SetEffect(GemEffect.None, null);
                  }
                }
              }
            }
          }

          // Destroy gems in the closed list and create orbs from them
          int orbCount = 0;
          GemColor gemColor = gemAsPurple ? GemColor.Purple : closedList[0].gemItem.color;
          foreach (Gem g in closedList)
          {
 
            // Handle cursed gems
            if (g.gemItem.effect != GemEffect.None)
            {
              gameManager.combatManager.CreateCurse(g);
            }

            // Collect explosions
            if (g.Type() == GemType.Bomb || gameManager.abilityManager.UseTrigger(AbilityTrigger.BombGem))
            {
              bombs.Add(g.position);
            }

            // Create orbs for length of
            if (num++ <= 0)
            {
              if (resourceCount > 0)
              {
                resourceCount--;
                orbCount++;
                gameManager.combatManager.CreateSphere(g, 1, true, gemAsPurple);
              }
            }

            // Special gems give +1/+2 bonus
            if (g.Type() != GemType.Normal)
            {
              GemColor color = g.Color();
              int value = (gemAsPurple == false && (color == GemColor.Gray || color == GemColor.Yellow)) ? 2 : 1;
              for (int j = 0; j < value; j++)
              {
                if (resourceCount > 0)
                {
                  resourceCount--;
                  gameManager.combatManager.CreateSphere(g, 1, true, gemAsPurple);
                  orbCount++;
                }
              }

            }
            gemsDestroyed = true;
            g.destroyed = true;
            g.Destroy(f);
            RemoveSlidingGem(g);
            f += gemChainDelay;
          }

          // Trigger potential
          //resourceCount = gameManager.GetRemainingResource(gemColor);
          if (gameManager.GetResource(Resource.Potential) > 0 && resourceCount > 0){
            bool canUsePotential = gemColor != GemColor.Purple;
            canUsePotential &= (DataManager.instance.saveData.emptyPowerToPotential && gemColor == GemColor.Gray) == false;
            if (canUsePotential){
              orbCount = Mathf.Min(orbCount, resourceCount);
              gameManager.ModifyResource(Resource.Potential, -1);
              StartCoroutine(CreatePotentialSpheres(gemColor, orbCount));
            }
          }

          // Bomb gems
          int explosionRadius = 1;
          foreach(Vector2Int pos in bombs)
          {
            for (int x = -explosionRadius; x <= explosionRadius; x++)
            {
              for (int y = -explosionRadius; y <= explosionRadius; y++)
              {
                int xx = pos.x + x;
                if (xx < 0 || xx >= width)
                  continue;
                int yy = pos.y + y;
                if (yy < 0 || yy >= height)
                  continue;
                Gem gem = gemGrid[xx, yy];
                if (gem != null && !gem.destroyed && !gemList.Contains(gem))
                {
                  gem.destroyed = true;
                  gem.Destroy(0);
                  RemoveSlidingGem(gem);
                }
              }

            }
          }
          return gemList;
        }
      }
    }

    return gemList;
  }

  List<Gem> potentialGems;
  bool sendingPotential = false;
  public IEnumerator CreatePotentialSpheres(GemColor gemColor, int count){
    while(sendingPotential){ yield return null; }

    for(int i = 0; i < count; i++){
      gameManager.combatManager.CreatePotentialSphere(gemColor);
      yield return new WaitForSeconds(0.5f);
    }
    yield return new WaitForSeconds(0.5f);
    sendingPotential = false;
  }

    private void CheckPosition(List<Gem> gemList, Gem[,] grid, int x, int y, ref int gemCount, ref GemColor currentColor, Vector2Int direction)
    {
      Gem current = gemGrid[x, y];
      if (current == null)
      {
          gemCount = 0;
      }
      else
      {

        // Same color
        if (gemCount == 0 || current.IsMatch(currentColor))
        {
          gemCount += 1;
          // Three found, add them to explosionGrid
          if (gemCount == matchN)
          {
            // Go backwards and add gems to triggerGrid
            for (int i = 0; i < gemCount; i++)
            {
              var yy = y - i * direction.y;
              var xx = x - i * direction.x;

              if (grid[xx, yy] != gemGrid[xx, yy])
              {
                grid[xx, yy] = gemGrid[xx, yy];
                gemList.Add(grid[xx, yy]);
              }
            }
          }
          // Over three, just add
          else if (gemCount > matchN)
          {
            grid[x, y] = current;
            gemList.Add(current);
          }
        }
        // Same color or first
        else
        {
          gemCount = 1;
        }
        currentColor = current.Color();
      }
    }

    private Gem GetGem(Vector2Int pos)
    {
        if (IsWithinBounds(pos))
        {
            return gemGrid[pos.x, pos.y];
        }
        return null;
    }

  private SlidingGem GetSlidingGem(Gem gem)
  {
    foreach (SlidingGem slider in slidingGems)
    {
      if (slider.gem == gem)
      {
        return slider;
      }
    }
    return null;
  }

  private void RemoveSlidingGem(Gem gem)
  {
    SlidingGem sliding = GetSlidingGem(gem);
    if (sliding != null)
      slidingGems.Remove(sliding);
  }

    private bool IsWithinBounds(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= width || pos.y >= height)
        {
            return false;
        }
        return true;
    }

    private List<GemColor> GetLegalColors(Vector2Int point)
    {
        List<GemColor> legalColors = new List<GemColor>(colors);

        Gem left = GetGem(point - new Vector2Int(1, 0));
        Gem left2 = GetGem(point - new Vector2Int(2, 0));
        if (left != null && left2 != null && left.IsMatch(left2))
        {
            if (legalColors.Contains(left.Color()))
            {
                legalColors.Remove(left.Color());
            }
        }

        Gem right = GetGem(point + new Vector2Int(1, 0));
        Gem right2 = GetGem(point + new Vector2Int(2, 0));
        if (right != null && right2 != null && right.IsMatch(right2))
        {
            if (legalColors.Contains(right.Color()))
            {
                legalColors.Remove(right.Color());
            }
        }

        if (right != null && left != null && right.IsMatch(left))
        {
            if (legalColors.Contains(right.Color()))
            {
                legalColors.Remove(right.Color());
            }
        }

        Gem down = GetGem(point - new Vector2Int(0, 1));
        Gem down2 = GetGem(point - new Vector2Int(0, 2));
        if (down != null && down2 != null && down.IsMatch(down2))
        {
            if (legalColors.Contains(down.Color()))
            {
                legalColors.Remove(down.Color());
            }
        }

        Gem up = GetGem(point + new Vector2Int(0, 1));
        Gem up2 = GetGem(point + new Vector2Int(0, 2));
        if (up != null && up2 != null && up.IsMatch(up2))
        {
            if (legalColors.Contains(up.Color()))
            {
                legalColors.Remove(up.Color());
            }
        }

        if (up != null && down != null && up.IsMatch(down))
        {
            if (legalColors.Contains(up.Color()))
            {
                legalColors.Remove(up.Color());
            }
        }

        return legalColors;
    }

    public void SetGemsUsable(bool isUsable)
    {
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gemGrid[x,y] != null)
                {
                    gemGrid[x, y].SetUsable(isUsable);
                }
            }
        }
    }

  public IEnumerator ShuffleBoard()
  {
    playerControl = false;
    nextTurn = false;

    for (int y = height -1; y >= 0; y--)
    {
      for (int x = 0; x < width; x++)
      {
        Gem gem = gemGrid[x, y];
        if (gem != null && gem.gemItem.effect == GemEffect.None)
        {
          gem.Destroy(0, false);
          RemoveSlidingGem(gem);
          yield return new WaitForSeconds(0.025f);
        }
      }
    }
    if (Shake())
    {
      yield return new WaitForSeconds(0.025f);
    }
    CreateGems(true);
  }

  public GemItem[] GetGems()
  {
    List<GemItem> gems = new List<GemItem>();
    for (int y = 0; y < height; y++)
      {
      for (int x = 0; x < width; x++)
      {
        gems.Add(gemGrid[x, y].gemItem);
      }
    }
    return gems.ToArray();
  }

  public void LoadBoard(GemItem[] gems){
    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
        {
          Gem gem = Instantiate(gemPrefab, transform);
          gem.gemContainer = this;
          gem.transform.localPosition = new Vector3(x, y, 10);
          GemItem gemItem = gems[x + y * width];
          gem.SetEffect(gemItem.effect, GetEffectSprite(gemItem.effect), gemItem.effectValue);
          gem.gemItem = gemItem;
          gem.SetColor(gemItem, gameManager.GetRemainingResource(gemItem.color) > 0);
          gem.position = new Vector2Int(x, y);
          gemGrid[x, y] = gem;
        }
    }
  }

  public void RemoveEffects(int count)
  {
    List<Gem> gems = new List<Gem>();
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        Gem gem = GetGem(new Vector2Int(x, y));
        if (gem != null && gem.gemItem.effect != GemEffect.None)
        {
          gems.Add(gem);
        }
      }
    }
    while (gems.Count > 0 && count > 0)
    {
      count--;
      Gem g = gems[Random.Range(0, gems.Count)];
      gems.Remove(g);
      g.SetEffect(GemEffect.None, null);
    }
  }

  public Sprite GetEffectSprite(GemEffect effect)
  {
    switch (effect)
    {
      case GemEffect.Attack:
          return attackEffect;
      case GemEffect.Defence:
        return defenceEffect;
      case GemEffect.Damage:
        return damageEffect;
      default:
        return null;
    }
  }
}
