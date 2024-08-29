using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOFollowsMouse : MonoBehaviour
{
    public Transform ground;
    public float movementThreshold = 0.25f;  // The minimum mouse movement required to move the object

    private Vector3 lastMousePosition;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        float distance = Vector3.Distance(currentMousePosition, lastMousePosition);

        if (distance >= movementThreshold)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(currentMousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == ground)
                {
                    transform.position = hit.point;
                }
            }

            lastMousePosition = currentMousePosition;
        }
    }
}
