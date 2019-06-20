using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public KeyCode MoveUp = KeyCode.W;
    public KeyCode MoveDown = KeyCode.S;

    public int PlayerNumber;
    public float MoveSpeed = 5;

    public bool AI;

    private Rigidbody2D rb;
    private Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (AI)
            AIControl();
        else
            PlayerInput();
    }

    private void AIControl()
    {
        Vector3 direction = Vector3.zero;

        Transform ball = GetClosestEnemy(GameManager.Instance.activeBalls);

        if (ball != null)
        {
            Vector3 paddleCenter = transform.GetComponent<Collider2D>().bounds.center;
            Vector3 ballCenter = ball.GetComponent<Collider2D>().bounds.center;

            if (ballCenter.y > paddleCenter.y)
            {
                direction.y = 1;
            }
            else if (ballCenter.y < paddleCenter.y)
            {
                direction.y = -1;
            }

            velocity = direction * MoveSpeed;
        }
    }

    Transform GetClosestEnemy(IEnumerable<Transform> balls)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in balls)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    private void PlayerInput()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(MoveUp))
        {
            direction.y = 1;
        }
        else if (Input.GetKey(MoveDown))
        {
            direction.y = -1;
        }

        velocity = direction * MoveSpeed;
    }
}
