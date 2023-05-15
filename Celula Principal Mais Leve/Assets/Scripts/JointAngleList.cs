using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JointAngleList : MonoBehaviour
{
    public Text displayText;
    private int selectedJoint = -1;

    private void Start()
    {
        //Initialize the joint angle list with the initial joint angles
        float[] jointAngles = { 30, 45, 60 };
        string jointAnglesText = "";
        for (int i = 0; i < jointAngles.Length; i++)
        {
            jointAnglesText += "Joint " + (i + 1) + ": " + jointAngles[i] + "\n";
        }
        displayText.text = jointAnglesText;
        displayText.GetComponent<Text>().color = Color.black;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero);
            if (hit.collider != null)
            {
                //Deselect the previously selected joint
                if (selectedJoint != -1)
                {
                    DeselectJointText(selectedJoint);
                }
                //Select the new joint
                selectedJoint = int.Parse(hit.collider.name);
                SelectJointText(selectedJoint);
            }
        }
    }

    void SelectJointText(int index)
    {
        string[] lines = displayText.text.Split("\n"[0]);
        string updatedText = "";
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == index)
            {
                updatedText += "<color=red>" + lines[i] + "</color>\n";
            }
            else
            {
                updatedText += lines[i] + "\n";
            }
        }
        displayText.text = updatedText;
    }

    void DeselectJointText(int index)
    {
        string[] lines = displayText.text.Split("\n"[0]);
        string updatedText = "";
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == index)
            {
                lines[i] = lines[i].Replace("<color=red>", "");
                lines[i] = lines[i].Replace("</color>", "");
                updatedText += lines[i] + "\n";
            }
            else
            {
                updatedText += lines[i] + "\n";
            }
        }
        displayText.text = updatedText;
    }
}