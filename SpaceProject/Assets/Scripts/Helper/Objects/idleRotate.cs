using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleRotate : MonoBehaviour
{
    public void FixedUpdate() { transform.Rotate(0, 2f, 0); }
}
