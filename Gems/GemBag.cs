using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBag : MonoBehaviour
{
    public List<BagGem> gems = new List<BagGem>();
    public Gem basicGem;
    public int gemChance = 10;

    public void AddGem(Gem gem)
    {
        gems.Add(new BagGem(gem));
    }

    public Gem GetGem(GemColor color)
    {
        if (Random.Range(0,100) < gemChance)
        {
            List<BagGem> matches = gems.FindAll(g => !g.inPlay && g.gem.IsMatch(color));
            if (matches.Count > 0)
            {
                return matches[Random.Range(0, matches.Count)].gem;
            }
        }
        return basicGem;
    }

    public void Reset()
    {
        foreach (BagGem g in gems)
        {
            g.inPlay = false;
        }
    }
}

public class BagGem
{
    public Gem gem;
    public bool inPlay;

    public BagGem(Gem gem)
    {
        this.gem = gem;
        inPlay = false;
    }
}