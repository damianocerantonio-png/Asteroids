using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
   
    public float buffer = 1.0f; 

    private float leftConstraint;
    private float rightConstraint;
    private float bottomConstraint;
    private float topConstraint;
    private Camera cam;
    private float distanceZ;

    void Start()
    {
        cam = Camera.main;
        distanceZ = Mathf.Abs(cam.transform.position.z - transform.position.z);

       
        leftConstraint = cam.ScreenToWorldPoint(new Vector3(0, 0, distanceZ)).x;
        rightConstraint = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, distanceZ)).x;
        bottomConstraint = cam.ScreenToWorldPoint(new Vector3(0, 0, distanceZ)).y;
        topConstraint = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, distanceZ)).y;
    }

    void Update()
    {
        // Controlliamo se l'oggetto è uscito dai bordi e lo spostiamo dall'altra parte
        
        // Uscito a Sinistra -> Vai a Destra
        if (transform.position.x < leftConstraint - buffer)
        {
            transform.position = new Vector3(rightConstraint + buffer, transform.position.y, transform.position.z);
        }
        
        // Uscito a Destra -> Vai a Sinistra
        if (transform.position.x > rightConstraint + buffer)
        {
            transform.position = new Vector3(leftConstraint - buffer, transform.position.y, transform.position.z);
        }

        // Uscito Sotto -> Vai Sopra
        if (transform.position.y < bottomConstraint - buffer)
        {
            transform.position = new Vector3(transform.position.x, topConstraint + buffer, transform.position.z);
        }

        // Uscito Sopra -> Vai Sotto
        if (transform.position.y > topConstraint + buffer)
        {
            transform.position = new Vector3(transform.position.x, bottomConstraint - buffer, transform.position.z);
        }
    }
}
