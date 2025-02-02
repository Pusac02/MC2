using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class fpsController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    public Camera cam;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float moveSpeedWalk = 3f;
    [SerializeField] float moveSpeedRun = 8f;
    [SerializeField] float moveSpeedCross = 2.5f;
    [SerializeField] float moveSpeedCrossWalk = 1.5f;
    [SerializeField] bool isWalking = false;
    [SerializeField] bool isRunning=false;
    [SerializeField] bool isCrossRunning=false;
    [SerializeField] bool isCrossWalking=false;
    [SerializeField] float jumpHeight = 4f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float lookSpeed = 2f;
    [SerializeField] float lookXLimit = 60f;//Aşağı bakma limiti
    private float rotationX ;
    [SerializeField] float currentVerticalSpeed;
    [SerializeField] float currentHorizontalSpeed;
    [SerializeField] float moveDirectionUp;
    private Vector3 moveDirection=Vector3.zero;


    private Vector3 cameraPosition1=new Vector3(0,0.844f,0.261f);
    private Vector3 cameraPosition3=new Vector3(0,1.109f,-1.161f);
    public KeyCode keytoDedectw=KeyCode.W ;
    
    private float lastClickTime=0f;
    private float doubleClickTime=0.5f;
    
    
    void Start()
    {
        //REFERANSLARI ALMA
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        //CURSORUN GÖRÜNÜR OLUP OLMAMASI
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        

       
    }

    // Update is called once per frame
    void Update()
    {   
        if(cam.transform.localPosition==cameraPosition1)
        {
            if(Input.GetKeyDown(KeyCode.F5))
            {
                cam.transform.localPosition = cameraPosition3;
            }
        }
        else if(cam.transform.localPosition==cameraPosition3)
        {
            if(Input.GetKeyDown(KeyCode.F5))
            {
                cam.transform.localPosition = cameraPosition1;
            }
        }


        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if(Input.GetKey(KeyCode.LeftShift))
        {isWalking = true;}
        else
        {isWalking = false;}

        if(Input.GetKeyDown(keytoDedectw))
        {
            if(Time.time-lastClickTime<doubleClickTime)
            {
               // isRunning=true;
               currentHorizontalSpeed=moveSpeedRun*Input.GetAxis("Horizontal");
               currentVerticalSpeed=moveSpeedRun*Input.GetAxis("Vertical");
            }
        /*  else
            {
                isRunning=false;
            }   */
            lastClickTime=Time.time;
            
        }   /*
            if(isRunning&&Input.GetKeyDown(KeyCode.A)&&Input.GetKeyDown(KeyCode.D))
            {
               isRunning=false;
            }
            */

            // W veya S ile A veya D'ye aynı anda basılı tutulduğunu kontrol et
    if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) &&
        (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
    {
        Debug.Log("Hem Vertical hem Horizontal tuşlarına basılıyor!");
                isCrossRunning=true;
            }
            else
            {
                isCrossRunning=false;
            }
        if(isWalking)
        {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) &&
        (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
    {
        Debug.Log("Hem Vertical hem Horizontal tuşlarına basılıyor!");
                isCrossWalking=true;
            }
            else
            {
                isCrossWalking=false;
            }
        }

       

        if(isWalking)
        {
            currentHorizontalSpeed = moveSpeedWalk*Input.GetAxis("Horizontal");
            currentVerticalSpeed = moveSpeedWalk*Input.GetAxis("Vertical");
        }
        if(isCrossWalking)
        {
            currentHorizontalSpeed = moveSpeedCrossWalk*Input.GetAxis("Horizontal");
            currentVerticalSpeed = moveSpeedCrossWalk*Input.GetAxis("Vertical");
        }
        else if(isRunning)
        {
            currentHorizontalSpeed = moveSpeedRun*Input.GetAxis("Horizontal");
            currentVerticalSpeed = moveSpeedRun*Input.GetAxis("Vertical");
        }
        else if(isCrossRunning)
        {
            currentHorizontalSpeed = moveSpeedCross*Input.GetAxis("Horizontal");
            currentVerticalSpeed = moveSpeedCross*Input.GetAxis("Vertical");
        }

        else
        {
            currentHorizontalSpeed = moveSpeed*Input.GetAxis("Horizontal");
            currentVerticalSpeed = moveSpeed*Input.GetAxis("Vertical");
        }

        moveDirection = (forward*currentVerticalSpeed)+(right*currentHorizontalSpeed);
        
        if(!controller.isGrounded)
        {
            moveDirectionUp+=gravity*Time.deltaTime;
        }
        else
        {
            moveDirectionUp = 0;
        }


       

        if(controller.isGrounded )
        {

            if(Input.GetButton("Jump"))
            {
                moveDirectionUp = jumpHeight;
            }
        }


        moveDirection.y = moveDirectionUp;

        rotationX+=-Input.GetAxis("Mouse Y")*lookSpeed;

        rotationX = Mathf.Clamp(rotationX,-lookXLimit,lookXLimit);

        cam.transform.localRotation = Quaternion.Euler(rotationX,0,0);

        transform.rotation*=Quaternion.Euler(0,Input.GetAxis("Mouse X")*lookSpeed,0);

        controller.Move(moveDirection*Time.deltaTime);

        
    }

}

