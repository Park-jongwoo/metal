using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    /* -------------- 이벤트 함수 -------------- */
    void Update()
    {
        transform.Rotate(Vector3.right * 90 * Time.deltaTime);
    }
}
