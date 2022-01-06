using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachingeGun : Gun
{
    public virtual void Update()
    {
        if(ShootAction.GetState(RightHand) && Shoot == false && AmmoManager.CurrentBullets > 0)
        {
            Shoot = true;
            ShootGun();
        }
    }
}
