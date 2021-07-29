using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomNode : MonoBehaviour
{
  public SpriteRenderer spriteRenderer;
  public SpriteRenderer icon;
  public MapManager mapManager;
  public bool bossRoom;
  public int difficulty;
  public int x;
  public int y;
  public int z;
  public int floor;
  public int gold;
  public int power;
  public int danger;
  public RoomType type = RoomType.Combat;
  public RoomTag roomTag;

  public bool available;

  public List<RoomNode> linkedNodes;
  public List<SpriteRenderer> roads;
  public List<SpriteRenderer> incomingRoads;

  [Header("Audio")]
  public AudioSource audioSource;
  public AudioClip clickedSound;

  public void AddLink(RoomNode node, SpriteRenderer road)
  {
    if (linkedNodes == null)
    {
      linkedNodes = new List<RoomNode>();
      roads = new List<SpriteRenderer>();
    }
    linkedNodes.Add(node);
    roads.Add(road);
  }

  public void AddIncomingRoad(SpriteRenderer road){
    incomingRoads.Add(road);
  }

  public void OnMouseDown()
  {
    if (EventSystem.current.IsPointerOverGameObject() == false)
    {
      if (available)
      {
        mapManager.NodeClicked(this);
        audioSource.clip = clickedSound;
        audioSource.Play();
      }
    }
  }

  public void SetActive(bool active)
  {
    Color color = active ? Color.white : Color.gray;
    spriteRenderer.color = color;
    icon.color = color;
  }

  public void SetRoads(bool active)
  {
    Color color = active ? Color.white : Color.gray;
    foreach (SpriteRenderer road in roads)
    {
      road.color = color;
    }
  }

  public void SetIcon(Sprite sprite)
  {
    icon.sprite = sprite;
  }
}

public enum RoomTag { None, Gold, Power, Elite, Rest, Event }
public enum RoomType { Combat, Rest, Event}

[System.Serializable]
public class RoomTagWeight
{
  public RoomTag tag;
  public int weight;
  public int count;

  public int GetWeight()
  {
    if (count == 0)
    {
      return weight;
    }
    return weight / (count * TagCountWeight(tag));
  }

  private int TagCountWeight(RoomTag tag){
    switch (tag){
      case RoomTag.Rest:
        return 2;
    }
    return 1;
  }
}

[System.Serializable]
public struct RoomVector{
  public int x;
  public int y; 
  public int z;

  public RoomVector(int x, int y, int z){
    this.x = x;
    this.y = y;
    this.z = z;
  }
}