using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrollCamera : MonoBehaviour
{
    [SerializeField] private float cmMaxDistance = 75f;
    [SerializeField] private float cmMinDistance = 10f;
    [SerializeField] private float zoomAcceleration = 2.5f;
    [SerializeField] private float zoomSpeed = 1f;

    private float current_cmDistance = 0f;
    private float new_cmDistance = 0f;

    private CinemachineFramingTransposer transposer;
    private PlayerInput input;
    private InputAction scroll;

    private void Awake()
    {
        transposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<PlayerInput>();

        scroll = input.actions["Zoom"];
        transposer.m_CameraDistance = cmMaxDistance;

        scroll.performed += cntxt => HandleZoom(cntxt.ReadValue<float>());
        scroll.canceled += cntxt => HandleZoom(0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateZoomLevel();
    }

    private void UpdateZoomLevel()
    {
        // Interpolate between current and target distance
        if (Mathf.Approximately(current_cmDistance, new_cmDistance))
            return;

        current_cmDistance = Mathf.Lerp(current_cmDistance, new_cmDistance, zoomAcceleration * Time.deltaTime);
        transposer.m_CameraDistance = Mathf.Clamp(current_cmDistance, cmMinDistance, cmMaxDistance);
    }

    private void HandleZoom(float zoomInput)
    {
        // Adjust the new zoom distance based on the input value
        if (zoomInput != 0)
        {
            new_cmDistance = current_cmDistance - zoomInput * zoomSpeed;
        }
        else
        {
            // Optional: Reset zoom distance if input is canceled
            new_cmDistance = current_cmDistance;
        }
    }
}