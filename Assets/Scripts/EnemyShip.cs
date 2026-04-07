using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed = 2f;
    public float changeDirectionInterval = 3f;

    private float changeDirectionTimer = 0f;
    private Vector2 currentDirection;

    public float shootInterval = 2f;
    private float shootTimer = 0f;

    public GameObject bulletPrefab;
    public GameObject explosion;

    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player NON trovato!");

        ChooseNewDirection();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = currentDirection * speed;

        changeDirectionTimer -= Time.fixedDeltaTime;
        if (changeDirectionTimer <= 0f)
        {
            ChooseNewDirection();
        }
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    void ChooseNewDirection()
    {
        float randomAngle = Random.Range(0f, 360f);

        currentDirection = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad),
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
        );

        changeDirectionTimer = changeDirectionInterval;
    }

    void ShootAtPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        
        float spread = 15f;
        float randomAngle = Random.Range(-spread, spread);
        Quaternion finalRotation = rotation * Quaternion.Euler(0, 0, randomAngle);

        Instantiate(bulletPrefab, transform.position, finalRotation);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            GameManager.Instance.AddScore(200);

            Destroy(gameObject);
            Destroy(collider.gameObject);

            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}