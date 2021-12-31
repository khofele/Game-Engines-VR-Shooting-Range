using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class UIManagerGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI points = null;
    [SerializeField] private TextMeshProUGUI ammo = null;
    [SerializeField] private GameObject rightHand = null;
    [SerializeField] private Image currentWeaponImage = null;
    private Sprite currentWeaponSprite = null;

    private void Update()
    {
        points.text = "Punkte: " + GameManager.Points;
        ammo.text = rightHand.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab.gameObject.GetComponent<Gun>().CurrentBullets.ToString() + "/" + AmmoManager.AmmoAmount.ToString();

        currentWeaponSprite = rightHand.GetComponent<Hand>().renderModelPrefab.gameObject.GetComponent<RenderModel>().controllerPrefab.gameObject.GetComponent<Gun>().Sprite;

        if(currentWeaponSprite != null)
        {
            currentWeaponImage.GetComponent<Image>().sprite = currentWeaponSprite;
            currentWeaponImage.SetNativeSize();
            currentWeaponImage.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
    }
}
