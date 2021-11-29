using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    GameObject Snail;
    CharacterController CC;
    public float Speed = 10;
    public float RotateSpeed = 20;
    public float Gravity = 10;
    // Start is called before the first frame update
    void Start()
    {
        Snail = transform.GetChild(0).gameObject;
        CC = Snail.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    void Movement()
    { 
        
        if (joystick.Vertical != 0) { CC.Move(Snail.transform.forward * Speed * joystick.Vertical * Time.deltaTime); }

        if (joystick.Horizontal != 0) { Snail.transform.rotation *=  Quaternion.Euler(0, joystick.Horizontal* RotateSpeed * Time.deltaTime, 0); }

        if (!CC.isGrounded) { CC.Move(Vector3.down * 10 * Time.deltaTime); }
    }
}
