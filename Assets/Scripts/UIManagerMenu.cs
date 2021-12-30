using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel = null;
    [SerializeField] private GameObject optionsPanel = null;
    
    public GameObject OptionsPanel
    {
        get => optionsPanel;
    }

    public void DisplayMenuPanel()
    {
        menuPanel.SetActive(true);
    }

    public void DisableMenuPanel()
    {
        menuPanel.SetActive(false);
    }

    public void DisplayOptionsPanel()
    {
        optionsPanel.SetActive(true);
    }

    public void DisableOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }
}
