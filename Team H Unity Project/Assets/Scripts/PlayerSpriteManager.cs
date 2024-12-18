using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using static PlayerMovementAndAttack;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements;

public class PlayerSpriteManager : MonoBehaviour
{
    [Header("Sprite Renderer Variables")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private SpriteRenderer p1ArmSpriteRenderer;
    [SerializeField] private SpriteRenderer p2ArmSpriteRenderer;

    [Header("Component and GameObject Vairables")]
    [SerializeField] private PlayerMovementAndAttack playerMovementAndAttack;
    [SerializeField] private GameObject characterCenter;
    [SerializeField] private GameObject p1Center;
    [SerializeField] private GameObject p2Center;

    [Header("Player Sprite Variables")]
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    [Header("P1 Arm Variables")]
    [SerializeField] private Sprite p1Down;
    [SerializeField] private Sprite p1Up;
    [SerializeField] private Sprite p1Left;
    [SerializeField] private Sprite p1Right;

    [Header("P2 Arm Variables")]
    [SerializeField] private Sprite p2Down;
    [SerializeField] private Sprite p2Up;
    [SerializeField] private Sprite p2Left;
    [SerializeField] private Sprite p2Right;

    [Header("P1 Pip Variables")]
    private Color fadedP1PipColor;
    [SerializeField] private SpriteRenderer p1Pip1Renderer;
    [SerializeField] private SpriteRenderer p1Pip2Renderer;
    [SerializeField] private SpriteRenderer p1Pip3Renderer;
    private Vector3 p1PipOriginalScale;

    [Header("P2 Pip Variables")]
    private Color fadedP2PipColor;
    [SerializeField] private SpriteRenderer p2Pip1Renderer;
    [SerializeField] private SpriteRenderer p2Pip2Renderer;
    [SerializeField] private SpriteRenderer p2Pip3Renderer;
    private Vector3 p2PipOriginalScale;

    [Header("Invulnerable Flash Variables")]
    [SerializeField] private float invulFlashMinAlpha;
    [SerializeField] private float invulFlashRate;
    private float invulFlashTimer = 0f;
    private bool isInvulFlashing = false;
    private Color originalPlayerCharacterColor;
    private Color fadedPlayerColor;

    [Header("Damage Vignette Variables")]
    [SerializeField] private float maxVignetteAlpha;
    private Color vignetteColor;
    private UnityEngine.UI.Image playerDamageVignetteImage;
    [SerializeField] private float vignetteFadeDuration;
    private float vignetteFadeTimer;

    [Header("Damage Hitstop Variable")]
    [SerializeField] private float damageHitStopTime;

    [Header("Player Particle Variables")]
    [SerializeField] private TrailRenderer inSyncTrail;
    [SerializeField] private ParticleSystem outSyncParticles;
    [SerializeField] private ParticleSystem opposingParticles;

    // Start is called before the first frame update
    void Start()
    {
        //set player sprite to front sprite
        playerSpriteRenderer.sprite = downSprite;

        //define fadedP1PipColor as white with 0.2 alpha
        fadedP1PipColor = Color.white;
        fadedP1PipColor.a = 0.2f;

        //save orignal scale of p1 pips
        p1PipOriginalScale = p1Pip1Renderer.gameObject.transform.localScale;

        //define fadedP2PipColor as white with 0.2 alpha
        fadedP2PipColor = Color.white;
        fadedP2PipColor.a = 0.2f;

        //save orignal scale of p2 pips
        p2PipOriginalScale = p2Pip1Renderer.gameObject.transform.localScale;

        //make note of player's original color
        originalPlayerCharacterColor = playerSpriteRenderer.color;

        //calculate fadedPlayerColor
        fadedPlayerColor = originalPlayerCharacterColor;
        fadedPlayerColor.a = invulFlashMinAlpha;

        //try to find PlayerDamageVignette
        playerDamageVignetteImage = GameObject.Find("PlayerDamageVignette").GetComponent<UnityEngine.UI.Image>();

        //calculate fadedVignette
        vignetteColor = playerDamageVignetteImage.color;
        vignetteColor.a = maxVignetteAlpha;

        //stop vignetteFadeTimer
        vignetteFadeTimer = vignetteFadeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        #region Invulnerability Flash Timer Update

        //if player is flashing,...
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
                //"flip" player character's opacity between original opacity and faded opacity
                if (playerSpriteRenderer.color == originalPlayerCharacterColor)
                {
                    playerSpriteRenderer.color = fadedPlayerColor;
                    p1ArmSpriteRenderer.color = fadedPlayerColor;
                    p2ArmSpriteRenderer.color = fadedPlayerColor;
                }
                else
                {
                    playerSpriteRenderer.color = originalPlayerCharacterColor;
                    p1ArmSpriteRenderer.color = originalPlayerCharacterColor;
                    p2ArmSpriteRenderer.color = originalPlayerCharacterColor;
                }

                //restart invulFlashTimer
                invulFlashTimer = invulFlashRate;
            }
        }
        //else player is NOT flashing,...
        else
        {
            //set player character's opacity back to original opacity
            playerSpriteRenderer.color = originalPlayerCharacterColor;
            p1ArmSpriteRenderer.color = originalPlayerCharacterColor;
            p2ArmSpriteRenderer.color = originalPlayerCharacterColor;
        }

