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

    [HideInInspector] public bool changingRooms;
    private float lerpAmount = 0;
    private GameObject exitRoom;
    private GameObject enterRoom;

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
        //if changing rooms, lerp the camera from the old room's cameraTransform to the new one
        if(changingRooms)
        {
            lerpAmount += .1f;
            mainCamera.transform.position = Vector3.Lerp(exitRoom.GetComponent<RoomManager>().cameraTransform.position, enterRoom.GetComponent<RoomManager>().cameraTransform.position, lerpAmount);
            
            //Once the lerp is done, set changingRooms to false is disable self if the room isn't clean
            if (lerpAmount >= 1)
            {
                lerpAmount = 0;
                changingRooms = false;

                if(!enterRoom.GetComponent<RoomManager>().roomClean)
                {
                    this.gameObject.SetActive(false);
                    player.GetComponent<Damageable>().health = player.GetComponent<Damageable>().maxHealth;
                }

            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {

        //If the player collides with the door
        if (collider.gameObject.GetComponent<PlayerMovementAndAttack>() != null)
        {
            changingRooms = true;

            //Checks if the player is in room 1 or 2, then switches which room is enables, moves the player
            //and finally sets which room they are exiting and which they are entering
            if (room1.activeSelf)
            {
                exitRoom = room1;
                enterRoom = room2;
                room1.SetActive(false);
                room2.SetActive(true);

                player.transform.position -= ((player.transform.position - room2.GetComponent<RoomManager>().cameraTransform.position)/2);
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
                //mainCamera.transform.position = room2.GetComponent<RoomManager>().cameraTransform.position;
            }
            else
            {
                exitRoom = room2;
                enterRoom = room1;
                room1.SetActive(true);
                room2.SetActive(false);

                player.transform.position -= ((player.transform.position - room1.GetComponent<RoomManager>().cameraTransform.position)/2);
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
                //mainCamera.transform.position = room1.GetComponent<RoomManager>().cameraTransform.position;

            }
        }
    }
}
