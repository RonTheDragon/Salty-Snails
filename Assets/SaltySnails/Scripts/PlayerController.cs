using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public GameObject AttackButton;
    public SaltRain saltRain;
    public Health health;
    public ParticleSystem SnailBlast;
    GameObject Snail;
    CharacterController CC;
    Animator anim;
    SkinnedMeshRenderer SMR;

    //camera stuff
    Camera cam;
    GameObject Tripod;
    GameObject CameraHolder;
    public float CameraDist;
    [Range(0,1)]
    public float CameraUp;
    public LayerMask CLM;

    public float Speed = 10;
    public float RotateSpeed = 20;
    public float Gravity = 10;
    public float RainDamageMultiplayer = 2;
    public TextMeshProUGUI ShellButtonTxt;

    float ComboResetTimer = 3;
    float ComboResetTime;
    float AttackCooldown;
    Vector3 ForceTowards;
    float ForceTime;  
    public int ComboCount;

    [HideInInspector]
    public bool InShell = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Snail = transform.GetChild(0).gameObject;
        SMR = Snail.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        CC = Snail.GetComponent<CharacterController>();
        health = Snail.GetComponent<Health>();
        anim = Snail.GetComponent<Animator>();
        cam = Camera.main;
        Tripod = cam.transform.parent.gameObject;
        CameraHolder = Tripod.transform.parent.gameObject;
        ShellButtonTxt.text = "Enter Shell";
        CameraHolder.transform.rotation = Quaternion.LookRotation(Snail.transform.forward - (Snail.transform.up * CameraUp));
    }

    // Update is called once per frame
    void Update()
    {
        if (!InShell)
        {
            Movement();
            RainDamage();
        }
        Look();
        Combat();
        ChangeColor();
    }
    void Movement()
    {
        float Moist = health.Hp / health.MaxHp;

        if (joystick.Vertical != 0) { CC.Move(Snail.transform.forward * Speed * Moist * joystick.Vertical * Time.deltaTime); anim.SetBool("Walk", true); }
        else { anim.SetBool("Walk", false); }

        if (joystick.Horizontal != 0) { Snail.transform.rotation *=  Quaternion.Euler(0, joystick.Horizontal* RotateSpeed * Time.deltaTime, 0); }

        if (!CC.isGrounded) { CC.Move(Vector3.down * 10 * Time.deltaTime); }
        else { CC.Move(Vector3.down * 0.1f * Time.deltaTime); }

    }
    void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(CameraHolder.transform.position,-CameraHolder.transform.forward,out hit,CameraDist, CLM))
        {
            Tripod.transform.position = hit.point;
        }
        else
        {
            Tripod.transform.position = CameraHolder.transform.position - CameraHolder.transform.forward * CameraDist;
        }
        if (InShell)
        {
            if (joystick.Vertical != 0) { CameraHolder.transform.rotation *= Quaternion.Euler(joystick.Vertical * -RotateSpeed * Time.deltaTime,0, 0); }
            if (joystick.Horizontal != 0) { CameraHolder.transform.rotation *= Quaternion.Euler(0, joystick.Horizontal * RotateSpeed * Time.deltaTime, 0); }
            Vector3 eulerRotation = CameraHolder.transform.rotation.eulerAngles;
            float ry = eulerRotation.x;
            if (ry >= 180) ry -= 360;
            ry = Mathf.Clamp(ry, -45, 45);

            CameraHolder.transform.rotation = Quaternion.Euler(ry, eulerRotation.y, 0);
        }
        else
        {
            CameraHolder.transform.rotation = Quaternion.Slerp(CameraHolder.transform.rotation, Quaternion.LookRotation(Snail.transform.forward-(Snail.transform.up* CameraUp)), Time.deltaTime * 10);
        }
    }
    public void ShellToggle()
    {
        if (AttackCooldown <= 0)
        {
            if (InShell)
            {
                anim.SetBool("Shell", false);
                ShellButtonTxt.text = "Enter Shell";
                AttackButton.SetActive(true);
            }
            else
            {
                ComboCount = 0;
                anim.SetBool("Shell", true);
                anim.SetTrigger("EnterShell");
                ShellButtonTxt.text = "Exit Shell";
                AttackButton.SetActive(false);
            }
            InShell = !InShell;
        }
    }
    void RainDamage()
    {
        if (saltRain.SaltAmount > 0)
        {
            if (!Physics.Raycast(Snail.transform.position+Vector3.up, Vector3.up,CLM))
            {
                health.Hp -= saltRain.SaltAmount * RainDamageMultiplayer * Time.deltaTime;
            }
        }
    }
    void ChangeColor()
    {
        Color c = SMR.materials[1].color;
        c.r = 1-health.Hp/health.MaxHp;
        SMR.materials[1].color = c ;
        c = SMR.materials[0].color;
        c.g = 1 - health.Hp / health.MaxHp;
        SMR.materials[0].color = c;
    }

    void Combat()
    {
        if (ForceTime > 0)
        {
            CC.Move(ForceTowards * Time.deltaTime);
            ForceTime -= Time.deltaTime;
        }
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
        if (ComboResetTime > 0)
        {
            ComboResetTime -= Time.deltaTime;
        }
        else
        {
            ComboCount = 0;
        }
    }

    public void Attack()
    {
        if (AttackCooldown <= 0)
        {         

            if (ComboCount == 0)
            {
                ForceTime = 1;
                ForceTowards = Snail.transform.forward;
                anim.SetTrigger("Attack1");
                AttackCooldown = 0.8f;
            }
            if (ComboCount == 1)
            {
                ForceTime = 0.5f;
                ForceTowards = Snail.transform.forward*2;
                anim.SetTrigger("Attack2");
                AttackCooldown = 0.6f;
            }
            if (ComboCount == 2)
            {
                ForceTime = 1;
                ForceTowards = Snail.transform.forward*4;
                anim.SetTrigger("Attack3");
                AttackCooldown = 1;
                ComboCount=-1;
                StartCoroutine(TriggerSnailBlast());
            }
            ComboCount++;
            ComboResetTime = ComboResetTimer;
        }
    }

    public IEnumerator TriggerSnailBlast()
    {
        yield return new WaitForSeconds(0.8f);
        SnailBlast.Emit(100);
    }

    public bool IsDoingSpecialAttack()
    {
        return AttackCooldown > 0 && ComboCount == 0;
    }

    public void SkipLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
