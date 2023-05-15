using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class TextEditor : MonoBehaviour
{
    public TMP_Text textComponent;
    public TMP_InputField inputField;
    public Button addButton;
    public Button deleteButton;
    public Button prevButton;
    public Button nextButton;

    private int currentLine = 0;
    private string[] lines;
    private string textComponent_raw = "";

    private void Start()
    {
        lines = textComponent.text.Split('\n');
        UpdateDisplay();

        addButton.onClick.AddListener(AddLine);
        deleteButton.onClick.AddListener(DeleteLine);
        prevButton.onClick.AddListener(PrevLine);
        nextButton.onClick.AddListener(NextLine);
    }

    private void AddLine()
    {
        // Add a new line of text to the TMP object at the current position
        lines = textComponent.text.Split('\n');
        lines[currentLine] += "\nPoint X	MOVJ  	V=0,00" + inputField.text;
        textComponent.text = string.Join("\n", lines);
        inputField.text = "";
        currentLine++;
        UpdateDisplay();
    }

    private void DeleteLine()
    {
        // Remove the line at the current position from the TMP object
        lines = textComponent.text.Split('\n');
        if (currentLine >= 0 && currentLine < lines.Length)
        {
            lines = lines.Where((val, idx) => idx != currentLine).ToArray();
            textComponent.text = string.Join("\n", lines).Trim();
            if (currentLine >= lines.Length)
            {
                currentLine = lines.Length - 1;
            }
        }
        UpdateDisplay();
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
        textComponent_raw = Regex.Replace(textComponent.text, "<.*?>", string.Empty);
        lines = textComponent_raw.Split('\n');
        string displayString = "";
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == currentLine)
            {
                displayString += "<b><color=yellow>" + lines[i].Trim() + "</color></b>\n";
            }
            else
            {
                displayString += lines[i].Trim() + "\n";
            }
        }
        displayString = displayString.TrimEnd('\n');
        textComponent.text = displayString;
    }
}