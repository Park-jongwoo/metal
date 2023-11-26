using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    /* -------------- 인스펙터 -------------- */
    public Transform target; // 따라가야할 오브젝트

    /* -------------- 프로퍼티 -------------- */
    public Vector3 offset; // 따라갈 목표와 위치 오프셋을 public변수로 선언

    /* -------------- 이벤트 함수 -------------- */
    void Update()
    {
        transform.position = target.position + offset;
    }
}
