using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteRot : MonoBehaviour
{
    public GameObject Obj1;
    public float step;
    public Quaternion Rotation1;
    public float speed = 0.01f;
    Quaternion A = new Quaternion(-0.7071068f, 0, 0, 0.7071068f);
    Quaternion B = new Quaternion(-0.6904554f, -0.1525497f, -0.1525497f, 0.6904554f);
    // Start is called before the first frame update
    void Start()
    {        
        Obj1.transform.rotation = A;
    }

    // Update is called once per frame
    void Update()
    {

        step = speed*Time.deltaTime;

        // Rotate our transform a step closer to the target's.
        Obj1.transform.rotation = Quaternion.Slerp(Obj1.transform.rotation, B, step);
        

        
    }
}
