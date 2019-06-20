using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [Range(1, 2)]
    public int PlayerGoal;

    [SerializeField] AudioEvent audioGoal;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            GameManager.Instance.Goal(PlayerGoal, collision.gameObject);

            audioGoal.Play(audioSource);
        }
    }
}
