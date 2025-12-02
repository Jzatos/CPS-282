using UnityEngine;

public class Door : MonoBehaviour
{

        void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Collision Detected");
            // only do stuff if collided with the Player
            if (other.gameObject.tag == "Player")
            {
                // if game manager exists, make adjustments based on target properties
                if (GameManager.gm)
                {
                    GameManager.gm.NextLevel();
                }

            }
        }
}