        #endregion

        #region Damage Vignette Timer Update

        //if vignetteFadeDuration has not yet elapsed,...
        if (vignetteFadeTimer < vignetteFadeDuration)
        {
            //lerp between vignetteColor and fully transparent
            playerDamageVignetteImage.color = Color.Lerp(vignetteColor, Color.clear, vignetteFadeTimer / vignetteFadeDuration);

            //increment vignetteFadeTimer
            vignetteFadeTimer += Time.deltaTime;
        }

        #endregion

        #region Player Character Sprite
        //Debug.Log(playerMovementAndAttack.characterCenter.transform.localEulerAngles);

        //compare the z rotation of the character's center object to determine the character's sprite
        switch (characterCenter.transform.localEulerAngles.z)
        {
            //character is facing up
            case 0f:
                //change character sprite to upSprite
                playerSpriteRenderer.sprite = upSprite;
                break;

            //character is facing down
            case 180f:
                //change character sprite to downSprite
                playerSpriteRenderer.sprite = downSprite;
                break;

            //character is facing left
            case 90f:
                //change character sprite to leftSprite
                playerSpriteRenderer.sprite = leftSprite;
                break;

            //character is facing right
            case 270f:
                //change character sprite to rightSprite
                playerSpriteRenderer.sprite = rightSprite;
                break;

            //character is facing up and right
            case -45f:
                //change character sprite to rightSprite
                playerSpriteRenderer.sprite = rightSprite;
                break;

            //character is facing up and left
            case 45f:
                //change character sprite to leftSprite
                playerSpriteRenderer.sprite = leftSprite;
                break;

            //character is facing down and left
            case 135f:
                //change character sprite to leftSprite
                playerSpriteRenderer.sprite = leftSprite;
                break;

            //character is facing down and right
            case -135f:
                //change character sprite to rightSprite
                playerSpriteRenderer.sprite = rightSprite;
                break;
        }
        #endregion

        #region P1 Arm Sprite
        //compare the z rotation of P1's center object to determine the P1's arm sprite
        switch (p1Center.transform.localEulerAngles.z)
        {
            //P1 Arm is facing up
            case 0f:
                //change P1 Arm sprite to p1Up
                p1ArmSpriteRenderer.sprite = p1Up;
                break;

            //P1 Arm is facing down
            case 180f:
                //change P1 Arm sprite to downSprite
                p1ArmSpriteRenderer.sprite = p1Down;
                break;

            //P1 Arm is facing left
            case 90f:
                //change P1 Arm sprite to p1Left
                p1ArmSpriteRenderer.sprite = p1Left;
                break;

            //P1 Arm is facing right
            case 270f:
                //change P1 Arm sprite to p1Right
                p1ArmSpriteRenderer.sprite = p1Right;
                break;
        }
        #endregion

        #region P2 Arm Sprite
        //compare the z rotation of P2's center object to determine the P2's arm sprite
        switch (p2Center.transform.localEulerAngles.z)
        {
            //P2 Arm is facing up
            case 0f:
                //change P2 Arm sprite to p2Up
                p2ArmSpriteRenderer.sprite = p2Up;
                break;

            //P2 Arm is facing down
            case 180f:
                //change P2 Arm sprite to downSprite
                p2ArmSpriteRenderer.sprite = p2Down;
                break;

            //P2 Arm is facing left
            case 90f:
                //change P2 Arm sprite to p2Left
                p2ArmSpriteRenderer.sprite = p2Left;
                break;

            //P2 Arm is facing right
            case 270f:
                //change P2 Arm sprite to p2Right
                p2ArmSpriteRenderer.sprite = p2Right;
                break;
        }
        #endregion

