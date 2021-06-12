using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages the ice asteroids
/// </summary>
public class Asteroid_Ice : MonoBehaviour
{
    private float maxDistance = 500f;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 0.2f;
    }

    /// <summary>
    /// Keep checking the distance from the asteroid to the player.
    /// If it's too big, deactivate this asteroid and spawn a new one closer to the player
    /// </summary>
    public IEnumerator CheckDistanceFromPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        if (gameObject.activeInHierarchy)
        {
            float distance = Vector3.Distance(gameObject.transform.position, PlayerController.GetPlayerPosition());

            // if i'm too far from the asteroid i deactivate it, so i can spawn a new one closer to the player
            if (distance > maxDistance)
            {
                gameObject.SetActive(false);
                Game.SpawnAsteroid();
            }

            StartCoroutine(CheckDistanceFromPlayer());
        }
    }

    /// <summary>
    /// Check if the asteroid is hit by a shot
    /// </summary>
    /// <param name="collision">What it hit</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Game.shotTag))
        {
            gameObject.SetActive(false);

            Game.SpawnAsteroid();
        }
    }
}
