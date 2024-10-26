using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private GameObject room1;
    [SerializeField] private GameObject room2;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;

    public bool movingCamera;
    private bool OnetoTwo = false;
    private bool TwotoOne = false;
    private float lerpAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //room2.gameObject.SetActive(false);

        if(player == null)
        {
            player = GameObject.Find("Players");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(OnetoTwo)
        {
            lerpAmount += .1f;
            mainCamera.transform.position = Vector3.Lerp(room1.GetComponent<RoomManager>().cameraTransform.position, room2.GetComponent<RoomManager>().cameraTransform.position, lerpAmount);
            if (lerpAmount >= 1)
            {
                lerpAmount = 0;
                OnetoTwo = false;
                movingCamera = false;

                if(!room2.GetComponent<RoomManager>().roomClean)
                {
                    this.gameObject.SetActive(false);
                }

            }
        }

        if (TwotoOne)
        {
            lerpAmount += .1f;
            mainCamera.transform.position = Vector3.Lerp(room2.GetComponent<RoomManager>().cameraTransform.position, room1.GetComponent<RoomManager>().cameraTransform.position, lerpAmount);
            if (lerpAmount >= 1)
            {
                lerpAmount = 0;
                TwotoOne = false;
                movingCamera = false;

                if (!room1.GetComponent<RoomManager>().roomClean)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {

        //If the player collides with the door
        if (collider.gameObject.GetComponent<PlayerMovementAndAttack>() != null)
        {
            if(room1.activeSelf)
            {
                OnetoTwo = true;
                movingCamera = true;
                room1.SetActive(false);
                room2.SetActive(true);

                player.transform.position -= ((player.transform.position - room2.GetComponent<RoomManager>().cameraTransform.position)/2);
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
                //mainCamera.transform.position = room2.GetComponent<RoomManager>().cameraTransform.position;
            }
            else
            {
                TwotoOne = true;
                movingCamera = true;
                room1.SetActive(true);
                room2.SetActive(false);

                player.transform.position -= ((player.transform.position - room1.GetComponent<RoomManager>().cameraTransform.position)/2);
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
                //mainCamera.transform.position = room1.GetComponent<RoomManager>().cameraTransform.position;

            }
        }
    }
}
