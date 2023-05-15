using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCanvasFollowCamera : MonoBehaviour
{
    public Transform target;
    public Camera centerCamera;
    public float smoothSpeed = 3.5f;
    public Vector3 offset = new Vector3(0.0f, 0.0f, 2.5f);

    // Update is called once per frame
    void Update()
    {
        Vector3 lookPos = target.position - centerCamera.transform.position;
        lookPos.y = 0;

        Quaternion desiredRotation = Quaternion.LookRotation(lookPos);
        target.rotation = desiredRotation;
        //target.rotation = Quaternion.Slerp(target.rotation, desiredRotation, smoothSpeed * Time.deltaTime);

        Vector3 resultingPosition = centerCamera.transform.position + centerCamera.transform.forward * 3f;
        Vector3 desiredPosition = new Vector3(resultingPosition.x, 1.5f, resultingPosition.z);
        target.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed/100);

        /*
        //Outra maneira (que deu errado)
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        

        //transform.LookAt(target);

        Quaternion desiredRotation = target.rotation;
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeed * Time.deltaTime);
        transform.rotation = smoothedRotation;
        
        transform.RotateAround(target.transform.position,
                               transform.right,
                               -desiredPosition.y * smoothSpeed);
        */

    }
}