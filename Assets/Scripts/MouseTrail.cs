using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]
public class MouseTrail : MonoBehaviour
{
    const int LEFT_CLICK = 0;

    private TrailRenderer trailRenderer;
    private BoxCollider boxCollider;

    private bool isMouseDown = false;

    // Start is called before the first frame update
    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void EnableTrail()
    {
        isMouseDown = true;
        trailRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    void DisableTrail()
    {
        isMouseDown = false;
        trailRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    void UpdatePosition()
    {
        Vector3 position = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            transform.position.z - Camera.main.transform.position.z
        );

        gameObject.transform.position = Camera.main.ScreenToWorldPoint(position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(LEFT_CLICK))
        {
            EnableTrail();
        }

        if (Input.GetMouseButtonUp(LEFT_CLICK))
        {
            DisableTrail();
        }

        if (isMouseDown)
        {
            UpdatePosition();
        }
    }
}
