using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioPlayer : MonoBehaviour
{
    public AudioSource RobotSFX;

    public void PlayRobotSFX(){
        RobotSFX.Play();
    }

    void Update()
    {
        if( (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) != 0) || (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) != 0) )
        {
            PlayRobotSFX();
        }

    }
    
}
