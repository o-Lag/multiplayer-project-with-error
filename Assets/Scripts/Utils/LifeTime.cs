using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] private float lifetimeValue = 5;

    private void Start()
    {
        Destroy(gameObject, lifetimeValue);
    }
}
