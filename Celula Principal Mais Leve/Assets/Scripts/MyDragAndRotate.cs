using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDragAndRotate : MonoBehaviour
{
    public GameObject myGameObj;
    public Camera cameraObj;
    public Transform pointer;
    public float speed = 2f;

    public void Drag()
    {
        myGameObj.transform.RotateAround(cameraObj.transform.position,
                                        myGameObj.transform.up,
                                        pointer.position.x);

    }
}