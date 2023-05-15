using UnityEngine;
using UnityEngine.EventSystems;

public class MySelectRobot : MonoBehaviour, IPointerClickHandler
{
    public Behaviour scriptToToggle;
    public GameObject selectionRing;

    private bool isSelected = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Toggle the script on/off
        isSelected = !isSelected;
        scriptToToggle.enabled = isSelected;
        selectionRing.SetActive(isSelected);

        // Disable script on all other robots
        MySelectRobot[] allRobots = FindObjectsOfType<MySelectRobot>();
        foreach (var robot in allRobots)
        {
            if (robot != this)
            {
                robot.scriptToToggle.enabled = false;
                robot.isSelected = false;
                robot.selectionRing.SetActive(robot.isSelected);
            }
        }
    }
}

