using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class HoverTipManager : MonoBehaviour
{
    public TMP_Text TitleText;
    public TMP_Text DescriptionText;
    public RectTransform tipWindow;


    public static Action<string,string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus; 

    private void OnEnable() {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }
    private void OnDisable() {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    // Start is called before the first frame update
    void Start()
    {
        HideTip();
    }
    private void ShowTip(string tip1, string tip2, Vector2 mousePos) {

        TitleText.text = tip1;
        DescriptionText.text = tip2;
        tipWindow.gameObject.SetActive(true);
    }
    private void HideTip() {
        TitleText.text = default;
        DescriptionText.text = default;
        tipWindow.gameObject.SetActive(false);
    }

}
