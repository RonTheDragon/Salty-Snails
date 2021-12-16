using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    float TheKnockback;
    Vector3 TheImpactLocation;
    CharacterController CC;
    PlayerController PC;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        CC = transform.GetComponent<CharacterController>();
        PC = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        TakeKnockback();
    }

    public override void TakeDamage(float damage, float knockback, Vector3 ImpactLocation)
    {
        if (PC.InShell)
        {
            damage *= 0.2f;
            knockback *= 0.2f;
        }
        else if (!PC.IsDoingSpecialAttack())
        {
            if (anim != null)
            {
                anim.SetTrigger("Hurt");
            }
        }
        base.TakeDamage(damage, knockback, ImpactLocation);
        TheKnockback = knockback;
        TheImpactLocation = ImpactLocation;
    }

    protected override void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void TakeKnockback()
    {     
        if (TheKnockback > 0)
        {
            TheKnockback -= TheKnockback * Time.deltaTime*2;
            CC.Move((transform.position-TheImpactLocation).normalized * TheKnockback * Time.deltaTime);
        }
    }
}
