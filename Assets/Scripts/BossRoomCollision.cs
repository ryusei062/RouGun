using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomCollision : MonoBehaviour
{
    public bool zoneInPlayer = false;

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            zoneInPlayer = true;
        }

    }
}
