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

    //sounds
    [SerializeField] private AudioClip[] footstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip jumpSound;           // the sound played when character leaves the ground.
    private AudioSource audioSource; //current audio source
    float elapsedTime = 0; //time since last step sound
    float soundDelay = 0.5f; //delay for sound to play

    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, environmentLayermask);

        // Trackpad Input
        UpdateInput();

        moveDirection = Quaternion.AngleAxis(Angle(trackpad) + axisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;

        velocity = Vector3.zero;

        // ----------- Movement ----------- 
        WalkWithTrackpad();

        Jump();

        GrapplingHook();
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
            rbody.AddForce(new Vector3(0, (Mathf.Sqrt(jumpHeight * -2f * gravity)), 0), ForceMode.VelocityChange);
            isGrounded = false;
            doubleJump = true;
            //sound
            audioSource.clip = jumpSound;
            audioSource.Play();
        }
        else if (jumpAction.GetStateDown(leftHand) && doubleJump == true)
        {
            rbody.AddForce(new Vector3(0, (Mathf.Sqrt(jumpHeight * -2f * gravity)), 0), ForceMode.VelocityChange);
            isGrounded = false;
            doubleJump = false;
            //sound
            audioSource.clip = jumpSound;
            audioSource.Play();
        }
    }

    private void WalkWithTrackpad()
    {
        if (trackpad.magnitude > trackPadEnd && isGrounded == true)
        {
            velocity = moveDirection;
            rbody.AddForce(velocity.x * movementSpeed - rbody.velocity.x, 0, velocity.z * movementSpeed - rbody.velocity.z, ForceMode.VelocityChange);

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= soundDelay) 
            {
                elapsedTime = 0;

                // pick & play a random footstep sound from the array,
                // excluding sound at index 0
                int n = Random.Range(1, footstepSounds.Length);
                audioSource.clip = footstepSounds[n];
                audioSource.PlayOneShot(audioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                footstepSounds[n] = footstepSounds[0];
                footstepSounds[0] = audioSource.clip;
            }
            
        }
    }

    private void GrapplingHook()
    {
        if (grapplingHookAction.GetStateDown(leftHand) && hookActive == false)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxDistance, environmentLayermask))
            {
                hookPos = hit.point;
                lineRenderer.enabled = true;
                hookStartTime = Time.time;
                hookActive = true;
            }
        }
        else if (grapplingHookAction.GetStateDown(leftHand) && hookActive == true)
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
}
