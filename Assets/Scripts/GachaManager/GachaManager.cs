using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    // Gacha System Global
    [SerializeField] GachaRate[] gacha;

    [SerializeField] CardInfo[] rewards;

    [SerializeField] Transform parent, pos;

    [SerializeField] GameObject characterCardGo;

    GameObject characterCard;

    Cards card;

    // Gacha System Rate up
    [SerializeField] CardInfo[] rateUpRewards;
    [Range(0.0f, 10.0f)][SerializeField] int rateUpRate;

    //Gacha System Soft Pity
    [SerializeField] int legendSoftPity;
    int[] normalRate;

    // Gacha System Guaranteed
    int guaranteedPull = 10;
    [SerializeField] TextMeshProUGUI pullLeft;

    CardInfo cardInfo;

    private void Start()
    {
        normalRate = new int[gacha.Length];
        for (int i = 0; i < gacha.Length; i++)
        {
            normalRate[i] = gacha[i].rate;
        }

        SetPullText();
    }

    public void Gacha()
    {
        GuaranteedGachaPull();
    }

#region SoftPity
    void GachasoftPity()
    {
        int rnd = UnityEngine.Random.Range(1, 101);
        for (int i = 0; i < gacha.Length; i++)
        {
            if (rnd <= gacha[i].rate)
            {
                Debug.Log(gacha[i].rarity);
                if (gacha[i].rarity != Rarity.Legend)
                {
                    AddRate();
                }
                else
                {
                    RefreshGachaRate();
                }
                cardInfo = Reward(gacha[i].rarity);
                break;
            }
        }

        SpawnNewCard(cardInfo);
    }

    void RefreshGachaRate()
    {
        Debug.Log("Refresh Gacha Rate");
        for (int i = 0; i < gacha.Length; i++)
        {
            gacha[i].rate = normalRate[i];
        }
    }

    void AddRate()
    {
        Debug.Log("Add Rate");
        for (int i = 0; i < gacha.Length; i++)
        {
            if (gacha[i].rate != 100)
            {
                gacha[i].rate += legendSoftPity;
            }
        }
    }
#endregion

    void GuaranteedGachaPull()
    {
        int rnd = UnityEngine.Random.Range(1, 101);
        for (int i = 0; i < gacha.Length; i++)
        {
            if (rnd <= gacha[i].rate)
            {
                if (gacha[i].rarity != Rarity.Legend)
                    guaranteedPull -= 1;
                else
                    guaranteedPull = 10;

                SetPullText();

                if (guaranteedPull == 0)
                {
                    Debug.Log(Rarity.Legend);
                    guaranteedPull = 10;
                    SetPullText();
                    cardInfo = Reward(Rarity.Legend);
                    break;
                }
                /*card.cardInfo = Reward(gacha[i].rarity);*/
                cardInfo = Reward(gacha[i].rarity);
                break;
            }
        }
        SpawnNewCard(cardInfo);
    }

    void SetPullText()
    {
        pullLeft.text = "Guaranteed " + guaranteedPull + " Pulls";
    }

    /// <summary>
    /// Untuk Gacha normal dan rate up gacha
    /// </summary>
    void NormalGacha()
    {
        int rnd = UnityEngine.Random.Range(1, 101);
        for (int i = 0; i < gacha.Length; i++)
        {
            if (rnd <= gacha[i].rate)
            {
                Debug.Log(gacha[i].rarity);
                /*card.cardInfo = Reward(gacha[i].rarity);*/
                cardInfo = RewardupRates(gacha[i].rarity);
                break;
            }
        }

        SpawnNewCard(cardInfo);
    }

    /// <summary>
    /// Untuk spawn kartu baru
    /// </summary>
    void SpawnNewCard(CardInfo newCardinfo)
    {
        if (characterCard == null)
        {
            characterCard = Instantiate(characterCardGo, pos.position, Quaternion.identity) as GameObject;
            characterCard.transform.SetParent(parent);
            characterCard.transform.localScale = Vector3.one;
            card = characterCard.GetComponent<Cards>();
            card.cardInfo = newCardinfo;
        }
    }

    // mendapatkan value rarity
    public int Rates(Rarity rarity)
    {
        GachaRate gr = Array.Find(gacha, rt => rt.rarity == rarity);

        return gr != null ? gr.rate : 0;
    }

    /// <summary>
    /// untuk reward biasa
    /// </summary>
    /// <param name="rarity"></param>
    /// <returns></returns>
    CardInfo Reward(Rarity rarity)
    {
        /*CardInfo[] reward = Array.FindAll();*/
        CardInfo reward = Array.Find(rewards, r => r.rarity == rarity);

        return reward;
    }

    /// <summary>
    /// Untuk rate up reward
    /// </summary>
    /// <param name="rarity"></param>
    /// <returns></returns>
    CardInfo RewardupRates(Rarity rarity)
    {
        CardInfo reward = Array.Find(rewards, r => r.rarity == rarity);

        CardInfo[] rateUpReward = Array.FindAll(rateUpRewards, rr => rr.rarity == rarity);

        if (rateUpReward.Length > 0)
        {
            int rnd = UnityEngine.Random.Range(1, 11);
            if (rnd <= rateUpRate)
            {
                Debug.Log("Rate Up Reward");
                rnd = UnityEngine.Random.Range(0, rateUpReward.Length);
                return rateUpReward[rnd];
            }
        }

        return reward;
    }
}

[CustomEditor(typeof(GachaManager)), CanEditMultipleObjects]
public class GachaEditor : Editor
{
    public int common, uncommon, rare, Epic, Legend;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        GachaManager gm = (GachaManager)target;

        common = EditorGUILayout.IntField("Common", (gm.Rates(Rarity.Common) - gm.Rates(Rarity.Rare)));
        rare = EditorGUILayout.IntField("Rare", (gm.Rates(Rarity.Rare) - gm.Rates(Rarity.Legend)));
        Legend = EditorGUILayout.IntField("Legend", gm.Rates(Rarity.Legend));
    }
}