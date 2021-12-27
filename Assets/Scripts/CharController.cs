using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CharController : MonoBehaviour
{
    // general
    [SerializeField] private SteamVR_Input_Sources leftHand;
    [SerializeField] private SteamVR_Input_Sources rightHand;
    [SerializeField] private Camera cam = null;
    private Rigidbody rbody = null;
    private bool isGrounded = true;
    private Vector3 moveDirection = Vector3.zero;
    private float gravity = -9.81f * 2;

    // groundcheck
    [SerializeField] private Transform groundCheck = null;
    [SerializeField] private LayerMask environmentLayermask = default;
    private float groundDistance = 0.4f;

    // jump
    [SerializeField] private SteamVR_Action_Boolean jumpAction;
    [SerializeField] private float jumpHeight = 20000;
    [SerializeField] private float movementSpeed = 1;
    private bool doubleJump = false;

    // walk with trackpad
    [SerializeField] private SteamVR_Action_Vector2 trackpadAction;
    [SerializeField] private float trackPadEnd = 0;
    [SerializeField] private GameObject axisHand = null;
    private Vector2 trackpad = Vector2.zero;
    private Vector3 velocity = Vector3.zero;

    // grappling hook
    [SerializeField] private SteamVR_Action_Boolean grapplingHookAction;
    [SerializeField] private float maxDistance = 15;
    [SerializeField] private float hookSpeed = 200f;
    private bool hookActive = false;
    private LineRenderer lineRenderer = null;
    private Vector3 hookPos = Vector3.zero;
    private float hookStartTime = 0;
    private float hookDuration = 0.5f;

    // climbing
    [SerializeField] private SteamVR_Action_Boolean climbAction;
    private bool isClimbing = false;
    private Vector3 climbDirection = Vector3.zero;


    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, environmentLayermask);

        // Trackpad Input
        UpdateInput();

        moveDirection = Quaternion.AngleAxis(Angle(trackpad) + axisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;

        climbDirection = Quaternion.AngleAxis(Angle(trackpad) + axisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.up;

        velocity = Vector3.zero;

        // ----------- Movement ----------- 
        WalkWithTrackpad();

        Jump();

        GrapplingHook();

        ClimbCheck();
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

    private void Jump()
    {
        if (jumpAction.GetStateDown(leftHand) && isGrounded == true)
        {
            Debug.Log("is jumping");
            rbody.AddForce(new Vector3(0, (Mathf.Sqrt(jumpHeight * -2f * gravity)), 0), ForceMode.VelocityChange);
            isGrounded = false;
            doubleJump = true;
        }
        else if (jumpAction.GetStateDown(leftHand) && doubleJump == true)
        {
            Debug.Log("double jump");
            rbody.AddForce(new Vector3(0, (Mathf.Sqrt(jumpHeight * -2f * gravity)), 0), ForceMode.VelocityChange);
            isGrounded = false;
            doubleJump = false;
        }
    }

    private void WalkWithTrackpad()
    {
        if (trackpad.magnitude > trackPadEnd && isClimbing == false && isGrounded == true)
        {
            velocity = moveDirection;
            rbody.AddForce(velocity.x * movementSpeed - rbody.velocity.x, 0, velocity.z * movementSpeed - rbody.velocity.z, ForceMode.VelocityChange);
        }
    }

    private void GrapplingHook()
    {
        if (grapplingHookAction.GetStateDown(rightHand) && hookActive == false)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxDistance, environmentLayermask))
            {
                hookPos = hit.point;
                lineRenderer.enabled = true;
                hookStartTime = Time.time;
                hookActive = true;
            }
        }
        else if (grapplingHookAction.GetStateDown(rightHand) && hookActive == true)
        {
            hookActive = false;
        }

        if (hookActive == true && Time.time < hookStartTime + hookDuration)
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

    private void ClimbCheck()
    {
        if (climbAction.GetStateDown(leftHand))
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f))
            {
                if (hit.collider.GetComponent<Climbable>() != null)
                {
                    StartCoroutine(Climb(hit.collider));
                }
            }
        }
    }

    private IEnumerator Climb(Collider climbCollider)
    {
        isClimbing = true;
        isGrounded = false;
        doubleJump = true;

        while (climbAction.GetStateDown(leftHand))
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f))
            {
                rbody.isKinematic = true;

                if(hit.collider == climbCollider)
                {
                    if (trackpad.magnitude > trackPadEnd && isClimbing == false)
                    {
                        Debug.Log("is climbing");
                        velocity = climbDirection;
                        rbody.AddForce(0, velocity.y * movementSpeed - rbody.velocity.y, 0, ForceMode.VelocityChange);
                    }
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }

        isClimbing = false;
        rbody.isKinematic = false;
    }
}
