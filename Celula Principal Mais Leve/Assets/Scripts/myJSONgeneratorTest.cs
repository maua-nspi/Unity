/*
//CORRETISSIMO MAS SEM CLASSE `PROGRAMAS` E ABAIXO DELA
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class myJSONgeneratorTest : MonoBehaviour
{

    [SerializeField]
    //Lista de robos a serem monitorados
    private List<GameObject> robotsToMonitor = new List<GameObject>();
    private Robots robots = new Robots();

    private void Start()
    {
        foreach (GameObject robot in robotsToMonitor)
        {
            Robozao robotData = robots.GetRobotData(robot.name);
            TraverseHierarchy(robot.transform, robotData);
        }
    }

    //Funcao recursiva para varrer os filhos do robozao e encontrar juntas atraves da tag `tagJoint`
    private void TraverseHierarchy(Transform parent, Robozao robotData)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("tagJoint"))
            {
                robotData.AddJoint(child.gameObject);
            }
            TraverseHierarchy(child, robotData);
        }
    }

    private void Update()
    {
        foreach (GameObject robot in robotsToMonitor)
        {
            Robozao robotData = robots.GetRobotData(robot.name);
            foreach (GameObject joint in robotData.Joints)
            {
                JointInfo jointInfo = robotData.GetJointInfo(joint.name);
                if (joint.transform.hasChanged)
                {
                    jointInfo.AddPosition(joint.transform.rotation, Time.time);
                    joint.transform.hasChanged = false;
                }
            }
        }
    }

    //Para salvar os dados apos todas as funcoes do Update serem executadas
    private void LateUpdate()
    {
        SaveData();
    }

    //Salva os dados num arquivo JSON
    private void SaveData()
    {
        string jsonData = JsonUtility.ToJson(robots);

        using (StreamWriter sw = new StreamWriter(GetFilePath()))
        {
            sw.Write(jsonData);
        }
    }

    //Diretorio para salvar o JSON
    private string GetFilePath()
    {
        string myDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string myPath = string.Format("{0}/my_folder", myDocuments);
        string fileName = "RobotJSON.json";

        if (!Directory.Exists(myPath))
        {
            Directory.CreateDirectory(myPath);
        }

        return Path.Combine(myPath, fileName);
    }
}


//Lista de Robozoes
[System.Serializable]
public class Robots
{
    [SerializeField]
    private List<Robozao> Fabrica = new List<Robozao>();

    public Robozao GetRobotData(string name)
    {
        Robozao robotData = Fabrica.Find(c => c.RobotName == name);
        if (robotData == null)
        {
            robotData = new Robozao(name);
            Fabrica.Add(robotData);
        }
        return robotData;
    }
}

//Cada robozao, com N juntas
[System.Serializable]
public class Robozao
{
    [SerializeField]
    private string Robo;
    
    //[SerializeField]
    private List<GameObject> joints = new List<GameObject>();

    [SerializeField]
    private List<JointInfo> Juntas = new List<JointInfo>();

    public Robozao(string name)
    {
        Robo = name;
    }

    public string RobotName { get { return Robo; } }

    [SerializeField]
    public List<GameObject> Joints 
    { 
        get { return joints; } 
        set { joints = value; } 
    }

    public JointInfo GetJointInfo(string name)
    {
        JointInfo jointInfo = Juntas.Find(c => c.JointName == name);
        if (jointInfo == null)
        {
            jointInfo = new JointInfo(name);
            Juntas.Add(jointInfo);
        }
        return jointInfo;
    }

    // Add joint to the list of joints
    public void AddJoint(GameObject joint)
    {
        Debug.Log("Adding joint " + joint.name + " to robot " + Robo);
        joints.Add(joint);
    }
}


//Informacoes das juntas
[System.Serializable]
public class JointInfo
{
    [SerializeField]
    private string Junta;
    [SerializeField]
    private List<Quaternion> positions = new List<Quaternion>();
    [SerializeField]
    private List<float> times = new List<float>();

    public JointInfo(string name)
    {
        Junta = name;
    }

    public string JointName { get { return Junta; } }

    public void AddPosition(Quaternion position, float time)
    {
        if (positions.Count == 0 || positions[positions.Count - 1] != position)
        {
            positions.Add(position);
            times.Add(time);
        }
    }
}
*/




