using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Katana : Weapon
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 5f;
    [SerializeField] private SteamVR_Input_Sources rightHand;
    [SerializeField] private SteamVR_Action_Boolean sliceAction = null;
    [SerializeField] private GameObject front = null;
    private bool slice = false;
    //sounds
    [SerializeField] private AudioClip sliceSound = null;
    private AudioSource audioSource = null; //current audio source


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sliceAction.GetStateDown(rightHand))
        {
            slice = true;
            Hit();
        }
    }

    private void Hit()
    {
        //Animation
        gameObject.GetComponent<Animator>().ResetTrigger("Idle");
        gameObject.GetComponent<Animator>().SetTrigger("Slice");

        //sound
        audioSource.clip = sliceSound;
        audioSource.Play();

        RaycastHit hit;

        if (Physics.Raycast(front.transform.position, front.transform.forward, out hit, range))
        {
            //if Dummy - get script for Dummy and call TakeDamage
            if (hit.collider.gameObject.CompareTag("Dummy"))
            {
                Dummy target = hit.collider.gameObject.GetComponent<Dummy>();

                if (target != null)
                {
                    target.TakeDamage(damage);

                }
            }

            slice = false;
            gameObject.GetComponent<Animator>().ResetTrigger("Slice");
            gameObject.GetComponent<Animator>().SetTrigger("Idle");

        }
    }
}
