using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGoal : MonoBehaviour
{
    float timer;
    public float coolDown;
    public float speed;
    bool up;
    void Update()
    {
        transform.Rotate(0, 20*Time.deltaTime, 0);
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = coolDown;
            up = !up;
        }

        if (up)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        else
        {
            transform.position -= Vector3.up * speed * Time.deltaTime;
        }              
    }
}
