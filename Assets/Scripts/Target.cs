using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody rigidbody;
    private GameManager gameManager;
    public ParticleSystem explosionParticle;

    private float initialForceMin = 12;
    private float initialForceMax = 16;
    private float torqueRange = 10;

    private float xRange = 4;
    private float yPosition = -2;

    public bool isBadTarget;

    Vector3 RandomUpwardForce()
    {
        return Vector3.up * Random.Range(initialForceMin, initialForceMax);
    }

    float RandomAxisTorque()
    {
        return Random.Range(-torqueRange, torqueRange);
    }

    Vector3 RandomTorque()
    {
        return new Vector3(RandomAxisTorque(), RandomAxisTorque(), RandomAxisTorque());
    }

    Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-xRange, xRange), yPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        rigidbody.AddForce(RandomUpwardForce(), ForceMode.Impulse);
        rigidbody.AddTorque(RandomTorque(), ForceMode.Impulse);
        transform.position = RandomPosition();
    }

    void DestroyWithExplosion()
    {
        Destroy(gameObject);

        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameOver)
        {
            DestroyWithExplosion();
        }
    }

    void HandleCollision()
    {
        if (gameManager.IsGamePaused) { return; }

        DestroyWithExplosion();

        if (isBadTarget)
        {
            gameManager.BadTargetDestroyed();
        }
        else
        {
            gameManager.GoodTargetDestroyed();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        switch (other.tag)
        {
            case "Mouse":
                HandleCollision();

                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if (!isBadTarget)
        {
            gameManager.DeductLives();
        }
    }
}
