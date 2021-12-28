using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 80f;
    [SerializeField] private SteamVR_Input_Sources rightHand;
    [SerializeField] private Camera cam = null;
    [SerializeField] private ParticleSystem particleSys = null;
    [SerializeField] private SteamVR_Action_Boolean shootAction = null;
    [SerializeField] private Sprite sprite = null;

    public Sprite Sprite
    {
        get => sprite;
    }

    private void Update()
    {
        if(shootAction.GetStateDown(rightHand))
        {
            ShootGun();
        }
    }

    private void ShootGun()
    {
        GetComponent<ParticleSystem>().Play();

        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Target target = hit.collider.gameObject.GetComponent<Target>();

            if(target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
