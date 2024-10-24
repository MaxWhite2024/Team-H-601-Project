using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerAmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text p1AmmoText;
    [SerializeField] private TMP_Text p2AmmoText;
    private PlayerMovementAndAttack playerMovementAndAttack;

    // Start is called before the first frame update
    void Start()
    {
        playerMovementAndAttack = GameObject.Find("Players").GetComponent<PlayerMovementAndAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        p1AmmoText.text = "P1 Ammo: " + playerMovementAndAttack.p1Ammo;
        p2AmmoText.text = "P2 Ammo: " + playerMovementAndAttack.p2Ammo;
    }
}
