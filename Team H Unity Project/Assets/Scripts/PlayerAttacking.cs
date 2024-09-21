using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{

    [SerializeField] private GameObject melee;
    [SerializeField] private GameObject ranged;
    [SerializeField] private float mTimer;
    [SerializeField] private float rTimer;
    [SerializeField] private bool attacking;
    [SerializeField] private bool attackWhileMove;

    // Start is called before the first frame update
    void Start()
    {
        melee.SetActive(false);
        ranged.SetActive(false);
        mTimer = 0.0f;
        rTimer = 0.0f;
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotating
        if (!attacking || attackWhileMove)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 90);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 180);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                this.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
        }
        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            melee.SetActive(true);
            mTimer = .5f;
            attacking = true;
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            ranged.SetActive(true);
            rTimer = .5f;
            attacking = true;
        }


        if (rTimer > 0f)
        {
            rTimer -= Time.deltaTime;
        }
        else
        {
            ranged.SetActive(false);
        }

        if (mTimer > 0f)
        {
            mTimer -= Time.deltaTime;
        }
        else
        {
            melee.SetActive(false);
        }

        if(rTimer <= 0 && mTimer <= 0)
        {
            attacking = false;
        }

    }
}
