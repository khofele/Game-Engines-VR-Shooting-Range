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
    private static int ammoAmount = 10;
    private int bullets = 0;
    private int currentBullets = 0;

    public static int AmmoAmount
    {
        get => ammoAmount;
        set
        {
            ammoAmount = value;
        }
    }

    private void Update()
    {
        bullets = rightHandGO.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab.gameObject.GetComponent<Gun>().Bullets;
        currentBullets = rightHandGO.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab.gameObject.GetComponent<Gun>().CurrentBullets;

        if (ammoAmount > 0 && reloadAction.GetStateDown(rightHand))
        {
            ammoAmount = ammoAmount - (bullets - currentBullets);
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
