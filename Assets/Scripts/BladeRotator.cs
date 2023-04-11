using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRotator : MonoBehaviour
{
    private float rotationSpeed = 400.0f; // speed at which the blades should rotate
    //private float rotationDuration = 1.0f; // duration of the rotation animation

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

}