/*
//TENTANDO IMPLEMENTAR CLASSE PROGRAMS E POINT
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class myJSONgeneratorTest : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> robotsToMonitor = new List<GameObject>();
    private Robots robots = new Robots();

    private void Start()
    {
        foreach (GameObject robot in robotsToMonitor)
        {
            Robozao robotData = robots.GetRobotData(robot.name);
            TraverseHierarchy(robot.transform, robotData);
        }
    }

    private void TraverseHierarchy(Transform parent, Robozao robotData)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("tagJoint"))
            {
                robotData.AddJoint(child.gameObject);
            }
            TraverseHierarchy(child, robotData);
        }
    }

    private void Update()
    {
        foreach (GameObject robot in robotsToMonitor)
        {
            Robozao robotData = robots.GetRobotData(robot.name);
            foreach (GameObject joint in robotData.Joints)
            {
                Programs programs = robotData.GetPrograms(joint.name);
                if (joint.transform.hasChanged)
                {
                    Point point = programs.GetPoint(joint.name);
                    point.Positions.Add(joint.transform.rotation);
                    point.Speed = GetJointSpeed(joint.transform.rotation, point.Positions, point.isLinear);
                    joint.transform.hasChanged = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        SaveData();
    }

    private void SaveData()
    {
        string jsonData = JsonUtility.ToJson(robots);

        using (StreamWriter sw = new StreamWriter(GetFilePath()))
        {
            sw.Write(jsonData);
        }
    }

    private string GetFilePath()
    {
        string myDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string myPath = string.Format("{0}/my_folder", myDocuments);
        string fileName = "RobotJSON.json";

        if (!Directory.Exists(myPath))
        {
            Directory.CreateDirectory(myPath);
        }

        return Path.Combine(myPath, fileName);
    }

    private float GetJointSpeed(Quaternion newRotation, List<Quaternion> positions, bool isLinear)
    {
        if (positions.Count < 2)
        {
            return 0f;
        }

        Quaternion prevRotation = positions[positions.Count - 2];
        float timeDiff = Time.deltaTime;
        if (positions.Count > 2)
        {
            timeDiff = Time.time - positions[positions.Count - 3].w;
        }

        float angleDiff = Quaternion.Angle(prevRotation, newRotation);
        float speed = angleDiff / timeDiff;

        if (isLinear)
        {
            speed *= Mathf.Rad2Deg;
        }

        return speed;
    }
}

[System.Serializable]
public class Robots
{
    [SerializeField]
    private List<Robozao> Fabrica = new List<Robozao>();

    public Robozao GetRobotData(string name)
    {
        Robozao robotData = Fabrica.Find(c => c.RobotName == name);
        if (robotData == null)
        {
            robotData = new Robozao(name);
            Fabrica.Add(robotData);
        }
        return robotData;
    }
}


[System.Serializable]
public class Robozao
{
    [SerializeField]
    private string Robo;
    [SerializeField]
    private List<GameObject> joints = new List<GameObject>();
    [SerializeField]
    private List<Programs> programs = new List<Programs>();

    public Robozao(string name)
    {
        Robo = name;
    }

    public string RobotName { get { return Robo; } }

    public List<Programs> GetPrograms()
    {
        Programs programs = new Programs();
        foreach(JointInfo jointInfo in Juntas)
        {
            programs.AddProgram(jointInfo.Point);
        }
        return programs;
    }


}


//Classes de programas contendo informações sobre cada junta
[System.Serializable]
public class Programs
{
    [SerializeField]
    private List<Point> points = new List<Point>();

    public void AddProgram(Point point)
    {
        points.Add(point);
    }

    public List<Point> GetPoints()
    {
        return points;
    }
}


//Classe de cada junta
[System.Serializable]
public class Point
{
    [SerializeField]
    public string PointName;
    [SerializeField]
    public float Speed;
    [SerializeField]
    public bool isLinear;
    [SerializeField]
    public List<Quaternion> Positions;

    public Point(string pointName, float speed, bool isLinear, List<Quaternion> positions)
    {
        PointName = pointName;
        Speed = speed;
        this.isLinear = isLinear;
        Positions = positions;
    }

    public string GetName()
    {
        return PointName;
    }

    public float GetSpeed()
    {
        return Speed;
    }

    public bool IsLinear()
    {
        return isLinear;
    }

    public List<Quaternion> GetPositions()
    {
        return Positions;
    }
}


//Informacoes das juntas
[System.Serializable]
public class JointInfo
{
    [SerializeField]
    private string Junta;
    [SerializeField]
    private List<Quaternion> positions = new List<Quaternion>();
    [SerializeField]
    private List<float> times = new List<float>();

    public JointInfo(string name)
    {
        Junta = name;
    }

    public string JointName { get { return Junta; } }

    public void AddPosition(Quaternion position, float time)
    {
        if (positions.Count == 0 || positions[positions.Count - 1] != position)
        {
            positions.Add(position);
            times.Add(time);
        }
    }
}
*/




