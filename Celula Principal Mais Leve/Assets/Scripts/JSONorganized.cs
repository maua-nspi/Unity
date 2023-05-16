using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Linq;
using TMPro;
using System.Text.RegularExpressions;


[System.Serializable]
public class Position
{
    public Quaternion rotation;
    //public float time;
}

[System.Serializable]
public class Point
{
    public string pointName;
    public float speed;
    public bool isLinear;
    public List<Position> positions = new List<Position>();
}


[System.Serializable]
public class Command
{
    public bool Wait;
    public int[] Inputs = new int[8];
    public int[] Outputs = new int[8];

    public void setInputs(int[] inputs)
    {
        if (inputs.Length != Inputs.Length)
        {
            Debug.LogError("Invalid number of inputs");
            return;
        }

        for (int i = 0; i < 8; i++)
        {
            Inputs[i] = inputs[i];
        }
    }

    public void resetInputs()
    {
        for (int i = 0; i < Inputs.Length; i++)
        {
            Inputs[i] = 0;
        }
    }

    public void setOutputs(int[] outputs)
    {
        if (outputs.Length != Outputs.Length)
        {
            Debug.LogError("Invalid number of outputs");
            return;
        }
        

        for (int i = 0; i < 8; i++)
        {
            Outputs[i] = outputs[i];
        }
        
    }

    public void resetOutputs()
    {
        for (int i = 0; i < Outputs.Length; i++)
        {
            Outputs[i] = 0;
        }
    }
}

[System.Serializable]
public class Program
{
    //public Point point;
    public List<Point> points = new List<Point>();
    public Command command;
}

[System.Serializable]
public class Robo
{
    public string robo;
    public List<Program> programs = new List<Program>();
}

[System.Serializable]
public class Fabrica
{
    public List<Robo> robos = new List<Robo>();
}


public class JSONorganized : MonoBehaviour
{
    [SerializeField] private string filePath = @"C:\Users\NSPi\Documents\my_folder\JSONorganized.json";
    [SerializeField] private GameObject[] robotObjects = new GameObject[4];

    private Fabrica fabrica = new Fabrica();
    public TextMeshProUGUI pointText;
    private int activeRobotIndex = 0; // Index of the active robot

    //Canvas Main
    public Button addButton;
    public Button deleteButton;
    public Button upButton;
    public Button downButton;

    public Button playButton;

    //Canvas Submit
    public Button SubmitButton;
    public Button ConfirmDeleteButton;
    public Toggle MovToggle;
    public Slider SpeedSlider;

    public GameObject CanvasToSubmit;
    public GameObject CanvasToDelete;

    private int currentLine = 0;
    private string[] lines;
    private string pointText_raw = "";

    private void Start()
    {
        addButton.onClick.AddListener(OpenCanvasToSubmit);
        deleteButton.onClick.AddListener(OpenCanvasToDelete);

        SubmitButton.onClick.AddListener(createPoint);
        ConfirmDeleteButton.onClick.AddListener(DeleteLine);

        upButton.onClick.AddListener(PrevLine);
        downButton.onClick.AddListener(NextLine);

        playButton.onClick.AddListener(PlayProgram);
        
        lines = pointText.text.Split('\n');


        // Add the robots to the fabrica
        for (int i = 0; i < robotObjects.Length; i++)
        {
            if (robotObjects[i] != null)
            {
                Robo robo = new Robo();
                robo.robo = robotObjects[i].name;

                // Find all joint GameObjects for this robot and add their positions to the Point
                Point startPoint = new Point();
                startPoint.pointName = "Start 0";
                startPoint.speed = 50.0f;
                startPoint.isLinear = false;

                Transform[] children = robotObjects[i].GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    for(int j = 1; j < 7; j++){
                        if (child.CompareTag("tagJoint" + j))
                        {
                            Position position = new Position();
                            position.rotation = child.localRotation;
                            //position.time = 0.0f;

                            startPoint.positions.Add(position);
                        }
                    }
                }

                Program program = new Program();
                program.points.Add(startPoint);
                program.command = new Command();

                robo.programs.Add(program);
                fabrica.robos.Add(robo);
            }
            
            UpdateDisplay();
        }

        // Save the fabrica as JSON
        string json = JsonUtility.ToJson(fabrica);
        File.WriteAllText(filePath, json);

