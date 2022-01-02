using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 80f;
    [SerializeField] private SteamVR_Input_Sources rightHand;
    [SerializeField] private SteamVR_Action_Boolean shootAction = null;
    [SerializeField] private Sprite sprite = null;
    [SerializeField] private GameObject shootEffectPrefab = null;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject front = null;
    [SerializeField] private GameObject parent = null;

    public Sprite Sprite
    {
        get => sprite;
    }

    private void Awake()
    {
        parent = GameObject.Find("ShootEffect");
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
        GameManager.Ammo -= 1;
        GetComponentInChildren<ParticleSystem>().Play();

        RaycastHit hit;

        if(Physics.Raycast(front.transform.position, front.transform.forward, out hit, range))
        {
            ShootEffect(hit.point, hit);
            //if Dummy - get script for Dummy and call TakeDamage
            if (hit.collider.gameObject.CompareTag("Dummy"))
            {
                Dummy target = hit.collider.gameObject.GetComponent<Dummy>();

                if (target != null)
                {
                    target.TakeDamage(damage);

                }
            }
            //if falling target - get script for TargetFall and call TakeDamage
            else if (hit.collider.gameObject.CompareTag("TargetFall"))
            {
                TargetFall target = hit.collider.gameObject.GetComponent<TargetFall>();

                if (target != null)
                {
                    target.TakeDamage(damage);

                }
            }
            
        }
    }

    private void ShootEffect(Vector3 hitPoint, RaycastHit hit)
    {
        GameObject shootEffect = Instantiate(shootEffectPrefab, hitPoint + new Vector3(0.05f, 0.05f, 0.05f), Quaternion.LookRotation(hit.normal));
        shootEffect.transform.parent = parent.transform;

    }
}
