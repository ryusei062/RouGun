using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnterDoor : MonoBehaviour
{
    private GameObject collisionDetector;
    BossRoomCollision bossRoomCollision;


    private void Start()
    {
        collisionDetector = GameObject.Find("CollisionDetector");
        bossRoomCollision = collisionDetector.GetComponent<BossRoomCollision>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(bossRoomCollision.zoneInPlayer)
        {
            gameObject.SetActive(true);
        }
    }
}
