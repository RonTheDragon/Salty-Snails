using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float Timer=1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
