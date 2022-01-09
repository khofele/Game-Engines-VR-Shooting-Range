using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] private GameObject rightHand = null;
    [SerializeField] private GameObject shortRangeWeapon = null;
    [SerializeField] private GameObject startWeapon = null;
    private GameObject newWeapon = null;
    private static GameObject currentWeapon = null;

    private void Awake()
    {
        rightHand.GetComponent<Hand>().renderModelPrefab = startWeapon;
        rightHand.GetComponent<Hand>().SetRenderModel(startWeapon);
        currentWeapon = startWeapon;
    }

    private void ChangeCurrentWeapon()
    {  
        rightHand.GetComponent<Hand>().renderModelPrefab = newWeapon;
        rightHand.GetComponent<Hand>().SetRenderModel(newWeapon);
        currentWeapon = newWeapon;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            newWeapon = other.gameObject.GetComponent<Weapon>().PrefabHand;
            ChangeCurrentWeapon();
        }
    }
}
