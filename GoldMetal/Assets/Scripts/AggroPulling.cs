using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroPulling : MonoBehaviour
{
    public float aggroRange = 10f; // 인스펙터에서 조절할 수 있는 어그로 범위
    private bool isAggroed = false; // 어그로 상태를 나타내는 플래그

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < aggroRange && !isAggroed)
            {
                isAggroed = true;
                Enemy enemy = GetComponentInParent<Enemy>();

                if (enemy != null)
                {
                    enemy.SetTarget(other.transform);
                }

                // 어그로 풀링이 완료되면 비활성화
                gameObject.SetActive(false);
            }
        }
    }
}
