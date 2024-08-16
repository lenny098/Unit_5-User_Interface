using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] bool isBadTarget;
    [SerializeField] ParticleSystem explosionParticle;

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
