using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity { Legend,Rare,Common}

[System.Serializable]
public class GachaRate
{
    public Rarity rarity;

    [Range(1.0f, 100.0f)]
    public int rate;
}
