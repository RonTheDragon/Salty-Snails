using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public float MaxHp = 100;
    public float HpRegan = 0;
    [HideInInspector]
    public float Hp;
    Vector3 startingPoint;
    Quaternion startingRotation; 
    protected Animator anim;

    // Start is called before the first frame update
    protected void Start()
    {
        anim = transform.GetComponent<Animator>(); 
        Hp = MaxHp;
        startingPoint = transform.position;
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Hp > MaxHp)
        {
            Hp = MaxHp;
        }
        else if (Hp > 0)
        {
            Hp += HpRegan * Time.deltaTime;
        }
        else
        {
            Death();
        }
    }

    public virtual void TakeDamage(float damage, float knockback,Vector3 ImpactLocation)
    {
        Hp -= damage;
        
    }

    protected abstract void Death(); 
}