using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SweetsSpec : MonoBehaviour
{   
    // 基礎値段、コンボ倍率、出現数のテキスト
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI magnificationText;
    [SerializeField] private TextMeshProUGUI occurrenceText;
    [SerializeField] private TextMeshProUGUI occurrencePercentText;
    
    public void SetSpecTexts(string price, string magnification, string occurrence, string occurrencePercent){
        priceText.text = price;
        magnificationText.text = magnification;
        occurrenceText.text = occurrence;
        occurrencePercentText.text = occurrencePercent;
    }
}
