using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleManager : MonoBehaviour
{
    public GameObject BlockEntry;
    public GameObject BeforeWar;
    public GameObject Boss;
    public GameObject Goal;
    bool AlreadyTriggered;
    bool CheckIfBossAlive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIfBossAlive)
        {
            if (Boss == null)
            {
                Goal.SetActive(true);
                CheckIfBossAlive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!AlreadyTriggered)
        {
            if (other.tag == "Player")
            {
                BlockEntry.SetActive(true);
                Boss.SetActive(true);
                BeforeWar.SetActive(false);
                CheckIfBossAlive = true;
                AlreadyTriggered = true;
            }
        }
    }
}
