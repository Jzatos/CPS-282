using UnityEngine;
using System.Collections;

public class TargetBehavior : MonoBehaviour
{

	// target impact on game
	public int scoreAmount = 0;
	public float timeAmount = 0.0f;

	// explosion when hit?
	public GameObject explosionPrefab;
	// SFX when hit?
	public AudioClip soundEffect;
	// when collided with another gameObject
	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Collision Detected");
		// only do stuff if collided with the Player
		if (other.gameObject.tag == "Player")
		{
			if (explosionPrefab)
			{
				// Instantiate an explosion effect at the gameObjects position and rotation
				Instantiate(explosionPrefab, transform.position, transform.rotation);
			}
			if (soundEffect)
			{ // if the SFX is provided, play it
				Debug.Log("Played the SFX");
				AudioSource.PlayClipAtPoint(soundEffect, this.gameObject.transform.position); ;
			}
			// if game manager exists, make adjustments based on target properties
			if (GameManager.gm)
			{
				GameManager.gm.targetHit(scoreAmount, timeAmount);
			}

			// destroy self
			Destroy(gameObject);
		}
	}

	// when collided with another gameObject
	void OnCollisionEnter (Collision newCollision)
	{	
		// only do stuff if collided with the Player
		if (newCollision.gameObject.tag == "Player") {
			if (explosionPrefab) {
				// Instantiate an explosion effect at the gameObjects position and rotation
				Instantiate (explosionPrefab, transform.position, transform.rotation);
			}
			if (soundEffect){ // if the SFX is provided, play it
				Debug.Log("Played the SFX");
				AudioSource.PlayClipAtPoint(soundEffect, this.gameObject.transform.position); ;
			}
			// if game manager exists, make adjustments based on target properties
			if (GameManager.gm) {
				GameManager.gm.targetHit (scoreAmount, timeAmount);
			}
						
			// destroy self
			Destroy (gameObject);
		}
	}


}
