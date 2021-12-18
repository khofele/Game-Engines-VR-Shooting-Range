using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CharController : MonoBehaviour
{
    [SerializeField] private SteamVR_Input_Sources rightHand;
    [SerializeField] private SteamVR_Action_Boolean isJumping;
    [SerializeField] private float jumpHeight = 2000000;

    private Rigidbody rbody = null;
    private bool doubleJump = false;
    private bool isGrounded = true;
    private Vector3 moveDirection = Vector3.zero;
    private float gravity = -9.81f * 2;

    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        moveDirection = transform.right * xAxis + transform.forward * zAxis;

        if (isJumping.GetStateDown(rightHand) && isGrounded == true)
        {
            Debug.Log("is jumping");
            rbody.AddForce(new Vector3(0, (Mathf.Sqrt(jumpHeight * -2f * gravity)), 0), ForceMode.VelocityChange);
            isGrounded = false;
            doubleJump = true;
        }
        else if(isJumping.GetStateDown(rightHand) && doubleJump == true)
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
}
