using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that manages the Player
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float normalSpeed = 25f;
    public float accelerationSpeed = 45f;
    /// <summary>
    /// Position of the camera attached to the player
    /// </summary>
    public Transform cameraPosition;
    public Camera mainCamera;
    public Transform spaceshipRoot;
    public float rotationSpeed = 2.0f;
    public float cameraSmooth = 4f;
    public RectTransform crosshairTexture;
    public bool canShoot = true;
    /// <summary>
    /// Position where the shot will spawn from
    /// </summary>
    public Transform shotSpawn;
    /// <summary>
    /// Slider for the player's "health"
    /// </summary>
    public Slider fuelBar;
    public Gradient fuelGradient;
    public Image fillImage;
    public Text distanceText;
    public GameObject laserRay;
    public GameObject thrusters;
    public GameObject thrusterBoosts;

    private float speed;
    private Rigidbody rb;
    private Quaternion lookRotation;
    private float rotationZ = 0;
    private float mouseXSmooth = 0;
    private float mouseYSmooth = 0;
    private Vector3 defaultShipRotation;

    private float currentFuel;
    private float maxFuel = 100;

    /// <summary>
    /// Animator of the player
    /// </summary>
    private Animator animator;  

    private static PlayerController instance;

    private void Awake()
    {
        instance = this;

        animator = GetComponent<Animator>();       
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning(name + ": PlayerController has no rigidbody!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.useGravity = false;
        lookRotation = transform.rotation;
        defaultShipRotation = spaceshipRoot.localEulerAngles;
        rotationZ = defaultShipRotation.z;

        fuelBar.maxValue = maxFuel;
        currentFuel = maxFuel;
        fuelBar.value = currentFuel;
        fillImage.color = fuelGradient.Evaluate(fuelBar.normalizedValue);

        thrusterBoosts.SetActive(false);
    }

    private void Update()
    {
        // SHOT
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            SpawnShot();
        }

        // ROLL ANIMATION
        try
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    try
                    {
                        animator.SetTrigger("Clockwise");
                    }
                    catch (Exception)
                    {
                        Debug.LogError(name + ": PlayerController has no animator!");
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    try
                    {
                        animator.SetTrigger("AntiClockwise");
                    }
                    catch (Exception)
                    {
                        Debug.LogError(name + ": PlayerController has no animator!");
                    }
                }
            }
        }
        catch (Exception)
        {
            Debug.LogError(name + ": PlayerController has no animator!");
        }

        // keep decreasing the fuel over time
        currentFuel -= (2 * Time.deltaTime) ;
        SetFuelBar(currentFuel);

        // if the fuel reaches 0 is gameover
        if (currentFuel <= 0)
        {
            Game.GameOver();
        }
    }

    void FixedUpdate()
    {
        // Press LeftShift to accelerate
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = Mathf.Lerp(speed, accelerationSpeed, Time.deltaTime * 3);

            thrusterBoosts.SetActive(true);
        }
        else
        {
            speed = Mathf.Lerp(speed, normalSpeed, Time.deltaTime * 10);

            thrusterBoosts.SetActive(false);
        }

        // Set moveDirection to the vertical axis (up and down keys) * speed
        Vector3 moveDirection = new Vector3(0, 0, speed);
        // Transform the vector3 to local space
        moveDirection = transform.TransformDirection(moveDirection);
        // Set the velocity, so you can move
        rb.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

        // Camera follow
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition.position, Time.deltaTime * cameraSmooth);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, cameraPosition.rotation, Time.deltaTime * cameraSmooth);

        // Rotation
        float rotationZTmp = 0;
        if (Input.GetKey(KeyCode.A))
        {
            rotationZTmp = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationZTmp = -1;
        }
        mouseXSmooth = Mathf.Lerp(mouseXSmooth, Input.GetAxis("Mouse X") * rotationSpeed, Time.deltaTime * cameraSmooth);
        mouseYSmooth = Mathf.Lerp(mouseYSmooth, Input.GetAxis("Mouse Y") * rotationSpeed, Time.deltaTime * cameraSmooth);
        Quaternion localRotation = Quaternion.Euler(-mouseYSmooth, mouseXSmooth, rotationZTmp * rotationSpeed);
        lookRotation *= localRotation;
        transform.rotation = lookRotation;
        rotationZ -= mouseXSmooth;
        rotationZ = Mathf.Clamp(rotationZ, -45, 45);
        spaceshipRoot.transform.localEulerAngles = new Vector3(defaultShipRotation.x, defaultShipRotation.y, rotationZ);
        rotationZ = Mathf.Lerp(rotationZ, defaultShipRotation.z, Time.deltaTime * cameraSmooth);

        ////Update crosshair texture
        //if (crosshairTexture)
        //{
        //    crosshairTexture.position = mainCamera.WorldToScreenPoint(transform.position + transform.forward * 100);
        //}

        // Show the distance if I hit an object with a forward raycast
        if (Physics.Raycast(shotSpawn.transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            //Debug.DrawRay(shotSpawn.transform.position, transform.forward * hit.distance, Color.yellow);
            distanceText.text = "DST: " + Vector3.Distance(shotSpawn.position, hit.point).ToString("0.00");
            laserRay.GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 235, 0, 255));
            Light[] laserLights = laserRay.GetComponentsInChildren<Light>();
            foreach (Light light in laserLights)
            {
                light.color = new Color32(255, 235, 0, 255);
            }
        }
        else
        {
            //Debug.DrawRay(shotSpawn.transform.position, transform.forward * 1000, Color.red);
            distanceText.text = "";
            laserRay.GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 10, 0, 255));
            Light[] laserLights = laserRay.GetComponentsInChildren<Light>();
            foreach (Light light in laserLights)
            {
                light.color = new Color32(255, 10, 0, 255);
            }
        }
    }

    /// <summary>
    /// Spawn the airship shot if I didn't ran out of ammos
    /// </summary>
    private void SpawnShot()
    {
        GameObject newShot = SpawnPool.instance.GetPoolObject(Game.shotTag);
        if (newShot != null)
        {
            newShot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            newShot.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            newShot.transform.forward = transform.forward;
            newShot.transform.position = shotSpawn.position;
            newShot.transform.rotation = shotSpawn.rotation;
            newShot.SetActive(true);
        }
        else
        {
            Debug.LogError("No Ammo!!");
        }
    }

    /// <summary>
    /// Check if hit an asteroid
    /// </summary>
    /// <param name="collision">The object I'm colliding with</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Game.asteroidTag))
        {
            Game.GameOver();
        }
    }

    /// <summary>
    /// Check if I enter the trigger to pick up the collectible orb
    /// </summary>
    /// <param name="collision">The object I'm colliding with</param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(Game.asteroidDropTag))
        {
            // pick up the drop
            currentFuel = (currentFuel + 10) <= 100 ? currentFuel + 10 : 100;
            SetFuelBar(currentFuel);

            // set the drop inactive and the whole asteroid
            collision.gameObject.SetActive(false);
            StartCoroutine(DelayedDeactivatationAsteroid(collision));          
        }
    }

    /// <summary>
    /// Deactivate the asteroid after a short time and spawn a new one
    /// </summary>
    private IEnumerator DelayedDeactivatationAsteroid(Collider _collision)
    {
        yield return new WaitForSeconds(3f);

        _collision.gameObject.transform.parent.gameObject.SetActive(false);

        // set active a new asteroid
        Game.SpawnAsteroid();
    }

    /// <summary>
    /// Updates the FuelBar of the player
    /// </summary>
    /// <param name="_fuelLevel"></param>
    private void SetFuelBar(float _fuelLevel)
    {
        fuelBar.value = _fuelLevel;
        fillImage.color = fuelGradient.Evaluate(fuelBar.normalizedValue);
    }

    /// <summary>
    /// Returns the player's position
    /// </summary>
    public static Vector3 GetPlayerPosition()
    {
        return instance.transform.position;
    }
}