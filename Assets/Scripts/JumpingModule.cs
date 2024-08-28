using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BasicMovementModule))]
public class JumpingModule : MonoBehaviour
{
    private ApplyMovement _applyMovement;
    private InputHandler _inputHandler;

    private float jumpHeight = 3f;
    private float gravityValue = -9.81f;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private Vector3 playerVelocity;

    private void OnEnable()
    {
        _applyMovement = GetComponent<ApplyMovement>();
        _inputHandler = GetComponent<InputHandler>();

        GetComponent<InputHandler>().JumpTriggeredCallback += Jumping;
        Debug.Log("Subscribed jump function to Jump Triggered");
    }

    private void OnDisable()
    {
        GetComponent<InputHandler>().JumpTriggeredCallback -= Jumping;
        Debug.Log("Unsubscribed jump function to Jump Triggered");
    }

    private void Jumping()
    {

        isGrounded = _applyMovement.isGrounded;

        if (isGrounded)
        {
            Debug.Log("Grounded");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        else
        {
            playerVelocity = new();
        }

        //Jump control
        playerVelocity.y += gravityValue * Time.deltaTime;
        Debug.Log("Jumping");
    }
    public float ReturnJumpVelocityModifier()
    {
        try
        {
            return playerVelocity.y;
        }
        finally //Reset velocity
        {
            playerVelocity = Vector3.zero;
        }
    }


}
