using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidHealth : HealthObjects
{
    public override void TakeDmg(float damage) {
        base.TakeDmg(damage);
    }
    public override void OnDestroy() {
        base.OnDestroy();
    }

}
