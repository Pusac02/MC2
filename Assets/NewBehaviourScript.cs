using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    public Camera cam;

    [Header("Movement")]
    [SerializeField] float move = 5f;
    [SerializeField] float walkMove = 3f;
    [SerializeField] float runMove = 8f;

    
    [SerializeField] bool isWalk = false;
    [SerializeField] bool isRun = false;

    [SerializeField] float jumpSpeed = 4f;
    [SerializeField] float gravity = -9.81f;

    [SerializeField] float MauseSensivity = 2f;

    [SerializeField] float verticalSpeed;
    [SerializeField] float horizontalSpeed;
    [SerializeField] float moveDirectionUp;

    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] float lookXLimit = 60f;


    private Vector3 cameraPosition1=new Vector3(0,0.844f,0.261f);
    private Vector3 cameraPosition3=new Vector3(0,1.109f,-1.161f);


    private float rotationX;

    private float doubleClickTime = 0.5f;
    private float lastClickTime = 0f;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
       
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        CameraChange();
        CrossMoveControl();
        MoveControl();
        Move(forward,right);
        Jump();
        CameraControl();


        controller.Move(moveDirection * Time.deltaTime);

    }

    void CameraChange()
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
    }


    void CrossMoveControl()

    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) &&
            (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            Debug.Log("Hem Vertical hem Horizontal tuşlarına basılıyor!");
            move = 3.536f;
            walkMove = 2.121f;
            runMove = 5.657f;
            
        }

        //Düzenlenebilir

        else if ((!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.S)) &&
                 (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D)))
        {
            Debug.Log("Hem Vertical hem Horizontal tuşlarına BASILMIYOR!!");
            move = 5;
            walkMove = 3;
            runMove = 8;
        }
    }


    void MoveControl()

    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isWalk = true;
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isWalk = false;
        }


        if (Input.GetKeyDown(KeyCode.W))
        {

            if (Time.time - lastClickTime < doubleClickTime)
            {
                Debug.Log("W tuşuna çift tıklandı!");
                isRun = true;

                return;
            }

            lastClickTime = Time.time;
        }

        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            isRun = false;
        }

    }

    void Move(Vector3 forward,Vector3 right)

    {

                if (isWalk)
                    {
                        horizontalSpeed = walkMove * Input.GetAxis("Horizontal");
                        verticalSpeed = walkMove * Input.GetAxis("Vertical");
                    }
                    

                else if (isRun)
                    {
                        horizontalSpeed = runMove * Input.GetAxis("Horizontal");
                        verticalSpeed = runMove * Input.GetAxis("Vertical");
                        if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.D))
                        {
                            isRun = false;
                        }
                    }

        
                else
                    {
                        horizontalSpeed = move * Input.GetAxis("Horizontal");
                        verticalSpeed = move * Input.GetAxis("Vertical");
                    }

        moveDirection = (forward * verticalSpeed) + (right * horizontalSpeed);

    }



    void Jump()     
    {
        if (controller.isGrounded)
        {
            
            moveDirectionUp = 0;
            Debug.Log("sıfırlandı");

            if (Input.GetButton("Jump"))
            {
                moveDirectionUp = jumpSpeed;
                Debug.Log("Jump tuşuna basıldı!");
            }
        }
        
        else 
        {
            moveDirectionUp += gravity * Time.deltaTime;
            Debug.Log("gravity");
        }



        moveDirection.y = moveDirectionUp;

    }
    

    void CameraControl()
    {
        rotationX += -Input.GetAxis("Mouse Y") * MauseSensivity;

        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * MauseSensivity, 0);
    }
    
    }





       
       
        
       