using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    float TheKnockback;
    Vector3 TheImpactLocation;
    public GameObject SpawnOnDeath;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        TakeKnockback();
    }

    public override void TakeDamage(float damage, float knockback, Vector3 ImpactLocation)
    {
        base.TakeDamage(damage,knockback, ImpactLocation);
        TheKnockback = knockback;
        TheImpactLocation = ImpactLocation;
        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }
    }

    protected override void Death()
    {
        Instantiate(SpawnOnDeath, transform.position, transform.rotation);
        Destroy(transform.parent.gameObject);
    }

    void TakeKnockback()
    {
        if (TheKnockback > 0) { TheKnockback -= TheKnockback*Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TheImpactLocation, -TheKnockback * Time.deltaTime);
        }
    }
}
