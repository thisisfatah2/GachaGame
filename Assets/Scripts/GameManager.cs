using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject CardSpawn;
    [SerializeField] Transform cardSpawnTransform;
    [SerializeField] Transform cardSpawnTarget;

    [SerializeField] GachaManager gachaManager;
    [SerializeField] SavingCards savingCards;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
}
