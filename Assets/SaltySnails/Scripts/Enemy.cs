using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameObject TheEnemy;
    NavMeshAgent nav;
    GameObject Target;
    Animator anim;

    public float WalkSpeed = 2;
    public float RunSpeed = 5;
    public float ScanRadius = 10;
    public float ScanCooldown = 2;
    float scanCooldown;
    public float RoamRadius = 10;
    public float RoamCooldown = 5;
    float roamCooldown;
    Vector3 LocationLastFrame;

    // Start is called before the first frame update
    void Start()
    {
        TheEnemy = transform.GetChild(0).gameObject;
        nav = TheEnemy.GetComponent<NavMeshAgent>();
        anim = TheEnemy.GetComponent<Animator>();
        LocationLastFrame = TheEnemy.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Target == null)
        {
            roam();
            scan();
        }
        else { attack(); }
    }

    void attack()
    {
        nav.speed = RunSpeed;
        nav.SetDestination(Target.transform.position);
        float distance = Vector3.Distance(TheEnemy.transform.position, Target.transform.position);
        if (distance < nav.stoppingDistance) { RotateTowards(Target.transform); anim.SetInteger("Walk", 0); }
        else { anim.SetInteger("Walk", 2); }      
        if (distance > ScanRadius * 1.5f) { Target = null; }
    }
    void scan()
    {
        if (scanCooldown > 0) { scanCooldown -= Time.deltaTime; }
        else
        {
            scanCooldown = ScanCooldown;
            Collider[] hitColliders = Physics.OverlapSphere(TheEnemy.transform.position, ScanRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Player")
                {
                    Target = hitCollider.gameObject;
                }
            }

        }
    }
    void roam()
    {
        nav.speed = WalkSpeed;
        if (TheEnemy.transform.position==LocationLastFrame)
        {
            anim.SetInteger("Walk", 0);
        }
        else
        {
            LocationLastFrame = TheEnemy.transform.position;
            anim.SetInteger("Walk", 1);
        }
        if (roamCooldown > 0) { roamCooldown -= Time.deltaTime; }
        else
        {
            roamCooldown = Random.Range(0,RoamCooldown);
            float x = Random.Range(-RoamRadius, RoamRadius);
            float z = Random.Range(-RoamRadius + 0.1f, RoamRadius);
            Vector3 MoveTo = new Vector3(TheEnemy.transform.position.x + x, TheEnemy.transform.position.y, TheEnemy.transform.position.z + z);
            nav.SetDestination(MoveTo);
        }
    }
    private void RotateTowards(Transform target)
    {
        Vector3 targetlocation = new Vector3(target.transform.position.x, TheEnemy.transform.position.y, target.transform.position.z);
        Vector3 direction = (targetlocation - TheEnemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        TheEnemy.transform.rotation = Quaternion.Slerp(TheEnemy.transform.rotation, lookRotation, Time.deltaTime * nav.angularSpeed);       
    }
}
