using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MachingeGun : Gun
{

    private int counter = 0; 

    public virtual void Update()
    {
        if(ShootAction.GetState(RightHand) && Shoot == false && AmmoManager.CurrentBullets > 0)
        {
            counter++;
            Shoot = true;
            ShootGun();
        }
    }

    public virtual void ShootGun()
    {
        GetComponentInChildren<ParticleSystem>().Play();
        RightHandGO.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab.gameObject.GetComponent<Gun>().CurrentBullets -= 1;
        AudioSource.clip = ShootingSound;
        
        //sound
        if (counter % 3 == 0)
        {
            AudioSource.Play();
        }

        RaycastHit hit;

        if (Physics.Raycast(Front.transform.position, Front.transform.forward, out hit, Range))
        {
            base.ShootEffect(hit.point, hit);
            //if Dummy - get script for Dummy and call TakeDamage
            if (hit.collider.gameObject.CompareTag("Dummy"))
            {
                Dummy target = hit.collider.gameObject.GetComponent<Dummy>();

                if (target != null)
                {
                    target.TakeDamage(Damage);

                }
            }
            //if falling target - get script for TargetFall and call TakeDamage
            else if (hit.collider.gameObject.CompareTag("TargetFall"))
            {
                TargetFall target = hit.collider.gameObject.GetComponent<TargetFall>();

                if (target != null)
                {
                    target.TakeDamage(Damage);

                }
            }



        }
        Shoot = false;
    }
}
