using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float TimeToDestroy = 3;

    void Start()
    {
        Destroy(gameObject, TimeToDestroy);
    }
}
