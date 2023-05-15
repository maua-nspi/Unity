using UnityEngine;

public class MyPickAndPlace : MonoBehaviour
{
    public Transform gripper;
    public Transform objectToPick;
    public Transform objectToPickCenter;
    public Transform objectPlacement;
    public float distanceThreshold = 0.02f;

    private bool gripperOpen = true;
    private bool objectHeld = false;

    private void Update()
    {
        float distance = Vector3.Distance(gripper.transform.position, objectToPickCenter.transform.position);

        if(OVRInput.GetDown(OVRInput.Button.Two))
        {
            if (distance <= distanceThreshold)
                ToggleGripper();
        }
    }

    private void ToggleGripper()
    {
        gripperOpen = !gripperOpen;

        if (gripperOpen)
        {
            if (objectHeld)
            {
                PlaceObject();
            }
        }
        else
        {
            if (!objectHeld)
            {
                PickUpObject();
            }
        }
    }

    private void PickUpObject()
    {
        objectToPick.GetComponent<Rigidbody>().useGravity = false;
        objectToPick.GetComponent<Rigidbody>().isKinematic = true;
        objectToPick.SetParent(gripper);
        objectToPick.localPosition = Vector3.zero;
        objectHeld = true;
    }

    private void PlaceObject()
    {
        objectToPick.GetComponent<Rigidbody>().useGravity = true;
        objectToPick.GetComponent<Rigidbody>().isKinematic = false;
        objectToPick.SetParent(null);
        objectToPick.position = objectPlacement.position;
        objectHeld = false;
    }
}