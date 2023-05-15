using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFollowAnyObj : MonoBehaviour
{

    public GameObject anyobject;
    public Vector3 offset;

    public bool followPosition = true;
    public bool followXRot = true;
    public bool followYRot = true;
    public bool followZRot = true;

    // Update is called once per frame
    void Update()
    {
        if(followPosition)
            anyobject.transform.position = transform.position + offset;
        float xRot = 0f;
        float yRot = 0f;
        float zRot = 0f;
        if(followXRot)
            xRot = transform.eulerAngles.x;
        if(followYRot)
            yRot = transform.eulerAngles.y;
        if(followZRot)
            zRot = transform.eulerAngles.z;
        anyobject.transform.eulerAngles = new Vector3(xRot, yRot, zRot);
    }
}