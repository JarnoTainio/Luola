using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Action", menuName = "GemRPG/Monster/Actions/Action")]
public class MonsterAction : ScriptableObject
{
  public ResourceAmount[] resources;
  public EffectAmount[] effects; 
  public int weight;

  public void Boost(int boost)
  {
    int total = 0;
    foreach (ResourceAmount ra in resources)
    {
      total += ra.weight;
    }
    foreach(EffectAmount ea in effects)
    {
      total += ea.weight;
    }

    while (boost-- > 0)
    {
      int roll = Random.Range(0, total);
      for (int i = 0; i < resources.Length; i++)
      {
        if (roll < 0) { break; }
        ResourceAmount ra = resources[i];
        roll -= ra.weight;
        if (roll < 0)
        {
          ra.amount++;
          resources[i] = ra;
          break;
        }
      }
      for (int i = 0; i < effects.Length; i++)
      {
        if (roll < 0) { break; }
        EffectAmount ea = effects[i];
        roll -= ea.weight;
        if (roll < 0)
        {
          ea.amount++;
          effects[i] = ea;
          break;
        }
      }
    }
  }

  public override string ToString()
  {
    string str = "";
    foreach (ResourceAmount re in resources)
    {
      str += re.ToString() + "\n";
    }
    return str;
  }
}

[System.Serializable]
public struct ResourceAmount
{
  public Resource resource;
  public int amount;
  public int weight;
  public bool usesTokens;

  public bool Required(int current)
  {
    switch (resource)
    {
      case Resource.FullLife:
        return current == amount;

      default:
        return current >= amount;
    }
  }

  public override string ToString(){
    return resource.ToString() + " " + amount + (weight != 0 ? "(" +weight  +")" : "");
  }
}

[System.Serializable]
public struct EffectAmount
{
  public GemEffect effect;
  public int amount;
  public int value;
  public int weight;
}
