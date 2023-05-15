using UnityEngine;

public class MyDragUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform pointer;

    [Header("Select To Include In Drag")]
    public bool x;
    public bool y;
    public bool z;

    public void Drag()
    {
        
            float newX = x ? pointer.position.x : transform.position.x;
            float newY = y ? pointer.position.y : transform.position.y;
            float newZ = z ? pointer.position.z : transform.position.z;
            transform.position = new Vector3(newX, newY, newZ);
        
    }
}