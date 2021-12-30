using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class Pointer : MonoBehaviour
{
    [SerializeField] private float m_DefaultLength = 5.0f;
    [SerializeField] private GameObject m_Dot = null;
    [SerializeField] private VRInputModule m_InputModule = null;
    [SerializeField] private LayerMask clickable = default;
    [SerializeField] private SteamVR_Action_Boolean clickAction;
    [SerializeField] private SteamVR_Input_Sources rightHand;
    [SerializeField] private GameObject rightHandGO = null;
    private LineRenderer m_LineRenderer = null;

    void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.enabled = true;
        m_LineRenderer.startWidth = 0.02f;
        m_LineRenderer.endWidth = 0.02f;
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        //Use default (when hitting nothing) or distance (to hitted object)
        PointerEventData data = m_InputModule.GetData();
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? m_DefaultLength : data.pointerCurrentRaycast.distance;

        if(Physics.Raycast(rightHandGO.transform.position, rightHandGO.transform.forward, out RaycastHit hit, m_DefaultLength, clickable))
        {
            Vector3 endPosition = transform.position + (transform.forward * targetLength);

            m_LineRenderer.SetPositions(new Vector3[] { rightHandGO.transform.position, hit.point });
            //...or based on hit
            //if (hit.collider != null)
            //{
            //    endPosition = hit.point;
            //}

            //Set position of the dot
            m_Dot.transform.position = endPosition;
            //Set position of line renderer
            m_LineRenderer.SetPosition(0, transform.position);
            m_LineRenderer.SetPosition(1, endPosition);

            if(clickAction.GetStateDown(rightHand))
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "ShootingRange":
                        Debug.Log("ShootingRange");
                        break;

                    case "Settings":
                        Debug.Log("Settings");
                        break;

                    case "Exit":
                        Debug.Log("Exit");
                        break;
                }
            }
        }
    }
}
