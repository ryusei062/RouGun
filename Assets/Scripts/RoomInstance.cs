using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInstance : MonoBehaviour {
    public static RoomInstance roominstance = null;
    [SerializeField]
    public GameObject doorU, doorD, doorL, doorR, doorUWall, doorDWall, doorLWall, doorRWall,
        uWall, dWall, lWall, rWall, enemy, enemy2, enemy3, bossEnemy, bossEnemy2, item1, item2,
        item3, Bonfire, stair, bossRoom,monsterHouse;
    public int maptype;
}