        Debug.Log("JSON saved to " + filePath);
    }

    public void createProgram()
    {
        // Check for active robot
        for (int i = 0; i < robotObjects.Length; i++)
        {
            Transform activeChecker = robotObjects[i].transform.Find("AroDeSelecao");
            if (activeChecker != null && activeChecker.gameObject.activeSelf)
            {
                activeRobotIndex = i;
                break;
            }
        }

        if (robotObjects[activeRobotIndex] != null)
        {
            // Create a new program for the active robot
            Program program = new Program();
            program.points.Add(new Point());
            program.command = new Command();

            fabrica.robos[activeRobotIndex].programs.Add(program);

            // Save the updated fabrica as JSON
            string json = JsonUtility.ToJson(fabrica);
            File.WriteAllText(filePath, json);

            Debug.Log("New program created for " + fabrica.robos[activeRobotIndex].robo);
        }
    }

    public void OpenCanvasToSubmit()
    {
        CanvasToSubmit.SetActive(true);
    }


    public void OpenCanvasToDelete()
    {
        CanvasToDelete.SetActive(true);
    }


    public void createPoint()
    {

        // Check for active robot
        activeRobotIndex = -1;

        for (int i = 0; i < robotObjects.Length; i++)
        {
            Transform activeChecker = robotObjects[i].transform.Find("AroDeSelecao");
            if (activeChecker != null && activeChecker.gameObject.activeSelf)
            {
                activeRobotIndex = i;
                break;
            }
        }

        if(activeRobotIndex == -1)
        {
            Debug.Log("Nenhum robo esta selecionado");
        }
        else if (robotObjects[activeRobotIndex] != null && fabrica.robos[activeRobotIndex].programs.Count > 0)
        {
            
            // Get the current program
            Program currentProgram = fabrica.robos[activeRobotIndex].programs[fabrica.robos[activeRobotIndex].programs.Count - 1];

            // Create a new point for the current program
            Point newPoint = new Point();
            newPoint.pointName = "Point " + (currentProgram.points.Count);
            newPoint.speed = SpeedSlider.value;
            newPoint.isLinear = MovToggle.isOn;

            currentProgram.points.Add(newPoint);

            Transform[] children = robotObjects[activeRobotIndex].GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                for(int j = 1; j < 7; j++){
                    if (child.CompareTag("tagJoint" + j))
                    {
                        Position position = new Position();
                        position.rotation = child.localRotation;
                        //position.time = 0.0f;

                        newPoint.positions.Add(position);
                    }
                }

            }

            // Save the updated fabrica as JSON
            string json = JsonUtility.ToJson(fabrica);
            File.WriteAllText(filePath, json);

            Debug.Log("New point created for program " + (fabrica.robos[activeRobotIndex].programs.Count));

            // Create a StringBuilder object to store the point details
            StringBuilder pointDetails = new StringBuilder();

            // Add the header to the StringBuilder
            pointDetails.AppendLine("Point\tisLinear\tSpeed");

            // Iterate over the points and append their details to the StringBuilder
            foreach (Point point in currentProgram.points)
            {
                string linearStr = point.isLinear ? "MOVL  " : "MOVJ  ";
                pointDetails.AppendLine(point.pointName + "\t" + linearStr + "\tV=" + point.speed.ToString("0.00"));
            }

            // Assign the point details to the TMP object
            pointText.text = pointDetails.ToString();

            currentLine++;
            UpdateDisplay();
            CanvasToSubmit.SetActive(false);
        }

    }

    //ainda tem problemas!! programs[0] e esta perdendo o indice correto ao apagar um ponto
    private void DeleteLine()
    {
        // Remove the line at the current position from the TMP object
        lines = pointText.text.Split('\n');
        if (currentLine >= 0 && currentLine < lines.Length)
        {
            lines = lines.Where((val, idx) => idx != currentLine).ToArray();
            pointText.text = string.Join("\n", lines).Trim();
            if (currentLine >= lines.Length)
            {
                currentLine = lines.Length - 1;
            }
        }
        
        // Remove the line at the current position from the JSON file

        // Check that there is a selected line
        if (lines.Length == 0 || currentLine < 0 || currentLine >= lines.Length) return;

        // Get the active program for the active robot
        Program program = fabrica.robos[activeRobotIndex].programs[0];

        // Remove the point from the program
        Point pointToRemove = program.points[currentLine];
        program.points.Remove(pointToRemove);

        // Serialize the Fabrica object back to JSON and write to file
        string json = JsonUtility.ToJson(fabrica);
        File.WriteAllText(filePath, json);


        UpdateDisplay();
        CanvasToDelete.SetActive(false);
    }


    private void PrevLine()
    {
        currentLine--;
        if (currentLine < 0)
        {
            currentLine = lines.Length - 1;
        }
        UpdateDisplay();
    }

    private void NextLine()
    {
        currentLine++;
        if (currentLine >= lines.Length)
        {
            currentLine = 0;
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        pointText_raw = Regex.Replace(pointText.text, "<.*?>", string.Empty);
        lines = pointText_raw.Split('\n');
        string displayString = "";
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == currentLine)
            {
                displayString += "<b><color=#000080>" + lines[i].Trim() + "</color></b>\n";
            }
            else
            {
                displayString += lines[i].Trim() + "\n";
            }
        }
        displayString = displayString.TrimEnd('\n');
        pointText.text = displayString;
    }

    /*
    public void PlayProgram()
    {
        if (robotObjects[activeRobotIndex] != null)
        {
            Robo robo = fabrica.robos[activeRobotIndex];

            if (robo.programs.Count > 0)
            {
                Program program = robo.programs[0]; // current program

                // iterate through each point in the program
                foreach (Point point in program.points)
                {
                    float speed = point.speed;
                    bool isLinear = point.isLinear;

                    // iterate through each position in the point
                    for (int i = 0; i < point.positions.Count; i++)
                    {
                        Quaternion startRot = robotObjects[activeRobotIndex].transform.GetChild(i).localRotation;
                        Quaternion endRot = point.positions[i].rotation;

                        // smoothly interpolate between the start and end rotations
                        float t = 0;
                        while (t < 1)
                        {
                            t += Time.deltaTime / speed;
                            Quaternion currentRot = Quaternion.Slerp(startRot, endRot, isLinear ? t : t * t);
                            robotObjects[activeRobotIndex].transform.GetChild(i).localRotation = currentRot;
                            yield return null;
                        }

                        // set the final rotation of the joint
                        robotObjects[activeRobotIndex].transform.GetChild(i).localRotation = endRot;
                    }
                }
            }
        }
    }
    */

    
    /*
    // Coroutine to wait for the duration of the current step and then move the robot to the next step
    private IEnumerator WaitAndMoveRobot(WaitForSeconds wait)
    {
        yield return wait;
        yield return null;
    }
    */

    public void PlayProgram()
    {
        if (robotObjects[activeRobotIndex] != null)
        {
            Robo robo = fabrica.robos[activeRobotIndex];

            if (robo.programs.Count > 0)
            {
                Program program = robo.programs[0]; // current program

                StartCoroutine(MoveRobot(program));
            }
        }
    }

    // private IEnumerator MoveRobot(Program program)
    // {
    //     for (int i = 0; i < program.points.Count; i++)
    //     {
    //         Point point = program.points[i];
    //         float speed = point.speed;
    //         bool isLinear = point.isLinear;

    //         for (int j = 0; j < point.positions.Count; j++)
    //         {
    //             Transform joint = robotObjects[activeRobotIndex].transform.FindWithTag("tagJoint");
                
    //             if (joint != null)
    //             {
    //                 Quaternion startRot = joint.localRotation;
    //                 Quaternion endRot = point.positions[j].rotation;

    //                 float t = 0;
    //                 while (t < 1)
    //                 {
    //                     t += Time.deltaTime / speed;
    //                     Quaternion currentRot = Quaternion.Slerp(startRot, endRot, isLinear ? t : t);
    //                     joint.localRotation = currentRot;
    //                     yield return null;
    //                 }

    //                 joint.localRotation = endRot;
    //             }
    //         }
    //     }
    // }

    private IEnumerator MoveRobot(Program program)
    {
        for (int i = 0; i < program.points.Count; i++)
        {
            //Debug.Log("Passando pelo ponto " + i);
            Point point = program.points[i];
            float speed = point.speed;
            bool isLinear = point.isLinear;
            for (int j = 0; j < point.positions.Count; j++)
            {
                //Debug.Log("Passando pela junta " + (j+1));
                string jointTag = "tagJoint" + (j+1);

                Transform joint = robotObjects[activeRobotIndex].transform.FindWithTag(jointTag);

                Quaternion startRot = joint.localRotation;
                Quaternion endRot = point.positions[j].rotation;

                float t = 0;
                while (t < 1)
                {
                    t += Time.deltaTime * speed/100;
                    Quaternion currentRot = Quaternion.Slerp(startRot, endRot, isLinear ? t : 3*t);
                    joint.localRotation = currentRot;
                    yield return null;
                }

            }
        }
    }
}

public static class TransformExtensions
{
    public static Transform FindWithTag(this Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
                return child;

            Transform result = child.FindWithTag(tag);
            if (result != null)
                return result;
        }

        return null;
    }
}
