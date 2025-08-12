using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill", order = 1)]
public class skillSO : ScriptableObject
{
    public string skillName;
    public string skillDesc;
    public int maxLvl;
    public Sprite skillIcon;
}
