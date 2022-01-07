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
    private UIManagerGame uIManagerGame = new UIManagerGame();

    private void Start()
    {
        //rightHand.GetComponent<Hand>().SetRenderModel(shortRangeWeapon);
        //currentWeapon = rightHand.GetComponent<Hand>().renderModelPrefab;
    }

    private void ChangeCurrentWeapon()
    {
        //if(newWeapon != currentWeapon)
        //{
            
            rightHand.GetComponent<Hand>().renderModelPrefab = newWeapon;
            rightHand.GetComponent<Hand>().SetRenderModel(newWeapon);
            currentWeapon = newWeapon;
        //}
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
