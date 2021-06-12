using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Class that manages the asteroids
/// </summary>
public class Asteroid : MonoBehaviour
{
    /// <summary>
    /// Full mesh of the asteroid
    /// </summary>
    [SerializeField] private GameObject asteroidMesh;
    /// <summary>
    /// Fractured mesh of the asteroid
    /// </summary>
    [SerializeField] private GameObject asteroidFracture;
    /// <summary>
    /// Asteroid drop
    /// </summary>
    [SerializeField] private GameObject asteroidDrop;

    private float maxDistance = 500f;
    private Collider[] fracturedColliders;
    private Renderer[] fracturedRenderers;
    private Transform[] fracturedInitialTransforms;
    private Rigidbody[] fracturedRigidbody;

    /// <summary>
    /// Struct containing position and rotation of a game object
    /// </summary>
    struct TransformStruct
    {
        public string name;
        public Vector3 localPos;
        public Quaternion localRot;
    }

    /// <summary>
    /// List that keep tracks of the initial transform of the fractured pieces of the asteroid
    /// </summary>
    List<TransformStruct> transformList = new List<TransformStruct>();

    // Start is called before the first frame update
    void Start()
    {
        fracturedColliders = asteroidFracture.GetComponentsInChildren<Collider>();
        fracturedRenderers = asteroidFracture.GetComponentsInChildren<Renderer>();
        fracturedInitialTransforms = asteroidFracture.GetComponentsInChildren<Transform>();
        fracturedRigidbody = asteroidFracture.GetComponentsInChildren<Rigidbody>();
        
        // i save the initial transform of the fractured pieces to put them back together after exploding them
        foreach (Transform trans in fracturedInitialTransforms)
        {
            TransformStruct ts = new TransformStruct();
            ts.name = trans.name;
            ts.localPos = trans.localPosition;
            ts.localRot = trans.localRotation;
            transformList.Add(ts);
        }

        StartCoroutine(CheckDistanceFromPlayer());
    }

    private void Update()
    {
        try
        {
            // rotate a little the asteroid
            GetComponentInChildren<Rigidbody>().angularVelocity = UnityEngine.Random.insideUnitSphere * 0.2f;
        }
        catch (NullReferenceException)
        {
            // don't do anything if the asteroid is not active
        }
        
    }

    private void OnEnable()
    {
        asteroidMesh.SetActive(true);

        // enable again the asteroid collider
        gameObject.GetComponent<Collider>().enabled = true;

        // enable again the fractured colliders needed for the explosion force
        if (fracturedColliders != null)
        {
            foreach (Collider fracture in fracturedColliders)
            {
                fracture.enabled = true;
            }
        }

        // reset the color alpha to 1
        if (fracturedRenderers != null)
        {
            foreach (Renderer rend in fracturedRenderers)
            {
                rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, 1);
            }
        }


        asteroidFracture.SetActive(false);
        asteroidDrop.SetActive(false);
    }

    /// <summary>
    /// Check if the asteroid is hit by a shot
    /// </summary>
    /// <param name="collision">What it hit</param>
    private void OnCollisionEnter(Collision collision)
    {
         if (collision.gameObject.CompareTag(Game.shotTag))
         {
            asteroidMesh.SetActive(false);
            asteroidFracture.SetActive(true);
            asteroidDrop.SetActive(true);

            Vector3 explosionPos = gameObject.transform.position;
            float radius = 20f;
            float power = 1500f;
            //Collider[] explosionColliders = Physics.OverlapSphere(explosionPos, radius);
            Collider[] fracturedColliders = asteroidFracture.GetComponentsInChildren<Collider>();
            foreach (Collider hit in fracturedColliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius);

                hit.enabled = false;
                
            }

            StartCoroutine(FadeOutFractures());

            // i also disable the asteroid collider
            gameObject.GetComponent<Collider>().enabled = false;
        }
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
    /// Fade out the fractured asteroid over time and reset the pieces together afterwards
    /// </summary>
    public IEnumerator FadeOutFractures()
    {
        while (asteroidFracture.GetComponentsInChildren<Renderer>()[0].material.color.a > 0)
        {
            foreach (Renderer rend in asteroidFracture.GetComponentsInChildren<Renderer>())
            {
                Color fractureColor = rend.material.color;
                float fadeAmount = fractureColor.a - (0.5f * Time.deltaTime);

                fractureColor = new Color(fractureColor.r, fractureColor.g, fractureColor.b, fadeAmount);
                rend.material.color = fractureColor;               
            }

            yield return null;
        }

        if (fracturedRigidbody != null)
        {
            foreach (Rigidbody rb in fracturedRigidbody)
            {
                rb.velocity = rb.angularVelocity = Vector3.zero;
            }
        }

        foreach (Transform trans in asteroidFracture.GetComponentsInChildren<Transform>())
        {
            trans.localPosition = transformList.First(x => x.name == trans.name).localPos;
            trans.localRotation = transformList.First(x => x.name == trans.name).localRot;
        }

        // when it's fadeout i can disable the whole object
        asteroidFracture.SetActive(false);
    }
}
