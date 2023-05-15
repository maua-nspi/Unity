using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Globalization;


namespace Factory_VR
{
    [Serializable]
    public class Robot
    {

        public string name;  // Contrutor 1
        public int JointNumber; //Construtor 2

        public string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//@"C:\Users\NSPi\Documents\Henrique Nspi";

        public GameObject[] RobotModel;
        public GameObject[] Robot_Obj;
        public float[] DH_a;
        public float[] DH_alpha;
        public float[] DH_d;
        public float[] DH_theta;
        public float[] JointAngleLowerLimit;
        public float[] JointAngleUpperLimit;

        public Quaternion[] Joints;

        public Material SelectedMaterial;
        public Material NormalMaterial;

        public Slider speedSlider;
        public float jointSpeed = 200f;
        public float[] ZeroJoint;
        public float[] JointAngle;
        public float deltaSlerp = 0.01f;
        public float speedSlerp = 0;
        public float[] Difference;

        private int RobotJoint = 1;
        private Quaternion[,] ReadJoint; //Programa Carregado [Linha do Programa ;  Junta]
        private string dirProgs = "";


        //Classe para construir robôs Industriais 6DoF esféricos
        public Robot(string _name, int _JointNumber)
        {

            name = _name;
            JointNumber = _JointNumber + 1;

            //Cria uma pasta para o Robô
            string diraux = dir + @"\" + name;
            //Debug.Log(diraux);
            // If directory does not exist, create it
            if (!Directory.Exists(diraux))
            {
                Directory.CreateDirectory(diraux);
            }

            dirProgs = diraux + @"\Programs";
            //Debug.Log(dirProgs);
            // If directory does not exist, create it
            if (!Directory.Exists(dirProgs))
            {
                Directory.CreateDirectory(dirProgs);
            }

            FileStream stream = new FileStream(dirProgs + @"\ini.txt", FileMode.OpenOrCreate);
            StreamWriter RobotWriter = new StreamWriter(stream);
            RobotWriter.Write(name);
            RobotWriter.WriteLine();
            RobotWriter.Write("Welcome");
            RobotWriter.Close();
        }

        public void PrintJointTest()
        {
            for (int i = 0; i < JointNumber; i++)
            {
                Debug.Log(Robot_Obj[i].transform.localEulerAngles.z);
            }
        }
        //Executa Inicializações do Robô
        public void RobotInitiate(float[] _ZeroJoint)
        {
            ZeroJoint = _ZeroJoint;
            RobotModel[1].GetComponent<MeshRenderer>().material = SelectedMaterial;
            for (int i = 2; i <= 6; i++)
                RobotModel[i].GetComponent<MeshRenderer>().material = NormalMaterial;

            JointAngle = ZeroJoint;
        }

