using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    /* -------------- 프로퍼티 -------------- */
    public GameManager manager;

    /* -------------- 이벤트 함수 -------------- */
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
            manager.StageStart();
    }
}
