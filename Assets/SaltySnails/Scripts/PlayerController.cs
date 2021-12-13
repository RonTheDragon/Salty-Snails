using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    GameObject Snail;
    CharacterController CC;
    Animator anim;

    //camera stuff
    Camera cam;
    GameObject Tripod;
    GameObject CameraHolder;
    public float CameraDist;
    public LayerMask CLM;

    public float Speed = 10;
    public float RotateSpeed = 20;
    public float Gravity = 10;
    public TextMeshProUGUI ShellButtonTxt;
    bool InShell = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Snail = transform.GetChild(0).gameObject;
        CC = Snail.GetComponent<CharacterController>();
        anim = Snail.GetComponent<Animator>();
        cam = Camera.main;
        Tripod = cam.transform.parent.gameObject;
        CameraHolder = Tripod.transform.parent.gameObject;
        ShellButtonTxt.text = "Enter Shell";
    }

    // Update is called once per frame
    void Update()
    {
        if (!InShell)
        {
            Movement();
        }
        Look();
    }
    void Movement()
    { 
        
        if (joystick.Vertical != 0) { CC.Move(Snail.transform.forward * Speed * joystick.Vertical * Time.deltaTime); anim.SetBool("Walk", true); }
        else { anim.SetBool("Walk", false); }

        if (joystick.Horizontal != 0) { Snail.transform.rotation *=  Quaternion.Euler(0, joystick.Horizontal* RotateSpeed * Time.deltaTime, 0); }

        if (!CC.isGrounded) { CC.Move(Vector3.down * 10 * Time.deltaTime); }
        else { CC.Move(Vector3.down * 0.1f * Time.deltaTime); }
    }
    void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(CameraHolder.transform.position,-CameraHolder.transform.forward,out hit,CameraDist, CLM))
        {
            Tripod.transform.position = hit.point;
        }
        else
        {
            Tripod.transform.position = CameraHolder.transform.position - CameraHolder.transform.forward * CameraDist;
        }
        if (InShell)
        {
            if (joystick.Vertical != 0) { CameraHolder.transform.rotation *= Quaternion.Euler(joystick.Vertical * -RotateSpeed * Time.deltaTime,0, 0); }
            if (joystick.Horizontal != 0) { CameraHolder.transform.rotation *= Quaternion.Euler(0, joystick.Horizontal * RotateSpeed * Time.deltaTime, 0); }
            Vector3 eulerRotation = CameraHolder.transform.rotation.eulerAngles;
            float ry = eulerRotation.x;
            if (ry >= 180) ry -= 360;
            ry = Mathf.Clamp(ry, -45, 45);

            CameraHolder.transform.rotation = Quaternion.Euler(ry, eulerRotation.y, 0);
        }
        else
        {
            CameraHolder.transform.rotation = Quaternion.Slerp(CameraHolder.transform.rotation, Quaternion.LookRotation(Snail.transform.forward), Time.deltaTime * 10);
        }
    }
    public void ShellToggle()
    {
        if (InShell)
        {
            anim.SetBool("Shell", false);
            ShellButtonTxt.text = "Enter Shell";
        }
        else
        {
            anim.SetBool("Shell", true);
            anim.SetTrigger("EnterShell");
            ShellButtonTxt.text = "Exit Shell";
        }
        InShell = !InShell;
    }
}
