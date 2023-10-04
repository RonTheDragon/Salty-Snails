using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleMove : MonoBehaviour
{
    public float rotationSpeed;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up * -rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
