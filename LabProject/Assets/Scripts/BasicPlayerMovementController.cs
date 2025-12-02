using UnityEngine;

public class BasicPlayerMovementController : MonoBehaviour
{
    public float speed = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float move_x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float move_y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(move_x, move_y, 0);
    }
}
