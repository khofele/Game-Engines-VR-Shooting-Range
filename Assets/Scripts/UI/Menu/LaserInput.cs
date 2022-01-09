using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR;

public class LaserInput : MonoBehaviour
{
    [SerializeField] private static GameObject currentGameObject = null;
    [SerializeField] private LayerMask clickable = default;
    [SerializeField] private SteamVR_Action_Boolean clickAction;
    [SerializeField] private SteamVR_Input_Sources rightHand;
    [SerializeField] private UIManagerMenu uIManagerMenu = null;
    [SerializeField] private GameObject slider = null;
    private int ID = 0;

    private void Update()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, 500.0f, clickable);

        if (clickAction.GetStateDown(rightHand))
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                int instanceID = hit.collider.gameObject.GetInstanceID();

                if (ID != instanceID)
                {
                    ID = instanceID;
                    currentGameObject = hit.collider.gameObject;

                    switch (currentGameObject.tag)
                    {
                        case "ShootingRange":
                            SceneManager.LoadScene("Main");
                            break;

                        case "Settings":
                            uIManagerMenu.DisableMenuPanel();
                            uIManagerMenu.DisplayOptionsPanel();
                            break;

                        case "Exit":
                            Application.Quit();
                            break;

                        case "IncreaseVol":
                            slider.GetComponent<SliderControl>().IncreaseByOne();
                            break;

                        case "DecreaseVol":
                            slider.GetComponent<SliderControl>().DecreaseByOne();
                            break;

                        case "Back":
                            uIManagerMenu.DisableOptionsPanel();
                            uIManagerMenu.DisplayMenuPanel();
                            break;
                    }
                }
            }
        }
    }
}
