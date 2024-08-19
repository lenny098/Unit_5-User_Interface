using UnityEngine;

public class MouseTrail : MonoBehaviour
{
    [SerializeField] GameObject spawnBound;

    float spawnToCamera;

    void UpdatePosition()
    {
        transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, spawnToCamera)
        );
    }

    void Awake()
    {
        spawnToCamera = spawnBound.transform.position.z - Camera.main.transform.position.z;
    }

    private void OnEnable()
    {
        UpdatePosition(); // Prevent a trail from the last active position to current position
    }

    // Update is called once per frame, only if GameObject is active
    void Update()
    {
        UpdatePosition();
    }
}
