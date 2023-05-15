using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LineCreator : MonoBehaviour
{
    //public OVRInput.Controller controller; // The controller that will be used
    public Transform textParent; // The parent object for the text components
    public TMP_Text textPrefab; // The prefab for the text components
    public Toggle checkboxPrefab; // The prefab for the checkbox components
    public float lineSpacing = 1f; // The amount of space between lines
    public Button button; // The button that will trigger the creation of new lines
    public Button deleteButton; // The button that will trigger the deletion of selected lines

    private float yPosition; // The current y position for the text components
    private List<TMP_Text> textList = new List<TMP_Text>(); // A list of the text components
    private List<Toggle> checkboxList = new List<Toggle>(); // A list of the checkbox components

    void Start()
    {
        yPosition = 0f; // Initialize the y position
        button.onClick.AddListener(CreateLine); // Add the CreateLine method to the button's onClick event
        deleteButton.onClick.AddListener(DeleteSelectedLines); // Add the DeleteSelectedLines method to the delete button's onClick event
    }

    void CreateLine()
    {
        TMP_Text newText = Instantiate(textPrefab, textParent); // Instantiate a new text component
        newText.transform.localPosition = new Vector3(0f, yPosition, 0f); // Set the local position of the new text component
        newText.text = "New line"; // Set the text of the new text component

        Toggle newCheckbox = Instantiate(checkboxPrefab, textParent); // Instantiate a new checkbox component
        newCheckbox.transform.localPosition = new Vector3(-0.5f, yPosition, 0f); // Set the local position of the new checkbox component
        newCheckbox.onValueChanged.AddListener(delegate {CheckboxValueChanged(newCheckbox);}); // Add the CheckboxValueChanged method to the checkbox's onValueChanged event

        textList.Add(newText); // Add the new text component to the text list
        checkboxList.Add(newCheckbox); // Add the new checkbox component to the checkbox list

        yPosition -= lineSpacing; // Decrement the y position for the next line
    }

    void CheckboxValueChanged(Toggle checkbox)
    {
        int index = checkboxList.IndexOf(checkbox); // Get the index of the checkbox in the checkbox list
        if (index >= 0 && index < textList.Count) // If the index is valid
        {
            textList[index].fontStyle = checkbox.isOn ? FontStyles.Strikethrough : FontStyles.Normal; // Set the text component's fontStyle to Strikethrough if the checkbox is checked, or Normal if the checkbox is unchecked
        }
    }

    void DeleteSelectedLines()
    {
        for (int i = checkboxList.Count - 1; i >= 0; i--) // Loop through the checkbox list in reverse order
        {
            if (checkboxList[i].isOn) // If the checkbox is checked
            {
                Destroy(textList[i].gameObject); // Destroy the corresponding text component
                Destroy(checkboxList[i].gameObject); // Destroy the corresponding checkbox component
                textList.RemoveAt(i); // Remove the corresponding text component from the text list
                checkboxList.RemoveAt(i); // Remove the corresponding checkbox component from the checkbox list
            }
        }
    }
}
