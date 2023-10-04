using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : Damage
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    private void OnTriggerEnter(Collider other)
    {
       
            Health H = other.gameObject.GetComponent<Health>();
            if (H != null)
            {
                if (H != MyHealth)
                {                   
                    H.TakeDamage(AttackDamage,Knockback,transform.parent.position);
                }
            }
        
    }
}
