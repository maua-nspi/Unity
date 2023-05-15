using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Factory_VR;
using UnityEngine.EventSystems;
using TMPro;

namespace Factory_VR
{
    public class TesteRobô : MonoBehaviour
    {
        public Robot MH24 = new Robot("MH24", 6);
        public float[] ZeroJoint = {0,0,-90,0,0,0,0};
        //private int i = 0;
        public float speed = 2;
        public int flag1 = 0;
        public int step = 0;
        public int i = 0;
        public int j = 0;
        public int test = 0;
        public float[] Angles = { 0, 0, 0, 0, 0, 0, 0 };
        public int Mode = 0;
        public GameObject GravaPonto;
        public TextMeshProUGUI TextoNomeDoRoboTV;
        public TextMeshProUGUI TextoAngJuntasTV;
        public TextMeshProUGUI TextoNomeDoRoboWrist;
        public TextMeshProUGUI TextoAngJuntasWrist;

        // Start is called before the first frame update
        void Start()
        {
            MH24.RobotInitiate(ZeroJoint);
            UpdateJointText();
            
        }

        // Update is called once per frame
        void Update()
        {
            UpdateJointText();
            MH24.Selection();
            MH24.Manipulate();
            
            switch(Mode)
            { 
                case 1:

                    if ( step != -1)//Se não é o fim do programa
                    {
                        if (step == 1) //Se chegou no fim da linha vai pra p´roxima
                        {
                            j++;
                            step = 0;
                        }
                        Debug.Log(j);
                        step = MH24.SetRobotToPosition(j, speed);
                        // if (flag1 == 0)
                        //{
                        //    test = MH24.SetRobotToPositionNOW(j + 1);
                        //     flag1 = 1;
                        // }
                        //else
                        //  step = MH24.SetRobotToPosition(j + 1, speed);
                    }
                    else
                    {
                        Mode = 0;
                        j = 0;
                        step = 0;
                    }
                        
                    break;

                case 2:
                    MH24.SetRobotToJointAngle(Angles);
                    break;



            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                if (Mode == 0)
                    Mode = 2;
                else
                    Mode = 0;
            }


            if (Input.GetKeyDown(KeyCode.K))
            //if(OVRInput.GetDown(OVRInput.Button.Two))
                CreateProgram();

            if (Input.GetKeyDown(KeyCode.P))
            //if(OVRInput.GetDown(OVRInput.Button.Four))
            {
                PrintJoint();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                PrintJointTest();
            }
            
            if (Input.GetKeyDown(KeyCode.E))
                PlayPorgram();


        }

        public void PlayPorgram()
        {
            Mode = 1;
            MH24.LoadProgram("P5");
        }

        public void SetPos()
        {
            Mode = 1;
        }

        public void CreateProgram()
        {
            MH24.CreateProgram(@"\P5.txt");
        }

        public void PrintJointTest()
        {
            MH24.PrintJointTest();
        }

        public void PrintJoint()
        {
            MH24.PrintJointPosition();
            MH24.WriteProgram(MH24.GetInstantJointPoint(), @"\P5.txt");
            //Debug.Log(MH24.GetInstantJointPoint());  
            //MH24.WriteProgram(MH24.GetInstantJointPoint());
        }

        public void CleanProgram()
        {
            MH24.CleanProgram("P5");
        }

        void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 24;

            GUI.Label(new Rect(10, 50, 0, 0), MH24.PrintJointPosition(), style);
        }

        public void UpdateJointText()
        {
            TextoNomeDoRoboTV.text = MH24.name;
            TextoNomeDoRoboWrist.text = MH24.name;
            TextoAngJuntasWrist.text = MH24.PrintJointPosition();
            TextoAngJuntasTV.text = MH24.PrintJointPosition();
            
            int Index = IndexControl();

            SelectJointText(Index, TextoAngJuntasWrist);
            SelectJointText(Index, TextoAngJuntasTV);
        }

        public void SelectJointText(int index, TextMeshProUGUI Texto)
        {
            string[] lines = Texto.text.Split("\n"[0]);
            string updatedText = "";
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == index)
                {
                    updatedText += "<color=#F9FF4E>" + lines[i] + "</color>\n";
                }
                else
                {
                    updatedText += lines[i] + "\n";
                }
            }
            Texto.text = updatedText;
        }


        public void DeselectJointText(int index, TextMeshProUGUI Texto)
        {
            string[] lines = Texto.text.Split("\n"[0]);
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
            Texto.text = updatedText;
        }

        public int IndexControl()
        {   
            if(OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
            {
                if (i < 5)
                    i++;
            }

            if(OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {
                if (i > 0)
                    i--;
            }

            return i;
        }
    }
}

