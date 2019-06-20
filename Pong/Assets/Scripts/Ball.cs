using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float MoveSpeed = 5;
    public bool IncreaseSpeedOverTime;
    public float increaseSpeedTimer = 3;

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject destroyEffect;
    [SerializeField] AudioEvent audioHitWall;
    [SerializeField] AudioEvent audioHitPlayer;

    private Rigidbody2D rb;
    private Vector3 velocity;
    private float timeActive;
    private TrailRenderer trail;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        timeActive = 0;

        Vector3 direction = Random.insideUnitCircle.normalized;

        velocity = direction * MoveSpeed;
        rb.velocity = velocity;
    }

    private void Update()
    {
        timeActive += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;

        if (IncreaseSpeedOverTime)
        {
            if (increaseSpeedTimer > 0)
            {
                increaseSpeedTimer -= Time.deltaTime;
            }
            else
            {
                velocity *= 1.1f;
                //trail.time *= 1.1f;
                increaseSpeedTimer += 3;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //velocity = -velocity;
        Vector3 reflectDir = Vector3.Reflect(velocity, collision.contacts[0].normal);
        velocity = reflectDir;

        Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);

        if (collision.gameObject.CompareTag("Player"))
        {
            audioHitPlayer.Play(audioSource);
        }
        else
        {
            audioHitWall.Play(audioSource);
        }
    }

    private void OnDestroy()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
    }
}
