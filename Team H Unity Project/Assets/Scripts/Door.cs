using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public GameObject room1;
    [SerializeField] public GameObject room2;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;

    [Header("Debug Vars")]
    [SerializeField] private HouseManager house;
    [SerializeField] public bool open;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;

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
        if (house == null)
        {
            house = transform.parent.parent.gameObject.GetComponent<HouseManager>();
        }

        Close();
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

                house.activeRoom = enterRoom.GetComponent<RoomManager>();

                if (!enterRoom.GetComponent<RoomManager>().roomClean)
                {
                    Close();
                    //player.GetComponent<Damageable>().health = player.GetComponent<Damageable>().maxHealth; //Heals players on entering new room
                }

            }
        }
    }

    public void Open()
    {
        this.GetComponent<BoxCollider2D>().enabled = true;

        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().sprite = openSprite;
        }
    }

    public void Close()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;

        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().sprite = closeSprite;
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

                MovePlayer(exitRoom, enterRoom);
                //mainCamera.transform.position = room2.GetComponent<RoomManager>().cameraTransform.position;
            }
            else
            {
                exitRoom = room2;
                enterRoom = room1;
                room1.SetActive(true);
                room2.SetActive(false);

                MovePlayer(exitRoom, enterRoom);
                //mainCamera.transform.position = room1.GetComponent<RoomManager>().cameraTransform.position;

            }
        }
    }

    /// <summary>
    /// Moves player from exitRoom to enterRoom based on the door's localScale values
    /// </summary>
    /// <param name="exitRoom">Room the player is leaving</param>
    /// <param name="enterRoom">Room the player is entering</param>
    private void MovePlayer(GameObject exitRoom, GameObject enterRoom)
    {
        //Checks to see if the rooms are vertical or horizontal
        Vector3 roomDif = exitRoom.GetComponent<RoomManager>().cameraTransform.position - enterRoom.GetComponent<RoomManager>().cameraTransform.position;
        roomDif = Vector3.Normalize(roomDif);
        if(Mathf.Abs(roomDif.x) > Mathf.Abs(roomDif.y))
        {
            //If the player is moving left
            if(exitRoom.transform.position.x > enterRoom.transform.position.x)
            {
                player.transform.position = new Vector3(player.transform.position.x - this.gameObject.transform.localScale.x - 1, player.transform.position.y, 0);
            }
            else //else moving right
            {
                player.transform.position = new Vector3(player.transform.position.x + this.gameObject.transform.localScale.x + 1, player.transform.position.y, 0);
            }
        }
        else
        {
            //If the player is moving down
            if (exitRoom.transform.position.y > enterRoom.transform.position.y)
            {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - this.gameObject.transform.localScale.y - .5f, 0);
            }
            else //else moving up
            {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + this.gameObject.transform.localScale.y + .5f, 0);
            }
        }

        //Old code which moves regardless of where the rooms are oriented, doesn't work if rooms are too close/far apart
        //player.transform.position -= ((player.transform.position - enterRoom.GetComponent<RoomManager>().cameraTransform.position) / 2);
        //player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
    }
}