/*
[System.Serializable]
public class Comandos
{
    [SerializeField]
    private bool[] Inputs;
    [SerializeField]
    private bool[] Outputs;
    [SerializeField]
    private bool Wait;


    public void setInput(bool[] inputs)
    {
        for(int i=0; i<inputs.lenght(); i++)
        {
            Inputs[i] = inputs[i];
        }
        return Inputs;
    }

    public void resetInput()
    {
        Inputs = {0,0,0,0,0,0,0,0};
    }

    public void setOutput(bool outputs[])
    {
        for(int i=0; i<ouputs.lenght(); i++)
        {
            Outputs[i] = outputs[i];
        }
        return Outputs;
    }

    public void resetOutput()
    {
        Outputs = {0,0,0,0,0,0,0,0};
    }

    public void Wait(bool state)
    {
        state ? Wait = 1 : Wait = 0;
    }
    
}
*/


/*
//TESTANDO (E TA DANDO CERTO)
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class myJSONgeneratorTest : MonoBehaviour
{

    [SerializeField]
    //Lista de robos a serem monitorados
    private List<GameObject> robotsToMonitor = new List<GameObject>();
    private Robots robots = new Robots();

    private void Start()
    {
        foreach (GameObject robot in robotsToMonitor)
        {
            Robozao robotData = robots.GetRobotData(robot.name);
            TraverseHierarchy(robot.transform, robotData);
        }
    }

    //Funcao recursiva para varrer os filhos do robozao e encontrar juntas atraves da tag `tagJoint`
    private void TraverseHierarchy(Transform parent, Robozao robotData)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("tagJoint"))
            {
                robotData.AddJoint(child.gameObject);
            }
            TraverseHierarchy(child, robotData);
        }
    }

    private void Update()
    {
        
        foreach (GameObject robot in robotsToMonitor)
        {
            Robozao robotData = robots.GetRobotData(robot.name);
            foreach (GameObject joint in robotData.Joints)
            {
                Point point = robotData.GetJointInfo(joint.name).point;
                Command command = robotData.GetJointInfo(joint.name).command;
                
                if (joint.transform.hasChanged)
                {
                    point.AddPosition(joint.transform.rotation, Time.time);
                    //command.setInputs(inputs);
                    joint.transform.hasChanged = false;
                }
            }
        }
    }

        private void LateUpdate()
    {
        SaveData();
    }

    private void SaveData()
    {
        string jsonData = JsonUtility.ToJson(robots);

        using (StreamWriter sw = new StreamWriter(GetFilePath()))
        {
            sw.Write(jsonData);
        }
    }

    private string GetFilePath()
    {
        string myDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string myPath = string.Format("{0}/my_folder", myDocuments);
        string fileName = "RobotJSON.json";

        if (!Directory.Exists(myPath))
        {
            Directory.CreateDirectory(myPath);
        }

        return Path.Combine(myPath, fileName);
    }


    private float GetJointSpeed(Quaternion newRotation, List<Quaternion> positions, bool isLinear)
    {
        if (positions.Count < 2)
        {
            return 0f;
        }

        Quaternion prevRotation = positions[positions.Count - 2];
        float timeDiff = Time.deltaTime;
        if (positions.Count > 2)
        {
            timeDiff = Time.time - positions[positions.Count - 3].w;
        }

        float angleDiff = Quaternion.Angle(prevRotation, newRotation);
        float speed = angleDiff / timeDiff;

        if (isLinear)
        {
            speed *= Mathf.Rad2Deg;
        }

        return speed;
    }

}


//Lista de Robozoes
[System.Serializable]
public class Robots
{
    [SerializeField]
    private List<Robozao> fabrica = new List<Robozao>();

    public Robozao GetRobotData(string name)
    {
        Robozao robotData = fabrica.Find(c => c.RobotName == name);
        if (robotData == null)
        {
            robotData = new Robozao(name);
            fabrica.Add(robotData);
        }
        return robotData;
    }
}

//Cada robozao, com N juntas
[System.Serializable]
public class Robozao
{
    [SerializeField]
    private string robo;
    
    private List<GameObject> joints = new List<GameObject>();

    [SerializeField]
    private List<Programs> programs = new List<Programs>();

    public Robozao(string name)
    {
        robo = name;
    }

    public string RobotName { get { return robo; } }

    public List<GameObject> Joints 
    { 
        get { return joints; } 
        set { joints = value; } 
    }

    public Programs GetJointInfo(string name)
    {
        Programs program = programs.Find(c => c.PointName == name);
        if (program == null)
        {
            program = new Programs(name);
            programs.Add(program);
        }
        return program;
    }

    // Add joint to the list of joints
    public void AddJoint(GameObject joint)
    {
        Debug.Log("Adding joint " + joint.name + " to robot " + robo);
        joints.Add(joint);
    }
}


//Informacoes das juntas
[System.Serializable]
public class Programs
{
    [SerializeField]
    public Point point;
    [SerializeField]
    public string pointName;
    [SerializeField]
    public Command command;


    public Programs(string name)
    {
        pointName = name;
        point = new Point();
        command = new Command();
    }

    public string PointName { get { return pointName; } }
    public Point pointInfo { get { return point; } }
    public Command commandInfo { get { return command; } }
}

[System.Serializable]
public class Point
{
    [SerializeField]
    private string pointName;
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool isLinear;

    public string PointName { get { return pointName; } }

    public float Speed 
    { 
        get { return speed; } 
        set { speed = value; } 
    }

    public bool IsLinear 
    { 
        get { return isLinear; } 
        set { isLinear = value; } 
    }

    public void AddPosition(Quaternion rotation, float time)
    {
        PositionData newData = new PositionData(rotation, time);
        positions.Add(newData);
    }

    [System.Serializable]
    private class PositionData
    {
        public Quaternion rotation;
        public float time;

        public PositionData(Quaternion rotation, float time)
        {
            this.rotation = rotation;
            this.time = time;
        }
    }

    [SerializeField]
    private List<PositionData> positions = new List<PositionData>();
}



[System.Serializable]
public class Command
{
    [SerializeField]
    public bool Wait { get; set; }
    [SerializeField]
    public List<bool> Inputs { get; private set; }
    [SerializeField]
    public List<bool> Outputs { get; private set; }

    public Command()
    {
        Inputs = new List<bool>(8);
        Outputs = new List<bool>(8);
        for (int i = 0; i < 8; i++)
        {
            Inputs.Add(false);
            Outputs.Add(false);
        }
    }

    public void setInputs(List<bool> inputs)
    {
        if (inputs.Count != Inputs.Count)
        {
            Debug.LogError("Invalid number of inputs");
            return;
        }

        Inputs = inputs;
    }

    public void resetInputs()
    {
        for (int i = 0; i < Inputs.Count; i++)
        {
            Inputs[i] = false;
        }
    }

    public void setOutputs(List<bool> outputs)
    {
        if (outputs.Count != Outputs.Count)
        {
            Debug.LogError("Invalid number of outputs");
            return;
        }

        Outputs = outputs;
    }

    public void resetOutputs()
    {
        for (int i = 0; i < Outputs.Count; i++)
        {
            Outputs[i] = false;
        }
    }
}

*/