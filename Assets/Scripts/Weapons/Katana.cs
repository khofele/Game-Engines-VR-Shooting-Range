using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Katana : Weapon
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private SteamVR_Input_Sources rightHand;
    private bool slice = false;
    //sounds
    [SerializeField] private AudioClip sliceSound = null;
    private AudioSource audioSource = null; //current audio source


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dummy"))
        {
            Dummy target = other.gameObject.GetComponent<Dummy>();

            if (target != null)
            {
                audioSource.clip = sliceSound;
                audioSource.Play();

                target.TakeDamage(damage);

            }
        }
    }
}
