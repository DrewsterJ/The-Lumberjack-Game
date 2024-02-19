using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBlade : MonoBehaviour
{
    public bool rotate;
    public float rotationSpeed = 2.0f;

    private void Start()
    {
        Debug.Assert(rotationSpeed != 0.0f);
    }

    private void FixedUpdate()
    {
        if (rotate)
            transform.Rotate(0.0f, 0.0f, rotationSpeed);
    }
}
