using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] private GameObject rightHand = null;
    [SerializeField] private GameObject shortRangeWeapon = null;
    private GameObject newWeapon = null;
    private static GameObject currentWeapon = null;

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
            newWeapon = other.gameObject.GetComponent<Gun>().PrefabHand;
            ChangeCurrentWeapon();
        }
    }
}
