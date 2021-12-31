using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] private GameObject rightHand = null;
    private GameObject newWeapon = null;
    private GameObject currentWeapon = null;

    private void Start()
    {
        currentWeapon = rightHand.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab.gameObject;
    }

    private void ChangeCurrentWeapon()
    {
        if(newWeapon != currentWeapon)
        {
            rightHand.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab = newWeapon;
            currentWeapon = newWeapon;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            newWeapon = other.gameObject;
            ChangeCurrentWeapon();
        }
    }
}
