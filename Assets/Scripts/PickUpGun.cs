using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGun : MonoBehaviour
{
    public GunStatus gunData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (GameManager.instance.items.Count < GameManager.instance.gunSlots.Length)
            {

                Destroy(gameObject);

                GameManager.instance.AddGun(gunData);
            }
            else
            {
                Debug.Log("‚©‚Î‚ñ‚ª‚¢‚Á‚Ï‚¢‚¾");
            }

        }
    }
}
