using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetInfo 
{
    public static bool IsTargetInRange(Vector3 rayPosition, Vector3 rayDirection, out RaycastHit HitInfo, float range, LayerMask mask) {
        return (Physics.Raycast(rayPosition, rayDirection, out HitInfo, range, mask));

    }

}
