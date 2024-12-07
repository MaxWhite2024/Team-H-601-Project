using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpriteManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bossSpriteRenderer;

    [Header("Sprites")]
    [SerializeField] private Sprite initial;
    [SerializeField] private Sprite second;
    [SerializeField] private Sprite third;
    [SerializeField] private Sprite fourth; 
    [SerializeField] private Sprite final;
    private byte sprite = 0;

    [Header("Invulnerable Flash Variables")]
    [SerializeField] private float invulFlashMinAlpha;
    [SerializeField] private float invulFlashRate;
    private float invulFlashTimer = 0f;
    private bool isInvulFlashing = false;
    private Color originalBossColor;
    private Color fadedBossColor;

    private float startScale;
    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale.x;

        //make note of boss' original color
        originalBossColor = bossSpriteRenderer.color;

        //calculate fadedBossColor
        fadedBossColor = originalBossColor;
        fadedBossColor.a = invulFlashMinAlpha;
    }

    // Update is called once per frame
    void Update()
    {
        //if boss is flashing,...
        if (isInvulFlashing)
        {
            //if invulFlashTimer is greater than 0,...
            if (invulFlashTimer > 0)
            {
                //decrement invulFlashTimer
                invulFlashTimer -= Time.deltaTime;
            }
            //else invulFlashTimer is less than or equal to 0,...
            else
            {
                //"flip" boss' opacity between original opacity and faded opacity
                if (bossSpriteRenderer.color == originalBossColor)
                {
                    bossSpriteRenderer.color = fadedBossColor;
                }
                else
                {
                    bossSpriteRenderer.color = originalBossColor;
                }

                //restart invulFlashTimer
                invulFlashTimer = invulFlashRate;
            }
        }
        //else boss is NOT flashing,...
        else
        {
            //set boss' opacity back to original opacity
            bossSpriteRenderer.color = originalBossColor;
        }
    }

    public void ChangeSprite(float healthPercent)
    {
        if (healthPercent <= .8f && sprite <= 0)
        {
            float scale = startScale * ((healthPercent / 2) + .5f);
            transform.localScale = new Vector3(scale, scale, 1);
            bossSpriteRenderer.sprite = second;
            sprite++;
        }
        else if (healthPercent <= .6f && sprite <= 1)
        {
            float scale = startScale * ((healthPercent / 2) + .5f);
            transform.localScale = new Vector3(scale, scale, 1);
            bossSpriteRenderer.sprite = third;
            sprite++;
        }
        else if (healthPercent <= .4f && sprite <= 2)
        {
            float scale = startScale * ((healthPercent / 2) + .5f);
            transform.localScale = new Vector3(scale, scale, 1);
            bossSpriteRenderer.sprite = fourth;
            sprite++;
        }
        else if (healthPercent <= .2f && sprite <= 3)
        {
            float scale = startScale * ((healthPercent / 2) + .5f);
            transform.localScale = new Vector3(scale, scale, 1);
            bossSpriteRenderer.sprite = final;
            sprite++;
        }

    }

    //***** Makes the boss start to flash between transparent and opaque to signify it is invulnerable *****
    public void StartInvulFlashVFX()
    {
        //set isInvulFlashing to true
        isInvulFlashing = true;
    }

    //***** Makes the boss stop flashing between transparent and opaque to signify it is no longer invulnerable *****
    public void EndInvulFlashVFX()
    {
        //set isInvulFlashing to false
        isInvulFlashing = false;
    }
}
