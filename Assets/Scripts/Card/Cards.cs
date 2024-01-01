using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cards : MonoBehaviour
{
    public CardInfo cardInfo;
    [SerializeField] Image characterImg;
    [SerializeField] Image cardImg;
    [SerializeField] TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cardInfo != null)
        {
            characterImg.sprite = cardInfo.characterImg;
            cardImg.sprite = cardInfo.cardImg;
            text.text = cardInfo.name;
        }
    }
}
