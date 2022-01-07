using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Sprite sprite = null;
    [SerializeField] private GameObject prefabHand = null;

    public Sprite Sprite
    {
        get => sprite;
    }

    public GameObject PrefabHand
    {
        get => prefabHand;
    }
}
