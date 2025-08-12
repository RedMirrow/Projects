using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSwitch : MonoBehaviour
{
    public bool isSwitched;
    public GameObject win1, win2;
    public void Update() {
        if (!isSwitched)
        {
            win1.SetActive(true);
            win2.SetActive(false);
        }
        else
        {
            win1.SetActive(false);
            win2.SetActive(true);
        }
    }
    public void SwitchWindow() {
        if (isSwitched) { isSwitched = false; }
        else { isSwitched = true; }
    }

}
