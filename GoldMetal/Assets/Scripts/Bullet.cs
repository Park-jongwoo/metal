using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /* -------------- 프로퍼티 -------------- */
    public int damage;
    public bool isMelee;
    public bool isRock;


    /* -------------- 이벤트 함수 -------------- */
    void OnCollisionEnter(Collision collision)
    {
        if (!isRock && collision.gameObject.tag == "Floor")
            Destroy(gameObject, 3);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.gameObject.tag == "Wall")
            Destroy(gameObject);
    }
}
