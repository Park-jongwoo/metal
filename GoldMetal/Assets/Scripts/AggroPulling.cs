using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroPulling : MonoBehaviour
{
    public float aggroRange = 10f; // �ν����Ϳ��� ������ �� �ִ� ��׷� ����
    private bool isAggroed = false; // ��׷� ���¸� ��Ÿ���� �÷���

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

                // ��׷� Ǯ���� �Ϸ�Ǹ� ��Ȱ��ȭ
                gameObject.SetActive(false);
            }
        }
    }
}
