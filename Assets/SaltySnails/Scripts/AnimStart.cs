using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStart : MonoBehaviour
{
    Animator anim;
    float timeDelay;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        timeDelay = Random.Range(0, 5);
    }
 
    void Update()
    {
        if(timeDelay > 0)
        {
            timeDelay -= Time.deltaTime;
        }
        else
        {
            anim.enabled = true;
        }
    }
}
