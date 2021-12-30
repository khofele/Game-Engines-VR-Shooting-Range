using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Valve.VR;
using Valve.VR.Extras;

public class ButtonTransitioner : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler*/
{
    [SerializeField] private Color32 m_NormalColor = Color.white;
    [SerializeField] private Color32 m_HoverColor = Color.grey;
    [SerializeField] private Color32 m_DownColor = Color.white;
    [SerializeField] private LaserPointer laserPointer = null;

    private Image m_Image = null;
    private bool selected = false;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    private void Start()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {

        if (e.target.name == this.gameObject.name && selected == false)
        {
            selected = true;
            e.target.gameObject.GetComponent<Image>().color = m_HoverColor;
            Debug.Log("pointer is inside this object" + e.target.name);
        }
    }
    public void PointerOutside(object sender, PointerEventArgs e)
    {

        if (e.target.name == this.gameObject.name && selected == true)
        {
            e.target.gameObject.GetComponent<Image>().color = m_NormalColor;
            selected = false;
            Debug.Log("pointer is outside this object" + e.target.name);
        }
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    m_Image.color = m_HoverColor;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    m_Image.color = m_NormalColor;
    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    m_Image.color = m_DownColor;
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{

    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    m_Image.color = m_HoverColor;
    //}
}
