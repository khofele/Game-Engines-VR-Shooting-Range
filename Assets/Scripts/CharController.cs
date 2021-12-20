using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CharController : MonoBehaviour
{
    [SerializeField] private SteamVR_Input_Sources leftHand;
    [SerializeField] private SteamVR_Input_Sources rightHand;
    [SerializeField] private SteamVR_Action_Boolean jumpAction;
    [SerializeField] private SteamVR_Action_Vector2 trackpadAction;
    [SerializeField] private SteamVR_Action_Boolean grapplingHookAction;
    [SerializeField] private float jumpHeight = 20000;
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float trackPadEnd = 0;
    [SerializeField] private GameObject axisHand = null;
    [SerializeField] private Camera cam = null;

    private Rigidbody rbody = null;
    private bool doubleJump = false;
    private bool isGrounded = true;
    private Vector3 moveDirection = Vector3.zero;
    private float gravity = -9.81f * 2;

    // walk with trackpad
    private Vector2 trackpad = Vector2.zero;
    private Vector3 velocity = Vector3.zero;

    // grappling hook
    [SerializeField] private float maxDistance = 15;
    [SerializeField] private LayerMask environment = default;
    [SerializeField] private float hookSpeed = 200f;
    private bool hookActive = false;
    private LineRenderer lineRenderer = null;
    private Vector3 hookPos = Vector3.zero;
    private float hookStartTime = 0;
    private float hookDuration = 0.5f;


    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }

    private void Update()
    {
        // Trackpad Input
        UpdateInput();

        moveDirection = Quaternion.AngleAxis(Angle(trackpad) + axisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;

        velocity = Vector3.zero;

        // Walk with trackpad
        if(trackpad.magnitude > trackPadEnd)
        {
            velocity = moveDirection;
            rbody.AddForce(velocity.x * movementSpeed - rbody.velocity.x, 0, velocity.z * movementSpeed - rbody.velocity.z, ForceMode.VelocityChange);
        }

        // Jump + double jump
        if (jumpAction.GetStateDown(leftHand) && isGrounded == true)
        {
            Debug.Log("is jumping");
            rbody.AddForce(new Vector3(0, (Mathf.Sqrt(jumpHeight * -2f * gravity)), 0), ForceMode.VelocityChange);
            isGrounded = false;
            doubleJump = true;
        }
        else if(jumpAction.GetStateDown(leftHand) && doubleJump == true)
        {
            Debug.Log("double jump");
            rbody.AddForce(new Vector3(0, (Mathf.Sqrt(jumpHeight * -2f * gravity)), 0), ForceMode.VelocityChange);
            isGrounded = false;
            doubleJump = false;
        }

        // Grappling hook
        if(grapplingHookAction.GetStateDown(rightHand) && hookActive == false)
        {
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxDistance, environment))
            {
                hookPos = hit.point;
                lineRenderer.enabled = true;
                hookStartTime = Time.time;
                hookActive = true;
            }
        }
        else if(grapplingHookAction.GetStateDown(rightHand) && hookActive == true)
        {
            hookActive = false;
        }

        if(hookActive == true && Time.time < hookStartTime + hookDuration)
        {
            Vector3 hookDirection = (hookPos - transform.position).normalized;
            rbody.AddForce(hookDirection * hookSpeed * Time.deltaTime, ForceMode.VelocityChange);
            lineRenderer.SetPositions(new Vector3[] { transform.position, hookPos });
        }
        else
        {
            hookActive = false;
            lineRenderer.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Groundcheck
        if (collision.gameObject.CompareTag("Environment"))
        {
            isGrounded = true;
            Debug.Log("is grounded");
        }
    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    private void UpdateInput()
    {
        trackpad = trackpadAction.GetAxis(rightHand);
    }
}
