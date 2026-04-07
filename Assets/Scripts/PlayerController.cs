using Unity.VisualScripting;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    [Header("Impostazioni Movimento")]
    public float forwardSpeed = 5.0f;   
    public float turnSpeed = 200.0f;   

    private Rigidbody2D rb;
    private float forwardInput;
    private float turnInput;

    public GameObject bulletPrefab; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        
        forwardInput = Input.GetAxis("Vertical"); 

       
        turnInput = Input.GetAxis("Horizontal"); 

       if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            TeleportRandomly();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
           Shoot();
        }
    }


    void Shoot()
    {
       
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        Debug.Log("Sparo!");
    }
    
    void FixedUpdate()
    {
        
        if (turnInput != 0)
        {
          
            float rotationAmount = -turnInput * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation + rotationAmount);
        }

        if (forwardInput > 0)
        {
         
            rb.AddForce(transform.up * forwardSpeed * forwardInput);
        }
       
    }

    
    void TeleportRandomly()
    {
        
        float distanceZ = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        Vector2 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distanceZ));
        Vector2 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distanceZ));

        
        float randomX = Random.Range(screenBottomLeft.x + 1f, screenTopRight.x - 1f);
        float randomY = Random.Range(screenBottomLeft.y + 1f, screenTopRight.y - 1f);

      
        transform.position = new Vector3(randomX, randomY, 0);

        
         rb.linearVelocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Asteroid") || 
            collider.gameObject.CompareTag("EnemyShip") || 
            collider.gameObject.CompareTag("EnemyBullet"))
        {
            GameManager.Instance.LoseLife();
            transform.position = Vector3.zero;
        }
    }
}