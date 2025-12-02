using UnityEngine;
using System.Collections;

public class DestroyableByPlayer : MonoBehaviour
{
	// when collided with another gameObject
	void OnCollisionEnter2D(Collision2D other)
	{
		// if hit by a projectile: collect score
		if (other.gameObject.CompareTag("Player"))
		{
			// destroy self
			Destroy(gameObject);
		}
	}
}
