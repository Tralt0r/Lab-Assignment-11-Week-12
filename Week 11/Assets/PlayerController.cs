using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player speed and cam ref
    public float speed = 8f;
    private Camera cam;

    void Start()
    { 
        //Set camera
        cam = Camera.main;
    }

    void Update()
    {
        //Get player input
        float input = Input.GetAxisRaw("Horizontal");
        Vector3 position = transform.position;
        position.x += input * speed * Time.deltaTime; 

        // Camera bounds from the orthographic size and the aspect ratio
        float halfW = cam.orthographicSize * cam.aspect;
        //Left edge of the screen
        float minX = cam.transform.position.x - halfW;
        //Right edge of the screen
        float maxX = cam.transform.position.x + halfW;
        //Don't let the player go past edge
        position.x = Mathf.Clamp(position.x, minX, maxX);
        //Move the player to new spot
        transform.position = position;
    }
}