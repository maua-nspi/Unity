using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEndEffector : MonoBehaviour
{
    public GameObject Dedo1;
    public GameObject Dedo2;
    public int sinal = 1;
    public float AberturaDaGarra = 0.01f;
    public float speed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Two))
        {
            sinal *= -1;

            Dedo1.transform.localPosition += new Vector3(0, 0, -sinal*AberturaDaGarra);
            Dedo2.transform.localPosition += new Vector3(0, 0, sinal*AberturaDaGarra);

        }
    }
}
