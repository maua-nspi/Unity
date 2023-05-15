using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButtonInput : MonoBehaviour
{

    public GameObject MenuCanvas;
    public GameObject WristCanvas;
    public GameObject MenuDeGravarPonto;
    public OVRPlayerController player;

    // Update is called once per frame
    void Update()
    {

        //Liga/desliga menu de GravarPonto
        if(MenuDeGravarPonto.activeInHierarchy)
        {
            if(OVRInput.GetDown(OVRInput.Button.Three))
                MenuDeGravarPonto.SetActive(!MenuDeGravarPonto.activeInHierarchy);
        }

        //Liga/desliga menu de pulso
        if(OVRInput.GetDown(OVRInput.Button.Three))
        {
            WristCanvas.SetActive(!WristCanvas.activeInHierarchy);
        }
        
        //Liga/desliga menu principal
        if(OVRInput.GetDown(OVRInput.Button.Start))
        {
            MenuCanvas.SetActive(!MenuCanvas.activeInHierarchy);
        }

        //Pular
        if(OVRInput.GetDown(OVRInput.Button.One))
        {
            //player.Jump();
        }

        //Pausa a cena se menu principal esta ativo
        Time.timeScale = MenuCanvas.activeSelf ? 0 : 1;
        Time.timeScale = MenuDeGravarPonto.activeSelf ? 0 : 1;

    }
}