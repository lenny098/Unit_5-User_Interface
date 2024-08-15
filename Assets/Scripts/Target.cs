using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{
    float xRange = 4;
    float yPosition = -2;

    float initialForceMin = 12;
    float initialForceMax = 16;
    float torqueRange = 10;

    [SerializeField] bool isBadTarget;
    [SerializeField] ParticleSystem explosionParticle;

    Rigidbody rigidbody;

    Vector3 RandomUpwardForce()
    {
        return Vector3.up * Random.Range(initialForceMin, initialForceMax);
    }

    Vector3 RandomTorque()
    {
        float RandomAxisTorque(){ return Random.Range(-torqueRange, torqueRange); }

        return new Vector3(RandomAxisTorque(), RandomAxisTorque(), RandomAxisTorque());
    }

    Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-xRange, xRange), yPosition);
    }

    void DestroyWithExplosion()
    {
        Destroy(gameObject);

        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
    }

    void DestroyByGameOver()
    {
        DestroyWithExplosion();
    }

    void DestroyByMouse()
    {
        if (GameManager.Instance.IsGamePaused) { return; }

        DestroyWithExplosion();

        if (isBadTarget)
        {
            GameManager.Instance.BadTargetDestroyed();
        }
        else
        {
            GameManager.Instance.GoodTargetDestroyed();
        }

    }

    void DestroyByBound()
    {
        Destroy(gameObject);

        if (!isBadTarget)
        {
            GameManager.Instance.GoodTargetMissed();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.AddForce(RandomUpwardForce(), ForceMode.Impulse);
        rigidbody.AddTorque(RandomTorque(), ForceMode.Impulse);

        transform.position = RandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            DestroyByGameOver();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        switch (other.tag)
        {
            case "Mouse":
                DestroyByMouse();

                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyByBound();
    }
}
