using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyController : MonoBehaviour
{
    public GameObject camera1;

    public const float WALK_SPEED = 2.5f;

    // Update is called once per frame
    void Update()
    {
        // returns a Vector2 of the primary (typically the Left) thumbstick’s current state.
        // (X/Y range of -1.0f to 1.0f)
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 movement = camera1.transform.TransformDirection(input.x, 0, input.y);
        movement.y = 0;
        movement = movement.magnitude == 0 ? Vector3.zero : movement/movement.magnitude;
        movement *= Time.deltaTime * WALK_SPEED;
        this.transform.Translate(movement);
    }
}
