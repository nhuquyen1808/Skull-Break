using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterAdd5 : BoosterBase
{
    protected override void OnSuccess()
    {
        ShooterController.Instance.AddBallCount();
        base.OnSuccess();
    }
}
