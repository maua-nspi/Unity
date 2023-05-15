using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Factory_VR
{
    
    public class CameraController : MonoBehaviour
    {
        #region GlobalVariables
        public GameObject FPSChar;
        public GameObject CharEye;
        public float MouseSensitivity = 100f;
        public CharacterController controller;
        public float speed = 100f;

        private float xRotation = 0f;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            float zMove = Input.GetAxis("Horizontal")*speed*Time.deltaTime;
            float xMove = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            float MouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
            float MouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

            xRotation = xRotation - MouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            CharEye.transform.localRotation = Quaternion.Euler(xRotation,0,0);

            FPSChar.transform.Rotate(Vector3.up * MouseX);

            Vector3 move = FPSChar.transform.right * zMove + FPSChar.transform.forward * xMove;
            controller.Move(move);


            


        }
    }
}
