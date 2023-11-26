using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    /* -------------- ������Ƽ -------------- */
    public GameManager manager;

    /* -------------- �̺�Ʈ �Լ� -------------- */
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
            manager.StageStart();
    }
}
