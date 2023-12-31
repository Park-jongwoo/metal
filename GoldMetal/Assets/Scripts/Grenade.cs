using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    /* -------------- 프로퍼티 -------------- */
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody _rigidbody;


    /* -------------- 기능 함수 -------------- */
    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, 
            Vector3.up, 0f, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }
        
        Destroy(gameObject, 5);
    }
}
