using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CharController : MonoBehaviour
{
    [SerializeField] private SteamVR_Input_Sources leftHand;
    [SerializeField] private SteamVR_Input_Sources rightHand;
    [SerializeField] private SteamVR_Action_Boolean isJumping;
    [SerializeField] private SteamVR_Action_Vector2 trackpadAction;
    [SerializeField] private float jumpHeight = 20000;
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float trackPadEnd = 0;
    [SerializeField] private GameObject axisHand = null;

    private Rigidbody rbody = null;
    private bool doubleJump = false;
    private bool isGrounded = true;
    private Vector3 moveDirection = Vector3.zero;
    private float gravity = -9.81f * 2;

    private Vector2 trackpad = Vector2.zero;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //float xAxis = Input.GetAxis("Horizontal");
        //float zAxis = Input.GetAxis("Vertical");

        UpdateInput();

        //moveDirection = transform.right * xAxis + transform.forward * zAxis;
        moveDirection = Quaternion.AngleAxis(Angle(trackpad) + axisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;

        velocity = Vector3.zero;

        if(trackpad.magnitude > trackPadEnd)
        {
            velocity = moveDirection;
            rbody.AddForce(velocity.x * movementSpeed - rbody.velocity.x, 0, velocity.z * movementSpeed - rbody.velocity.z, ForceMode.VelocityChange);
        }



        if (isJumping.GetStateDown(leftHand) && isGrounded == true)
        {
            Debug.Log("is jumping");
            rbody.AddForce(new Vector3(0, (Mathf.Sqrt(jumpHeight * -2f * gravity)), 0), ForceMode.VelocityChange);
            isGrounded = false;
            doubleJump = true;
        }
        else if(isJumping.GetStateDown(leftHand) && doubleJump == true)
        {
            Debug.Log("double jump");
            rbody.AddForce(new Vector3(0, (Mathf.Sqrt(jumpHeight * -2f * gravity)), 0), ForceMode.VelocityChange);
            isGrounded = false;
            doubleJump = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
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
