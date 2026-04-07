using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed = 5f;
    public float maxAngularVelocity = 50f;

    public enum Type { Big, Medium, Small }
    public Type asteroidType;

    public GameObject explosion;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.AddForce(Random.insideUnitCircle.normalized * speed);
        rb.angularVelocity = Random.Range(-maxAngularVelocity, maxAngularVelocity);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            
            switch (asteroidType)
            {
                case Type.Big:
                    GameManager.Instance.AddScore(20);
                    GameManager.Instance.SpawnAsteroid(transform.position, Type.Medium);
                    GameManager.Instance.SpawnAsteroid(transform.position, Type.Medium);
                    break;

                case Type.Medium:
                    GameManager.Instance.AddScore(50);
                    GameManager.Instance.SpawnAsteroid(transform.position, Type.Small);
                    GameManager.Instance.SpawnAsteroid(transform.position, Type.Small);
                    break;

                case Type.Small:
                    GameManager.Instance.AddScore(100);
                    break;
            }

           
            GameManager.Instance.RemoveAsteroid(gameObject);

            Destroy(gameObject);
            Destroy(collider.gameObject);

            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}