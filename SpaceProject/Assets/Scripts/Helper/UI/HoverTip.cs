using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverTip : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public skillSO skillSO;
    public string skillNameTip, skillDescTip;
    public float timeToWait = 0.5f;
    public int ID;

    bool checkIfSkillTreeOpen() {
        if (Input.GetAxisRaw("SkillMenu") != 0) { return true; }
        else { return false; }
    }
    // When mouse hovers
    public void OnPointerEnter(PointerEventData eventData) {
        if (checkIfSkillTreeOpen()) {
            FindObjectOfType<AudioManager>().Play("HoverOverPerk");
            ShowMessage();
        }
        
    }

    // When mouse unhovers
    public void OnPointerExit(PointerEventData eventData) {
        HoverTipManager.OnMouseLoseFocus();
    }
    private void ShowMessage() {
        skillNameTip = skillSO.skillName;
        skillDescTip = skillSO.skillDesc;
        HoverTipManager.OnMouseHover(skillNameTip, skillDescTip, Input.mousePosition);
    }

    private IEnumerator StartTimer() {
        yield return new WaitForSeconds(timeToWait);
        
    }
}
