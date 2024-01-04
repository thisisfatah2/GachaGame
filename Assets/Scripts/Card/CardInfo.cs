using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Character", menuName = "New Card Info")]
public class CardInfo : ScriptableObject
{
    public Sprite characterImg;
    public Sprite cardImg;

    public new string name;
    public int health;
    public int attack;
    public int armor;

    public Rarity rarity;

    public int cardCount;
}
