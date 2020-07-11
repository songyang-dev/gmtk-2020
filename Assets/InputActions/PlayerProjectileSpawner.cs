using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileSpawner : MonoBehaviour
{
    public Animator anim;

    [Header("Spawner Settings")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    
    public float spawnRate;
    private float timer;

    [Header("Particles")]
    public ParticleSystem spawnParticles;

    [Header("Audio")]
    public AudioSource spawnAudioSource;

    PlayerInteraction inputActions;

    Vector2 lookPosition;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        inputActions = new PlayerInteraction();
        inputActions.Player.Fire.performed += ctx => lookPosition = ctx.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(lookPosition.magnitude > 0.1f)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if(timer < spawnRate) { return; }
        timer = 0f;
        Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        if (spawnParticles)
        {
            //spawnParticles.Play();
        }

        if (spawnAudioSource)
        {
            //spawnAudioSource.Play();
        }
    }
}
