using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseOverEvent : MonoBehaviour
{
    [SerializeField]
    private LayerMask targetLayer; // The layer you want to detect

    private Camera mainCamera;
    private Mouse mouse;
    private GameObject currentCountry;
    private GameObject lastCountry;

    public delegate void SelectingCountryOnMap(GameObject country);
    public event SelectingCountryOnMap SelectingCountryOnMapCallBack;

    public delegate void ExitingCountryOnMap(GameObject country);
    public event ExitingCountryOnMap ExitingCountryOnMapCallBack;

    void Start()
    {
        // Cache the main camera reference for efficiency
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Ensure your camera is tagged as 'MainCamera'.");
            enabled = false; // Disable this script
            return;
        }

        // Get a reference to the Mouse device
        mouse = Mouse.current;

        if (mouse == null)
        {
            Debug.LogError("Mouse device not detected! Make sure the new Input System is set up correctly.");
            enabled = false; // Disable this script
        }
    }

    void Update()
    {
        DetectObjectUnderMouse();
    }

    void DetectObjectUnderMouse()
    {
        // Get the current mouse position
        Vector2 mousePosition = mouse.position.ReadValue();

        // Create a ray from the camera to the mouse position
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        // Debug: Draw the ray in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 0.1f); // Adjust the length and duration as needed

        // Perform the raycast and get all hits
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, targetLayer);

        // Sort hits by distance
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        if (hits.Length > 0)
        {
            GameObject closestObject = hits[0].collider.gameObject;

            // If the current object is different from the last object, invoke the events
            if (closestObject != lastCountry)
            {
                // If we had a previously selected country, trigger the exit event
                if (lastCountry != null)
                {
                    ExitingCountryOnMapCallBack?.Invoke(lastCountry);
                }

                // Trigger the selection event for the new closest country
                SelectingCountryOnMapCallBack?.Invoke(closestObject);
            }

            // Update lastCountry to the closest one
            lastCountry = closestObject;
        }
        else
        {
            // If the raycast doesn't hit anything and there was a previously selected country
            if (lastCountry != null)
            {
                // Trigger the exit event since the mouse has moved to empty space
                ExitingCountryOnMapCallBack?.Invoke(lastCountry);
                lastCountry = null; // Clear the last country
            }

            currentCountry = null; // No country selected since the mouse is over empty space
        }
    }

    public GameObject SelectedCountry()
    {
        return currentCountry;
    }
}
