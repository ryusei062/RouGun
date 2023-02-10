using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoriaDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerStatusTakeOver.floorLevel >= 2)
        {
            Destroy(gameObject);
        }
    }

}
