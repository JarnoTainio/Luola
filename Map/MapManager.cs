using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
  [Header("References")]
  public RoomManager roomManager;
  public SpriteRenderer playerSprite;
  public Image lifeImage;
  public TextMeshProUGUI lifeText;
  public GameObject goldObject;
  public TextMeshProUGUI goldText;
  public TextMeshProUGUI powerText;
  public TextMeshProUGUI fadingFloorText;
  public Image powerImage;
  public FadeIn fadePanel;
  public TextMeshProUGUI seedText;
  public TextObject floorText;
  public Announcement announcement;
  public Camera camera;

  [Header("Settings")]
  public float playerMoveSpeed = 2.5f;
  public int currentFloor = 0;
  public int floorHeight = 8;
  public float roomDistance = 1.5f;
  public float yOffset = -4f;
  public float xOffset = 0f;

  [Header("Sprites")]
  public Sprite visitedPath;
  public Sprite visited;
  public Sprite gold;
  public Sprite power;
  public Sprite boss;
  public Sprite elite;
  public Sprite rest;
  public Sprite choice;

  [Header("Event weights")]
  public List<RoomTagWeight> roomWeights;

  [Header("Prefabs")]
  public RoomNode roomPrefab;
  public Transform pathPrefab;

  private bool playerControl;
  private List<RoomNode> roomList;

  private const int NORMAL = 0;
  private const int ELITE = 3;

  public AudioSource audioSource;

  private void Awake()
  {
    SaveData saveData = DataManager.instance.saveData;
    if (saveData.disabledGameElements.Contains(GameElement.PowerTokens)){
      roomWeights.Find(r => r.tag == RoomTag.Power).weight = 0;
    }
    if (saveData.disabledGameElements.Contains(GameElement.GoldTokens)){
      roomWeights.Find(r => r.tag == RoomTag.Gold).weight = 0;
    }
    if (saveData.disabledGameElements.Contains(GameElement.DamageTaken)){
      roomWeights.Find(r => r.tag == RoomTag.Rest).weight = 0;
    }

    if (DataManager.instance.saveData.roomY == -1){
      fadePanel.wait = 2f;
      fadingFloorText.text = Translation.GetTranslation("currentFloor", new Dictionary<string, string>{ { "floor", (DataManager.instance.saveData.floor + 1).ToString() } });
    }
    else{
      fadePanel.wait = 0f;
      fadingFloorText.text = "";
    }
    floorHeight = DataManager.instance.saveData.floorHeight;
    if (floorHeight == 10){
      camera.transform.position += new Vector3(0, 1.5f, 0);
      camera.orthographicSize = 6.5f;

    } else if (floorHeight == 12){
      camera.transform.position += new Vector3(0, 2.6f, 0);
      camera.orthographicSize = 7.25f;
    }
    fadePanel.StartFadeIn();
    floorText.SetText("currentFloor", new Dictionary<string, string>{ { "floor", (DataManager.instance.saveData.floor + 1).ToString() } });
  }
  void Start()
  {
    seedText.text = "seed: " + DataManager.instance.saveData.seed;
    currentFloor = DataManager.instance.saveData.floor;
    Random.InitState(DataManager.instance.saveData.seed * ( 1 + currentFloor));
    roomList = new List<RoomNode>();
    SaveData saveData = DataManager.instance.saveData;

    List<Vector3Int> roomPositions;
    roomPositions = new List<Vector3Int>();
    int lastRoll = 999;
    for (int y = 0; y < floorHeight; y++)
    {
      int pos = y == 0 ? 0 : (y + 1) % floorHeight;
      int z = y % (floorHeight / 2) == 0 ? ELITE : NORMAL;
      if (pos == 0)
      { // Boss room
        roomPositions.Add(new Vector3Int(0, y, z));
      }
      else if (pos == 2 || pos == floorHeight - 1)
      { // After start or before boss
        roomPositions.Add(new Vector3Int(-1, y, z));
        roomPositions.Add(new Vector3Int(1, y, z));
      }
      else
      {
        int roll = Random.Range(0, 3);
        while (roll == lastRoll)
        {
          roll = Random.Range(0, 3);
        }
        lastRoll = roll;

        if (roll == 0)
        {
          roomPositions.Add(new Vector3Int(-1, y, z));
          roomPositions.Add(new Vector3Int(0, y, z));
          roomPositions.Add(new Vector3Int(1, y, z));
        }
        else if (roll == 1)
        {
          roomPositions.Add(new Vector3Int(-1, y, z));
          roomPositions.Add(new Vector3Int(1, y, z));
        }
        else
        {
          roomPositions.Add(new Vector3Int(-2, y, z));
          roomPositions.Add(new Vector3Int(-1, y, z));
          roomPositions.Add(new Vector3Int(1, y, z));
          roomPositions.Add(new Vector3Int(2, y, z));
        }
      }
    }

    foreach (Vector3Int pos in roomPositions)
    {
      RoomNode room = Instantiate(roomPrefab, transform);
      SetRoomValues(room, pos.x, pos.y, pos.z);
      room.transform.localPosition = new Vector2(pos.x * roomDistance + xOffset, pos.y * roomDistance + yOffset);
      room.mapManager = this;
      roomList.Add(room);

      foreach (RoomNode r in roomList)
      {
        if (r.y == room.y - 1 && Mathf.Abs(r.x - room.x) < 2)
        {
          Transform path = Instantiate(pathPrefab, transform);
          r.AddLink(room, path.GetComponent<SpriteRenderer>());
          room.AddIncomingRoad(path.GetComponent<SpriteRenderer>());
          Vector2 mid = room.transform.localPosition + (r.transform.localPosition - room.transform.localPosition) / 2;
          path.localPosition = new Vector3(mid.x, mid.y, path.localPosition.z);
          /*
          if (saveData.rooms.Contains(new Vector3Int(r.x, r.y, r.z))
            && saveData.rooms.Contains(new Vector3Int(room.x, room.y, room.z))){
            path.GetComponent<SpriteRenderer>().sprite = visitedPath;
          }
          */

          float offset = 90f;
          Vector3 direction = room.transform.localPosition - r.transform.localPosition;
          direction.Normalize();
          float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
          Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
          path.Rotate(rotation.eulerAngles);
        }
      }
    }

    RoomNode currentNode = null;
    if (DataManager.instance.saveData.roomX == -1 && DataManager.instance.saveData.roomY == -1)
    {
      currentNode = roomList[0];
    }
    else
    {
      int x = DataManager.instance.saveData.roomX;
      int y = DataManager.instance.saveData.roomY;
      currentNode = roomList.Find((r) => r.x == x && r.y == y);
    }
    SetCurrentRoom(currentNode);

    if (DataManager.instance.loading == false)
    {
      DataManager.instance.saveData.SaveMap(this);
    }
    else
    {
      DataManager.instance.saveData.Load(null);
      string eventName = DataManager.instance.saveData.eventName;
      if (eventName != ""){
        roomManager.SetEvent(eventName);
      }
      DataManager.instance.loading = false;
    }

    ModifyResource(new ResourceAmount { resource = Resource.Life, amount = 0});
    ModifyResource(new ResourceAmount { resource = Resource.Power, amount = 0 });
    ModifyResource(new ResourceAmount { resource = Resource.Gold, amount = 0 });
  }

  public void NodeClicked(RoomNode room)
  {
    if (playerControl == false || roomManager.gameObject.activeSelf)
      return;
    SaveData save = DataManager.instance.saveData;
    save.roomX = room.x;
    save.roomY = room.y;
    save.roomDifficulty = room.difficulty;
    save.roomGold = Mathf.Max(0, room.gold + save.roomExtraGold);
    save.roomPower = Mathf.Max(room.power + save.roomExtraPower);
    save.bossRoom = room.bossRoom;
    save.roomDanger = room.danger;
    save.room += 1;
    save.rooms.Add(new RoomVector(room.x, room.y, currentFloor));
    StartCoroutine(MoveToNode(room));
  }
  public void SetCurrentRoom(RoomNode currentRoom)
  {
    foreach (RoomNode room in roomList)
    {
      if (room == currentRoom)
      {
        room.SetActive(true);
        room.SetRoads(true);
        room.available = false;
        playerSprite.transform.localPosition = room.transform.localPosition;
      }
      else if (currentRoom.linkedNodes.Contains(room))
      {
        room.SetActive(true);
        room.SetRoads(false);
        room.available = true;
      }
      else
      {
        room.SetActive(false);
        room.SetRoads(false);
        room.available = false;
      }
    }
    currentRoom.SetIcon(null);
    playerControl = true;

    // Update visited paths and rooms
    SaveData saveData = DataManager.instance.saveData;
    List<RoomNode> visitedRooms = roomList.FindAll(r => saveData.rooms.Contains(new RoomVector(r.x, r.y, currentFloor)));
    foreach(RoomNode rn in visitedRooms){
      rn.SetIcon(visited);
      foreach(RoomNode rn2 in visitedRooms){
        if (rn != rn2){
          foreach(SpriteRenderer sr1 in rn.roads){
            foreach(SpriteRenderer sr2 in rn2.incomingRoads){
              if (sr1 == sr2){
                sr1.sprite = visitedPath;
              }
            }
          }
        }
      }
    }
  }

  public IEnumerator MoveToNode(RoomNode room)
  {
    while (Vector2.Distance(playerSprite.transform.localPosition, room.transform.localPosition) > .1f)
    {
      playerSprite.transform.localPosition = Vector2.MoveTowards(playerSprite.transform.localPosition, room.transform.localPosition, Time.deltaTime * playerMoveSpeed);
      yield return null;
    }
    DataManager.instance.saveData.roomTag = room.roomTag;
    DataManager.instance.saveData.room++;
    switch (room.type)
    {
      case RoomType.Combat:
        SetCurrentRoom(room);
        fadePanel.StartFadeOut(DataManager.instance.StartCombat);
        break;

      case RoomType.Rest:
        SetCurrentRoom(room);
        roomManager.StartRestEvent();
        break;

      case RoomType.Event:
        SetCurrentRoom(room);
        roomManager.StartEvent();
        break;
    }
  }

  public void SetRoomValues(RoomNode room, int x, int y, int z)
  {
    room.audioSource = audioSource;
    
    SaveData saveData = DataManager.instance.saveData;
    room.x = x;
    room.y = y;
    room.floor = currentFloor;

    int floorPart = y > (floorHeight / 2) ? 1 : 0;
    room.difficulty = floorPart + (currentFloor * 2);
    room.z = z;
    room.gold = currentFloor + 3 + floorPart ;
    room.power = currentFloor + 2;
    room.bossRoom = y == floorHeight - 1;

    if (room.bossRoom)
    {
      room.power += currentFloor * 2;
      room.gold += currentFloor * 2;
      room.SetIcon(boss);
      return;
    }
    RoomTagWeight tag = null;
    if (room.z == 0)
    {
      int range = 0;
      foreach (RoomTagWeight rt in roomWeights)
      {
        range += rt.GetWeight();
      }

      int roll = Random.Range(0, range);
      foreach (RoomTagWeight rt in roomWeights)
      {
        roll -= rt.GetWeight();
        if (roll < 0)
        {
          tag = rt;
          tag.count += 1;
          break;
        }
      }
    }
    else
    {
      tag = roomWeights.Find(rt => rt.tag == RoomTag.Elite);
    }

    if (saveData.rooms.Contains(new RoomVector(x, y, currentFloor)))
    {
      room.SetIcon(visited);
      return;
    }
    room.roomTag = tag.tag;

    switch (tag.tag)
    {
      case RoomTag.Rest:
        room.type = RoomType.Rest;
        room.SetIcon(rest);
        break;

      case RoomTag.Gold:
        room.type = RoomType.Combat;
        room.gold += 3 + currentFloor * 2;
        room.danger = 1;
        room.SetIcon(gold);
        break;

      case RoomTag.Power:
        room.type = RoomType.Combat;
        room.power += 2 + currentFloor;
        room.danger = 1;
        room.SetIcon(power);
        break;

      case RoomTag.Event:
        room.type = RoomType.Event;
        room.SetIcon(choice);
        break;

      case RoomTag.Elite:
        room.type = RoomType.Combat;
        room.difficulty = (currentFloor * 2) + 2;
        room.gold += 3 + currentFloor * 2;
        room.power += 2 + currentFloor;
        room.SetIcon(elite);
        break;
    }
  }

  public void ModifyResource(ResourceAmount resource, bool skipModifiers = false)
  {
    SaveData saveData = DataManager.instance.saveData;
    if (skipModifiers == false){
      resource.amount = DataManager.instance.GetAmount(resource);
    }
    switch (resource.resource)
    {
      case Resource.Gold:
        int gold = Mathf.Max(saveData.gold + resource.amount, 0);
        saveData.gold = gold;
        TestUnlocking(UnlockKey.Gold, gold);
        goldText.text = gold.ToString();
        goldObject.SetActive(gold > 0);
        break;

      case Resource.Life:
        if (resource.amount < 0){
          saveData.damageTaken -= resource.amount;
        }
        int life = Mathf.Clamp(saveData.life + resource.amount, 0, saveData.maxLife);
        saveData.life = life;
        lifeText.text = life + " / " + saveData.maxLife;
        lifeImage.fillAmount = (float) saveData.life / saveData.maxLife;
        break;

      case Resource.MaxLife:
        saveData.maxLife = Mathf.Max(saveData.maxLife + resource.amount, 1);
        if (resource.amount > 0){
            if (saveData.maxLife > 999){
              saveData.maxLife = 999;
            }
            saveData.life += resource.amount;
        }else{
          saveData.life = Mathf.Min(saveData.life, saveData.maxLife);
        }
        lifeText.text = saveData.life + " / " + saveData.maxLife;
        lifeImage.fillAmount = (float)saveData.life / saveData.maxLife;
        TestUnlocking(UnlockKey.MaxLife, saveData.maxLife);
        break;

      case Resource.Power:
        int power = Mathf.Max(saveData.power + resource.amount, 0);
        int requiredPower = saveData.GetRequiredPower();
        while(power >= requiredPower)
        {
          power -= requiredPower;
          saveData.power = power;
          saveData.powerLevel++;
          requiredPower = saveData.GetRequiredPower();
          if (saveData.lifeGrowthFromPower){
            ModifyResource(new ResourceAmount(){resource = Resource.MaxLife, amount = 2});
          }
        }
        TestUnlocking(UnlockKey.PowerLevel, saveData.powerLevel);
        powerText.text = saveData.powerLevel.ToString();
        powerImage.fillAmount = ((float)power) / requiredPower;
        break;

      case Resource.RoomGold:
        saveData.roomExtraGold += resource.amount;
        break;

      case Resource.RoomPower:
        saveData.roomExtraPower += resource.amount;
        break;

      case Resource.Aggro:
        saveData.aggroModifier += resource.amount;
        break;

      case Resource.noGoldGemWeight:
        saveData.noGoldGemWeight = resource.amount;
        break;

      case Resource.noPowerGemWeight:
        saveData.noPowerGemWeight = resource.amount;
        break;

      case Resource.redGemWeight:
        saveData.redGemWeight += resource.amount;
        break;

      case Resource.blueGemWeight:
        saveData.blueGemWeight += resource.amount;
        break;
    }
  }

  public void TestUnlocking(UnlockKey target, int value){
    List<int> unlocks = DataManager.instance.TestHeroUnlocking(target, value);
    foreach(int i in unlocks){
      announcement.ShowHeroUnlock(i);
    }
  }
}
