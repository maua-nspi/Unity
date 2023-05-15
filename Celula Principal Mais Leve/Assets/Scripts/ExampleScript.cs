using UnityEngine;
public class ExampleScript : MonoBehaviour
{
    float rotationSpeed = 45;
    Vector3 currentEulerAngles;
    float x;
    float y;
    float z;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) x = 1 - x;
        if (Input.GetKeyDown(KeyCode.Y)) y = 1 - y;
        if (Input.GetKeyDown(KeyCode.Z)) z = 1 - z;

        //modifying the Vector3, based on input multiplied by speed and time
        currentEulerAngles += new Vector3(x, y, z) * Time.deltaTime * rotationSpeed;

        //apply the change to the gameObject
        transform.localEulerAngles = currentEulerAngles;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        GUI.Label(new Rect(10, 0, 0, 0), "Rotating on X:" + x + " Y:" + y + " Z:" + z, style);

        GUI.Label(new Rect(10, 50, 0, 0), "Transform.localEulerAngle: " + transform.localEulerAngles, style);
    }
}