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
    [SerializeField] private int bullets = 2;
    private int currentBullets = 0;

    public Sprite Sprite
    {
        get => sprite;
    }

    public int Bullets
    {
        get => bullets;
        set
        {
            bullets = value;
        }
    }

    public int CurrentBullets
    {
        get => currentBullets;
        set
        {
            currentBullets = value;
        }
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
        AmmoManager.AmmoAmount -= 1;
        GetComponentInChildren<ParticleSystem>().Play();

        RaycastHit hit;

        if(Physics.Raycast(front.transform.position, front.transform.forward, out hit, range))
        {
            ShootEffect(hit.point, hit);
            Target target = hit.collider.gameObject.GetComponent<Target>();

            if(target != null)
            {
                target.TakeDamage(damage);

            }
        }
    }

    private void ShootEffect(Vector3 hitPoint, RaycastHit hit)
    {
        GameObject shootEffect = Instantiate(shootEffectPrefab, hitPoint + new Vector3(0.05f, 0.05f, 0.05f), Quaternion.LookRotation(hit.normal));
        shootEffect.transform.parent = parent.transform;

        StartCoroutine(DespawnShootEffect(shootEffect));

    }

    private IEnumerator DespawnShootEffect(GameObject shootEffect)
    {
        yield return new WaitForSeconds(2f);
        Destroy(shootEffect);
    }
}
