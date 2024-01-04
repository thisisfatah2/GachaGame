using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum GachaType { Guaranteed, SoftPity, Rateup }

public class GachaManager : MonoBehaviour
{
    // Gacha System Global
    [SerializeField] GachaRate[] gacha;

    [SerializeField] CardInfo[] rewards;

    [SerializeField] Transform parent, pos, finishLocation;

    [SerializeField] GameObject characterCardGo;

    public List<Cards> card;

    [SerializeField] List<Transform> parents;

    // Gacha System Rate up
    [SerializeField] CardInfo[] rateUpRewards;
    [Range(0.0f, 10.0f)][SerializeField] int rateUpRate;

    //Gacha System Soft Pity
    [SerializeField] int legendSoftPity;
    int[] normalRate;

    // Gacha System Guaranteed
    int guaranteedPull = 10;
    [SerializeField] TextMeshProUGUI pullLeft;

    bool completeTenPull = false;

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

    private void LateUpdate()
    {
        if (completeTenPull)
        {
            completeTenPull = false;

            for (int i = 0; i < card.Count; i++)
            {
                card[i].MoveToFinishLocation();
            }
            card.Clear();
        }
    }

    void GachaOneTime(GachaType type)
    {

        if (Data.isCanGacha)
        {
            Data.isCanGacha = false;
            switch (type)
            {
                case GachaType.Guaranteed:
                    GuaranteedGachaPull();
                    break;
                case GachaType.SoftPity:
                    GachasoftPity();
                    break;
                case GachaType.Rateup:
                    GachaRateup();
                    break;
            }

            SpawnNewCard(cardInfo).MoveToLocation(parent, finishLocation.position, true);
        }
    }

    void GachaTenTime(GachaType type)
    {
        if (Data.isCanGacha)
        {
            Data.isCanGacha = false;
            StartCoroutine(GachaDelayForMultiple(type));
        }
    }

    #region Guaranteed
    public void PullOneTimeGuaranteed()
    {
        GachaOneTime(GachaType.Guaranteed);

    }

    public void PullTenTimeGuaranteed()
    {
        GachaTenTime(GachaType.Guaranteed);
    }

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
                cardInfo = Reward(gacha[i].rarity);
                break;
            }
        }
    }
    #endregion

    #region SoftPity
    public void PullOneTimeSoftPity()
    {
        GachaOneTime(GachaType.SoftPity);

    }

    public void PullTenTimeSoftPity()
    {
        GachaTenTime(GachaType.SoftPity);
    }

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
    }

    void RefreshGachaRate()
    {
        for (int i = 0; i < gacha.Length; i++)
        {
            gacha[i].rate = normalRate[i];
        }
    }

    void AddRate()
    {
        for (int i = 0; i < gacha.Length; i++)
        {
            if (gacha[i].rate != 100)
            {
                gacha[i].rate += legendSoftPity;
            }
        }
    }
    #endregion

    #region Rate up reward
    /// <summary>
    /// Untuk rate up reward
    /// </summary>
    /// <param name="rarity"></param>
    /// <returns></returns>
    public void PullOneTimeRateup()
    {
        GachaOneTime(GachaType.Rateup);

    }

    public void PullTenTimeRateup()
    {
        GachaTenTime(GachaType.Rateup);
    }

    void GachaRateup()
    {
        int rnd = UnityEngine.Random.Range(1, 101);
        for (int i = 0; i < gacha.Length; i++)
        {
            if (rnd <= gacha[i].rate)
            {
                CardInfo[] rateUpReward = Array.FindAll(rateUpRewards, rr => rr.rarity == gacha[i].rarity);
                if (rateUpReward.Length > 0)
                {
                    int index = UnityEngine.Random.Range(1, 11);
                    if (index <= rateUpRate)
                    {
                        index = UnityEngine.Random.Range(0, rateUpReward.Length);
                        cardInfo = rateUpReward[index];
                        break;
                    }
                }
                cardInfo = Reward(gacha[i].rarity);
                break;
            }
        }
    }
    #endregion

    IEnumerator GachaDelayForMultiple(GachaType type)
    {
        int count = 0;
        while (count < 10)
        {
            switch (type)
            {
                case GachaType.Guaranteed:
                    GuaranteedGachaPull();
                    break;
                case GachaType.SoftPity:
                    GachasoftPity();
                    break;
                case GachaType.Rateup:
                    GachaRateup();
                    break;
                default:
                    break;
            }

            Cards _card = SpawnNewCard(cardInfo);
            _card.MoveToLocation(parents[count], finishLocation.position, false);
            card.Add(_card);

            yield return new WaitForSeconds(0.5f);

            if (count == 9)
            {
                int index = 0;
                while (index < card.Count)
                {
                    card[index].PlayAnimation();
                    index++;
                    if (index >= card.Count)
                    {
                        completeTenPull = true;
                    }
                    yield return new WaitForSeconds(0.6f);
                }
            }
            count += 1;
        }
    }

    void SetPullText()
    {
        if (pullLeft != null)
        {
            pullLeft.text = "Guaranteed " + guaranteedPull + " Pulls";
        }
    }

    /// <summary>
    /// Untuk spawn kartu baru
    /// </summary>
    Cards SpawnNewCard(CardInfo newCardinfo)
    {
        GameObject characterCard = Instantiate(characterCardGo, pos.position, Quaternion.identity, pos) as GameObject;
        characterCard.transform.localScale = Vector3.one;

        Cards newCard = characterCard.GetComponent<Cards>();
        newCard.SetCardInfo(newCardinfo);

        return newCard;
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
        CardInfo[] reward = Array.FindAll(rewards, r => r.rarity == rarity);
        int rnd = UnityEngine.Random.Range(0, reward.Length);

        return reward[rnd];
    }
}