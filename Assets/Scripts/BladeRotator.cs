using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRotator : MonoBehaviour
{
    public float rotationSpeed = 7.0f; // speed at which the blades should rotate
    private float rotationDuration = 1.0f; // duration of the rotation animation

    void Start()
    {
        iTween.RotateBy(gameObject, iTween.Hash(
            "z", -1.0f, // rotate around the z-axis by 1 full rotation (360 degrees)
            "time", rotationDuration,
            "easetype", iTween.EaseType.easeInOutSine,
            "looptype", iTween.LoopType.loop,
            "delay", 0.0f
        ));
    }

    
}

