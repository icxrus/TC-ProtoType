using System.Collections;
using UnityEngine;

public class ApplyMovement : MonoBehaviour
{
    private CharacterController controller;
    private InputHandler _inputHandler;
    private BasicMovementModule _basicMovementModule;

    [SerializeField]
    private Vector3 playerVelocity;
    private float vertVelocity;
    [SerializeField]
    public bool isGrounded = true;

    private float gravityValue = 9.81f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _inputHandler = GetComponent<InputHandler>();
        _basicMovementModule = GetComponent<BasicMovementModule>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check
        isGrounded = GroundCheck();
        playerVelocity = _basicMovementModule.ReturnMoveVector3Values();

        if (isGrounded) 
        {
            vertVelocity = 0;
            if (GetComponent<JumpingModule>() && _inputHandler.JumpTriggeredCheckForDownForce()) //If we are jumping we should change the vertical velocity to be that of the jump
            {
                vertVelocity = GetComponent<JumpingModule>().ReturnJumpVelocityModifier();
            }
        }

        //Apply Gravity to velocity
        vertVelocity -= gravityValue * Time.deltaTime;
        playerVelocity.y = vertVelocity;

        //Reduce Slope bounce
        bool jumpTriggered = _inputHandler.JumpTriggeredCheckForDownForce();
        if (isGrounded && !jumpTriggered)
        {
            Vector3 rayOrigin = transform.position - transform.up * (controller.height / 2 - controller.radius);
            float rayDistance = controller.stepOffset;
            float rayRadius = controller.radius + controller.skinWidth;

            if (Physics.SphereCast(rayOrigin, rayRadius, -transform.up, out RaycastHit hit, rayDistance))
            {
                Vector3 offset = hit.point - transform.position;
                float o = Vector3.ProjectOnPlane(offset, transform.up).magnitude;
                float a = Mathf.Sqrt(Mathf.Pow(rayRadius, 2) - Mathf.Pow(o, 2));
                float floorDelta = (controller.height / 2 - controller.radius + a) - Vector3.Project(offset, transform.up).magnitude;
                transform.position -= transform.up * floorDelta * 2f;
            }
        }

        //Do the movings
        controller.Move(playerVelocity * Time.deltaTime);
        //StartCoroutine(UpdateGroundedStatus());
    }

    public bool GroundCheck()
    {
        Vector3 dir = new(0, -1);
        if (Physics.Raycast(transform.position, dir, controller.height * 0.5f + 0.1f))
            return true;
        else
            return false;
    }
    IEnumerator UpdateGroundedStatus()
    {
        yield return new WaitForSeconds(1);
        isGrounded = false;
    }

}