        #region P1 Ammo Pips
        //compare P1's ammo
        switch (playerMovementAndAttack.p1Ammo)
        {
            //P1 was 0 ammo
            case 0:
                //fade all 3 pips
                p1Pip1Renderer.color = fadedP1PipColor;
                p1Pip2Renderer.color = fadedP1PipColor;
                p1Pip3Renderer.color = fadedP1PipColor;

                //scale up pip1 over time and shrink pip2 and pip3
                p1Pip1Renderer.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p1PipOriginalScale, playerMovementAndAttack.tempP1AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);
                p1Pip2Renderer.gameObject.transform.localScale = Vector3.zero;
                p1Pip3Renderer.gameObject.transform.localScale = Vector3.zero;

                break;

            //P1 was 1 ammo
            case 1:
                //intensify lower pip
                p1Pip1Renderer.color = Color.white;

                //fade 2 higher pips
                p1Pip2Renderer.color = fadedP1PipColor;
                p1Pip3Renderer.color = fadedP1PipColor;

                //scale up pip2 over time and shrink pip3
                p1Pip1Renderer.gameObject.transform.localScale = p1PipOriginalScale;
                p1Pip2Renderer.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p1PipOriginalScale, playerMovementAndAttack.tempP1AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);
                p1Pip3Renderer.gameObject.transform.localScale = Vector3.zero;

                break;

            //P1 was 2 ammo
            case 2:
                //intensify 2 lower pips
                p1Pip1Renderer.color = Color.white;
                p1Pip2Renderer.color = Color.white;

                //fade 1 higher pip
                p1Pip3Renderer.color = fadedP1PipColor;

                //scale up pip3 over time
                p1Pip1Renderer.gameObject.transform.localScale = p1PipOriginalScale;
                p1Pip2Renderer.gameObject.transform.localScale = p1PipOriginalScale;
                p1Pip3Renderer.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p1PipOriginalScale, playerMovementAndAttack.tempP1AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);

                break;

            //P1 was 3 ammo
            case 3:
                //intensify all 3 pips
                p1Pip1Renderer.color = Color.white;
                p1Pip2Renderer.color = Color.white;
                p1Pip3Renderer.color = Color.white;

                //keep all pips at original scale
                p1Pip1Renderer.gameObject.transform.localScale = p1PipOriginalScale;
                p1Pip2Renderer.gameObject.transform.localScale = p1PipOriginalScale;
                p1Pip3Renderer.gameObject.transform.localScale = p1PipOriginalScale;

                break;
        }
        #endregion

        #region P2 Ammo Pips
        //compare P2's ammo
        switch (playerMovementAndAttack.p2Ammo)
        {
            //P2 was 0 ammo
            case 0:
                //fade all 3 pips
                p2Pip1Renderer.color = fadedP2PipColor;
                p2Pip2Renderer.color = fadedP2PipColor;
                p2Pip3Renderer.color = fadedP2PipColor;

                //scale up pip1 over time and shrink pip2 and pip3
                p2Pip1Renderer.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p2PipOriginalScale, playerMovementAndAttack.tempP2AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);
                p2Pip2Renderer.gameObject.transform.localScale = Vector3.zero;
                p2Pip3Renderer.gameObject.transform.localScale = Vector3.zero;

                break;

            //P1 was 1 ammo
            case 1:
                //intensify lower pip
                p2Pip1Renderer.color = Color.white;

                //fade 2 higher pips
                p2Pip2Renderer.color = fadedP2PipColor;
                p2Pip3Renderer.color = fadedP2PipColor;

                //scale up pip2 over time and shrink pip3
                p2Pip1Renderer.gameObject.transform.localScale = p2PipOriginalScale;
                p2Pip2Renderer.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p2PipOriginalScale, playerMovementAndAttack.tempP2AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);
                p2Pip3Renderer.gameObject.transform.localScale = Vector3.zero;

                break;

            //P1 was 2 ammo
            case 2:
                //intensify 2 lower pips
                p2Pip1Renderer.color = Color.white;
                p2Pip2Renderer.color = Color.white;

                //fade 1 higher pip
                p2Pip3Renderer.color = fadedP2PipColor;

                //scale up pip3 over time
                p2Pip1Renderer.gameObject.transform.localScale = p2PipOriginalScale;
                p2Pip2Renderer.gameObject.transform.localScale = p2PipOriginalScale;
                p2Pip3Renderer.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p2PipOriginalScale, playerMovementAndAttack.tempP2AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);

                break;

            //P1 was 3 ammo
            case 3:
                //intensify all 3 pips
                p2Pip1Renderer.color = Color.white;
                p2Pip2Renderer.color = Color.white;
                p2Pip3Renderer.color = Color.white;

                //keep all pips at original scale
                p2Pip1Renderer.gameObject.transform.localScale = p2PipOriginalScale;
                p2Pip2Renderer.gameObject.transform.localScale = p2PipOriginalScale;
                p2Pip3Renderer.gameObject.transform.localScale = p2PipOriginalScale;

                break;
        }
        #endregion

        #region Player Particles

        //if players are moving in the same direction,...
        if(playerMovementAndAttack.p1MoveDir == playerMovementAndAttack.p2MoveDir)
        {
            //if both players are not moving,...
            if(playerMovementAndAttack.p1MoveDir == Vector2.zero)
            {
                //play nothing
                StopAllPlayerParticles();
            }
            //else both players are moving the same direction and are non-zero,...
            else
            {
                //play in-sync trail
                PlayInSyncTrail();
            }
        }
        //else if one player is not moving while the other is,...
        else if((playerMovementAndAttack.p1MoveDir != Vector2.zero && playerMovementAndAttack.p2MoveDir == Vector2.zero) || (playerMovementAndAttack.p1MoveDir == Vector2.zero && playerMovementAndAttack.p2MoveDir != Vector2.zero))
        {
            //play out-sync particles
            PlayOutSyncParticles();
        }
        //else if players are moving in opposing directions,...
        else if(playerMovementAndAttack.p1MoveDir + playerMovementAndAttack.p2MoveDir == Vector2.zero)
        {
            //play oppising particles
            PlayOpposingParticles();
        }
        //else player are moving in orthoganal directions,...
        else
        {
            //play out-sync particles
            PlayOutSyncParticles();
        }

        #endregion
    }

    //***** Makes the player start to flash between transparent and opaque to signify they are invulnerable *****
    public void StartPlayerDamageVFX()
    {
        //set isInvulFlashing to true
        isInvulFlashing = true;

        //make damage vignette visible
        if (playerDamageVignetteImage != null)
            playerDamageVignetteImage.color = vignetteColor;

        //hitstop for damageHitStopTime seconds
        StartCoroutine(HitStop(damageHitStopTime));

        //start vignetteFadeDuration
        vignetteFadeTimer = 0f;
    }

    //***** Makes the player stop flashing between transparent and opaque to signify they are no longer invulnerable *****
    public void EndPlayerInvulFlashVFX()
    {
        //set isInvulFlashing to false
        isInvulFlashing = false;
    }

    //***** Stop time for hitStopTime seconds *****
    IEnumerator HitStop(float hitStopTime)
    {
        //save original time scale
        float originalTimeScale = Time.timeScale;

        //set timescale to 0
        Time.timeScale = 0f;

        //wait for hitstop seconds
        yield return new WaitForSecondsRealtime(hitStopTime);

        //set timescale back to original time scale
        Time.timeScale = originalTimeScale;
    }

    private void PlayInSyncTrail()
    {
        //turn on in-sync trail
        inSyncTrail.emitting = true;

        //turn off out-sync particles
        if (outSyncParticles.isPlaying)
            outSyncParticles.Stop();

        //turn off opposing partilces
        if (opposingParticles.isPlaying)
            opposingParticles.Stop();
    }

    private void PlayOutSyncParticles()
    {
        //turn off in-sync trail
        inSyncTrail.emitting = false;

        //turn on out-sync particles
        if (!outSyncParticles.isPlaying)
            outSyncParticles.Play();

        //turn off opposing partilces
        if (opposingParticles.isPlaying)
            opposingParticles.Stop();
    }

    private void PlayOpposingParticles()
    {
        //turn off in-sync trail
        inSyncTrail.emitting = false;

        //turn off out-sync particles
        if (outSyncParticles.isPlaying)
            outSyncParticles.Stop();

        //turn on opposing partilces
        if (!opposingParticles.isPlaying)
            opposingParticles.Play();
    }

    private void StopAllPlayerParticles()
    {
        //turn off in-sync trail
        inSyncTrail.emitting = false;

        //turn off out-sync particles
        if (outSyncParticles.isPlaying)
            outSyncParticles.Stop();

        //turn off opposing partilces
        if (opposingParticles.isPlaying)
            opposingParticles.Stop();
    }
}