using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Cards : MonoBehaviour
{
    public CardInfo cardInfo;
    [SerializeField] Image characterImg;
    [SerializeField] Image cardImg;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text attactText;
    [SerializeField] TMP_Text armorText;

    Vector3 finishPosition;

    [SerializeField] Animator anim;

    public void SetCardInfo(CardInfo _cardInfo)
    {
        cardInfo = _cardInfo;
        characterImg.sprite = cardInfo.characterImg;
        cardImg.sprite = cardInfo.cardImg;
        nameText.text = cardInfo.name;
        healthText.text = cardInfo.health.ToString();
        attactText.text = cardInfo.attack.ToString();
        armorText.text = cardInfo.armor.ToString();
    }

    public void MoveToLocation(Transform targetLocation, Vector3 finishLocation, bool withAnimation)
    {
        transform.DOMove(targetLocation.position, 0.7f, true).OnComplete(() =>
        {
            gameObject.transform.SetParent(targetLocation);
            cardInfo.cardCount += 1;
            finishPosition = finishLocation;

            if (withAnimation)
            {
                PlayAnimation();
                StartCoroutine(DelayMoveToFinishLocation(finishPosition));
            }
        });
    }

    public void MoveToFinishLocation()
    {
        StartCoroutine(DelayMoveToFinishLocation(finishPosition));
    }

    IEnumerator DelayMoveToFinishLocation(Vector3 targetLocation)
    {
        yield return new WaitForSeconds(1.0f);

        transform.DOMove(targetLocation, 1.0f, true).OnComplete(() =>
        {
            transform.DOScale(0, 0.3f).OnComplete(() =>
            {
                Data.isCanGacha = true;
                Destroy(gameObject);
            });
        });
    }

    public void PlayAnimation()
    {
        anim.Play("SpawnAnimation");
    }
}