        //Rotina de Animação de Seleção de Junnta
        public void Selection() //Select a Joint and Show in Green
        {
            if(OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
            {
                if (RobotJoint < 6)
                    RobotJoint++;
                RobotModel[RobotJoint].GetComponent<MeshRenderer>().material = SelectedMaterial;
                RobotModel[RobotJoint - 1].GetComponent<MeshRenderer>().material = NormalMaterial;
            }

            if(OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {
                if (RobotJoint > 1)
                    RobotJoint--;

                RobotModel[RobotJoint].GetComponent<MeshRenderer>().material = SelectedMaterial;
                RobotModel[RobotJoint + 1].GetComponent<MeshRenderer>().material = NormalMaterial;
            }
        }

        //Rotina de Manipulçao de Junta Selecionada
        public void Manipulate() //Manipulate a Selected Joint
        {

            if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) != 0  && JointAngle[RobotJoint] > JointAngleLowerLimit[RobotJoint])
            {
                JointAngle[RobotJoint] -= jointSpeed * speedSlider.value * Time.deltaTime * OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
                Robot_Obj[RobotJoint].transform.Rotate(-Vector3.forward * jointSpeed * speedSlider.value * Time.deltaTime * OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger));
            }
            if(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) != 0 && JointAngle[RobotJoint] < JointAngleUpperLimit[RobotJoint])
            {
                JointAngle[RobotJoint] += jointSpeed * speedSlider.value * Time.deltaTime * OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
                Robot_Obj[RobotJoint].transform.Rotate(Vector3.forward * jointSpeed * speedSlider.value * Time.deltaTime * OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger));
            }
            

        }

        //Retorna o valor da posição x,y,z do endEffector
        public Vector3 GetEndEffectorPosition()
        {
            Vector3 PositionXYZ = 100 * Robot_Obj[0].transform.InverseTransformPoint(Robot_Obj[6].transform.position);
            return PositionXYZ;
        }

        //Retorna os Qauternions da Posição de junta atual
        public Quaternion[] GetInstantJointPoint()
        {
            Quaternion[] JointGroup = new Quaternion[7];
            for (int i = 0; i <= JointNumber; i++)
            {
                JointGroup[i] = Robot_Obj[i].transform.localRotation;
            }
            return JointGroup;
        }

        public void CreateProgram(string programName)
        {
            programName = dirProgs + programName;
            using (FileStream fs = File.Create(programName))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes("");
                fs.Write(title, 0, title.Length);
            }



        }

        //Escreve uma linha com um ponto em Quaternion em um arquivo
        public void WriteProgram(Quaternion[] Point, string programName)
        {
            programName = dirProgs + programName;
            try
            {
                string Pos = "";
                for (int i = 0; i <= JointNumber; i++)
                {
                    if (i < JointNumber)
                        Pos += Point[i].ToString("G") + ";";
                    else
                        Pos += Point[i].ToString("G");
                }

                // Create a new file
                using (StreamWriter fs = File.AppendText(programName))
                {
                    fs.WriteLine(Pos);
                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        //Apaga o arquiuvo de Prgrama
        public void CleanProgram(string file)
        {
            string path = dirProgs + @"\" + file + @".txt"; // Utiliz a variável global de progrmas para encontrar o caminho desejado
            FileStream fileStream = File.Open(path, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close();
        }

        //Escreve a Junta atual no Console
        public string PrintJointPosition()
        {

            string Pos = "";
            string pos2file = "";
            
            for (int i = 1; i <= JointNumber; i++)
            {
                Pos = Pos + "   J" + (i).ToString() + ": " + JointAngle[i].ToString("n4") + "\n";
                if (i != JointNumber)
                    pos2file = pos2file + JointAngle[i].ToString() + ";";
                else
                    pos2file = pos2file + JointAngle[i].ToString();
            }
            return Pos;
        }

        //Decodifica o Arquivo de Programa
        public Quaternion[,] SplitFileString(string file)
        {
            string path = dirProgs + @"\" + file + @".txt";
            int j;
            string[] JointStrings = new string[1000];
            int i = 1;
            StreamReader reader = new StreamReader(path);

            JointStrings[0] = reader.ReadLine();
            while (JointStrings[i - 1] != null)
            {
                JointStrings[i] = reader.ReadLine();
                i++;
            }
            int progSize = i - 1;

            Quaternion[,] ReadJoint = new Quaternion[progSize, JointNumber + 1];

            for (i = 0; i <= progSize - 1; i++)
            {
                string[] sArray = JointStrings[i].Split(';');
                for (j = 0; j <= JointNumber; j++)
                {
                    ReadJoint[i, j] = StringToQuaternion(sArray[j]);
                }
            }
            return ReadJoint;
        }

        //Converte String para Quaternion
        public static Quaternion StringToQuaternion(string sQuaternion)
        {
            // Remove the parentheses
            if (sQuaternion.StartsWith("(") && sQuaternion.EndsWith(")"))
            {
                sQuaternion = sQuaternion.Substring(1, sQuaternion.Length - 2);
            }
            // split the items
            string[] sArray = sQuaternion.Split(',');

            // store as a Vector3
            Quaternion result = new Quaternion(
                float.Parse(sArray[0], CultureInfo.InvariantCulture),
                float.Parse(sArray[1], CultureInfo.InvariantCulture),
                float.Parse(sArray[2], CultureInfo.InvariantCulture),
                float.Parse(sArray[3], CultureInfo.InvariantCulture));
            return result;
        }

        //Move o Robô para a posição desejada no programa Imediatamente
        public int SetRobotToPositionNOW(int line)
        {
            if (line < ReadJoint.GetUpperBound(0))
            {
                for (int i = 0; i <= JointNumber; i++)
                {
                    Robot_Obj[i].transform.localRotation = ReadJoint[line, i];
                }
                return 1;
            }
            else
            {
                return -1;
            }



        }//SetRobotToPositionNOW()

        public int SetRobotToPosition(int line, float speed)
        {
            //Verifica se a linha pedida está dentro dos limites do programa
            if (line < ReadJoint.GetUpperBound(0))
            {
                for (int i = 0; i <= JointNumber; i++)
                {
                    speedSlerp += deltaSlerp;
                    Robot_Obj[i].transform.localRotation = Quaternion.Slerp(ReadJoint[line, i], ReadJoint[line + 1, i], speedSlerp);
                    Difference[i] = Quaternion.Angle(Robot_Obj[i].transform.localRotation, ReadJoint[line + 1, i]);
                    if (speedSlerp > 1)
                        break;
                }

                if (speedSlerp > 1)
                {
                    speedSlerp = 0;
                    return 1;
                }
                    
                else
                    return 0;
            }
            else
                return -1;
        }//SetRobotToPosition()


        //Carrega o programa na variável de programa 
        public int LoadProgram(string file)
        {
            string path = dirProgs + @"\" + file + @".txt"; // Utiliz a variável global de progrmas para encontrar o caminho desejado
            int lineCount = File.ReadAllLines(path).Length; //Conta as linhas do programa desejado
            ReadJoint = SplitFileString(file); //Converte o arquivo desejado em uma matriz read Joits   
            return lineCount;
        }


        public void SetRobotToJointAngle(float[] Angles)
        {
            
            
            for (int i = 0; i <= JointNumber; i++)
            {
                Quaternion Rot = Quaternion.Euler(DH_alpha[i], 0f, Angles[i]);                 
                Robot_Obj[i].transform.localRotation = Rot;
                JointAngle[i] = Angles[i];
            }
        } 

        //public void RunProgram(string file, float speed)
        //{
        //    int i = 1;
        //    int step = 0;
        //    while(i<4)
        //    { 
        //        step = SetRobotToPositionNOW(file, i, speed);
        //        Debug.Log(i);
        //        if (step == 1)
        //            i++;
        //    }
        //}
    }//class Robot
}//namespace
