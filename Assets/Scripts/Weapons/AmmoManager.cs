using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    private static int ammoAmount = 100;

    public static int AmmoAmount
    {
        get => ammoAmount;
        set
        {
            ammoAmount = value;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ammopack")
        {
            ammoAmount += other.gameObject.GetComponent<Ammopack>().Ammo;
            Debug.Log("Ammo " + ammoAmount);
        }
    }
}
