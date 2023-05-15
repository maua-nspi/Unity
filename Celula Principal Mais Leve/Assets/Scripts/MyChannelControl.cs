using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyChannelControl : MonoBehaviour
{
    public GameObject[] channels;
    public GameObject[] robotRings;
    private int activeRobotIndex = 0; //default channel active at first

    void Start()
    {
        //activate the default channel
        channels[activeRobotIndex].SetActive(true);

        //deactivate all other channels
        for (int i = 1; i < channels.Length; i++)
        {
            if(i != activeRobotIndex)
            {
                channels[i].SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //changes the default TV channels
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            //active channel
            activeRobotIndex = (activeRobotIndex + 1) % channels.Length;
            channels[activeRobotIndex].SetActive(true);

            //deactivate all channels
            for (int i = 0; i < channels.Length; i++)
            {
                if(i != activeRobotIndex)
                {
                    channels[i].SetActive(false);
                }
            }
        }

        // Checking which robot is active to make its image appear on TV
        for (int j = 0; j < robotRings.Length; j++)
        {
            if(robotRings[j].activeSelf == true)
            {
                //activate the corresponding channel
                channels[j+2].SetActive(true);

                //deactivate all other channels
                for (int i = 0; i < channels.Length; i++)
                {
                    if (i != j+2)
                    {
                        channels[i].SetActive(false);
                    }
                }
            }
        }
    }
}


