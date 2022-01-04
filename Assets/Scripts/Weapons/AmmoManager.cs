using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AmmoManager : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean reloadAction;
    [SerializeField] private SteamVR_Input_Sources rightHand;
    [SerializeField] private GameObject rightHandGO = null;
    private static int ammoAmount = 0;
    private static int currentBullets = 0;
    private int bullets = 0;
    private AudioSource audioSource = null;

    public static int AmmoAmount
    {
        get => ammoAmount;
        set
        {
            ammoAmount = value;
        }
    }

    public static int CurrentBullets
    {
        get => currentBullets;
        set
        {
            currentBullets = value;
        }
    }

    private void Awake()
    {
        currentBullets = 0;
    }

    private void Update()
    {
        bullets = rightHandGO.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab.gameObject.GetComponent<Gun>().Bullets;
        currentBullets = rightHandGO.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab.gameObject.GetComponent<Gun>().CurrentBullets;

        if (ammoAmount > 0 && reloadAction.GetStateDown(rightHand))
        {
            audioSource.clip = rightHandGO.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab.gameObject.GetComponent<Gun>().ReloadSound;
            audioSource.Play();
            ammoAmount -= (bullets - currentBullets);
            rightHandGO.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab.gameObject.GetComponent<Gun>().CurrentBullets = bullets;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ammopack")
        {
            ammoAmount += other.gameObject.GetComponent<Ammopack>().Ammo;
            other.gameObject.SetActive(false);
        }
    }
}
