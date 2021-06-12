using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages the laser shot from the player airship
/// </summary>
public class Shot : MonoBehaviour
{
    private Rigidbody rb;
    /// <summary>
    /// Speed of the shot
    /// </summary>
    private float shotSpeed = 100;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (rb != null)
        {
            rb.AddForce(transform.forward * shotSpeed, ForceMode.Impulse);

            StartCoroutine(DeactivateAfterTime());
        }
    }

    /// <summary>
    /// Check if the shot hit the asteroid
    /// </summary>
    /// <param name="collision">What i hit</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Game.asteroidTag))
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Deactivate the shot after a fixed time
    /// </summary>
    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(5);

        gameObject.SetActive(false);
    }
}