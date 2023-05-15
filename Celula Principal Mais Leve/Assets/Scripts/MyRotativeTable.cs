/*
using UnityEngine;

public class MyRotativeTable : MonoBehaviour
{
    public float rotationSpeed = 45f;
    public float angle = 45f;
    public float duration = 0.1f;

    
    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Four))
        {
            RotateTable();
        }
    }

    private void RotateTable()
    {
        Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed / duration);

    }
}

*/

/*
using UnityEngine;

public class MyRotativeTable : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 10.0f;
    private float startTime;
    private bool isRotating = false;
    private Quaternion targetRotation;

    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Four))
        {
            isRotating = true;
            startTime = Time.time;
            targetRotation = transform.rotation * Quaternion.Euler(0, 45, 0);
        }

        if (isRotating)
        {
            float journeyLength = Vector3.Distance(transform.position, target.position);
            float distCovered = (Time.time - startTime) * rotationSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, fractionOfJourney);

            if (transform.rotation == targetRotation)
            {
                isRotating = false;
            }
        }
    }
}
*/

using UnityEngine;

public class MyRotativeTable : MonoBehaviour
{
    public float rotationSpeed = 10.0f;
    private float startTime;
    private bool isRotating = false;
    private Quaternion targetRotation;

    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Four))
        {
            isRotating = true;
            startTime = Time.time;
            targetRotation = transform.rotation * Quaternion.Euler(0, 45, 0);
        }

        if (isRotating)
        {
            float distCovered = (Time.time - startTime) * rotationSpeed;
            float fractionOfJourney = distCovered / 45.0f;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, fractionOfJourney);

            if (transform.rotation == targetRotation)
            {
                isRotating = false;
            }
        }
    }
}