using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltRain : MonoBehaviour
{
    ParticleSystem[] Rain = new ParticleSystem[2];
    public float RainMinCooldown  = 10;
    public float RainMaxCooldown  = 20;
    public float noRainMinCooldown = 20;
    public float noRainMaxCooldown = 30;
    public int MaxSalt  = 10;
    public float Strengthening  = 2;
    public float Weakening  = 3;
    [HideInInspector]
    public float SaltAmount;
    float RainCooldown;
    bool IsRaining = true;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Rain.Length; i++)
        {
            Rain[i] = transform.GetChild(i).GetComponent<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        WeatherSystem();
        Raining();
    }

    void WeatherSystem()
    {
        if (RainCooldown > 0)
        {
            RainCooldown -= Time.deltaTime;
        }
        else
        {
            if(IsRaining)
            {
                RainCooldown = Random.Range(noRainMinCooldown, noRainMaxCooldown);
            }
            else
            {
                RainCooldown = Random.Range(RainMinCooldown, RainMaxCooldown);              
            }
            IsRaining = !IsRaining;
        }
    }
    void Raining()
    {
        
        foreach (ParticleSystem r in Rain)
        {
                r.Emit((int)SaltAmount);
        }
        
        if (IsRaining)
        {
            if (SaltAmount < MaxSalt)
            {
                SaltAmount += Strengthening * Time.deltaTime;
            }
        }
        else
        {
            if (SaltAmount > 0)
            {
                SaltAmount -= Weakening * Time.deltaTime;
            }
        }

        
    }
}
