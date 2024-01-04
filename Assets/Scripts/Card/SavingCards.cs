using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavingCards : MonoBehaviour
{
    [SerializeField] CardInfo cardInfo;
    [SerializeField] Image cardImg;
    [SerializeField] Image characterImg;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text attactText;
    [SerializeField] TMP_Text armorText;
    [SerializeField] TextMeshProUGUI cardCount;

    void Start()
    {
        cardImg.sprite = cardInfo.cardImg;
        characterImg.sprite = cardInfo.characterImg;
        nameText.text = cardInfo.name;
        healthText.text = cardInfo.health.ToString();
        attactText.text = cardInfo.attack.ToString();
        armorText.text = cardInfo.armor.ToString();

        cardCount.text = cardInfo.cardCount.ToString() + "x";
    }
}
