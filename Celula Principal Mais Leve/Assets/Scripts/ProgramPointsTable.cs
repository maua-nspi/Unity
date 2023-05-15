using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class ProgramPointsTable : MonoBehaviour
{
    [SerializeField] private string filePath = @"C:\Users\NSPi\Documents\my_folder\JSONorganized.json";
    [SerializeField] private GameObject programPointsPrefab;
    [SerializeField] private Transform programPointsParent;

    private Fabrica fabrica;
    private List<TMP_Text> pointNameTexts = new List<TMP_Text>();
    private List<TMP_Text> speedTexts = new List<TMP_Text>();
    private List<TMP_Text> isLinearTexts = new List<TMP_Text>();

    public void Start()
    {
        // Load the fabrica from JSON
        string json = File.ReadAllText(filePath);
        fabrica = JsonUtility.FromJson<Fabrica>(json);

        // Create UI elements for each program and point
        foreach (Robo robo in fabrica.robos)
        {
            foreach (Program program in robo.programs)
            {
                // Create a new UI element from the prefab
                GameObject programPoints = Instantiate(programPointsPrefab, programPointsParent);
                TMP_Text roboNameText = programPoints.transform.Find("RoboName").GetComponent<TMP_Text>();
                TMP_Text programNameText = programPoints.transform.Find("ProgramName").GetComponent<TMP_Text>();

                foreach (Point point in program.points)
                {
                    // Get the TMP components from the new element
                    TMP_Text pointNameText = programPoints.transform.Find("PointName").GetComponent<TMP_Text>();
                    TMP_Text speedText = programPoints.transform.Find("Speed").GetComponent<TMP_Text>();
                    TMP_Text isLinearText = programPoints.transform.Find("IsLinear").GetComponent<TMP_Text>();

                    // Add the TMP components to the list
                    pointNameTexts.Add(pointNameText);
                    speedTexts.Add(speedText);
                    isLinearTexts.Add(isLinearText);
                }

                // Set the text for each TMP component
                roboNameText.text = robo.robo;
                programNameText.text = "Program " + robo.programs.IndexOf(program);
            }
        }
    }
}
