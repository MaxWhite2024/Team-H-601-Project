using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public GameObject doors;
    public float healthDropChance;
    public RoomManager activeRoom;
<<<<<<< Updated upstream
=======
    public string nextSceneName;
    [SerializeField] private GameObject winScreen;
>>>>>>> Stashed changes

    [Header("Debug Vars")]
    [SerializeField] private List<RoomManager> rooms = new List<RoomManager>();
    [SerializeField] private List<Door> doorsList = new List<Door>();

    // Start is called before the first frame update
    void Start()
    {
        //Populate rooms
        foreach (Transform child in transform)
        {
            RoomManager room = child.gameObject.GetComponent<RoomManager>();
            if (room != null && !rooms.Contains(room))
            {
                rooms.Add(room);
            }
        }

        //Populate doorsList
        foreach (Transform child in doors.transform)
        {
            Door door = child.gameObject.GetComponent<Door>();
            if (door != null && !doorsList.Contains(door))
            {
                doorsList.Add(door);
            }
        }

        //Populate each room's doors
        foreach (Door door in doorsList)
        {
            foreach(RoomManager room in rooms)
            {
                if((room == door.room1.GetComponent<RoomManager>() || room == door.room2.GetComponent<RoomManager>()) && !room.doors.Contains(door))
                {
                    room.doors.Add(door);   
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoomsClean()
    {
        //return if a room isn't clean
        foreach(RoomManager room in rooms)
        {
            if(!room.roomClean)
            {
                return;
            }
        }

<<<<<<< Updated upstream
        Debug.Log("ALL ROOMS CLEAN");
=======
        //Debug.Log("ALL ROOMS CLEAN");
        Win();
    }

    public void Win()
    {
        winScreen.SetActive(true);

        /*if (nextSceneName != "")
        {
            SceneManager.LoadScene(nextSceneName);
        }*/
>>>>>>> Stashed changes
    }
}
