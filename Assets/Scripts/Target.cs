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

    public void DestroyByGameOver()
    {
        DestroyWithExplosion();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
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
        switch (other.gameObject.tag)
        {
            case "Sensor":
                DestroyByBound();

                break;
            default:
                break;
        }
    }
}
