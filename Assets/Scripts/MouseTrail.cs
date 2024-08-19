using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]
public class MouseTrail : MonoBehaviour
{
    [SerializeField] GameObject spawnBound;

    TrailRenderer trailRenderer;
    BoxCollider boxCollider;

    bool isMouseDown = false;
    float cameraToSpawn;

    const int LEFT_CLICK = 0;

    void EnableTrail()
    {
        isMouseDown = true;

        trailRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    void DisableTrail()
    {
        isMouseDown = false;

        // Setting the gameObject to inactive will stop Update()
        trailRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    void UpdatePosition()
    {
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraToSpawn);

        transform.position = Camera.main.ScreenToWorldPoint(position);
    }

    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        cameraToSpawn = spawnBound.transform.position.z - Camera.main.transform.position.z;
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
