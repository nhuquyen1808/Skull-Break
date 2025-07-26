using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterArm : BoosterBase
{
    protected override void OnSuccess()
    { 
        ShooterController.Instance.TurnOnArm();
        base.OnSuccess();
    }
}
