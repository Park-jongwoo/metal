using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroPulling : MonoBehaviour
{
    public float aggroRange = 10f; // �ν����Ϳ��� ������ �� �ִ� ��׷� ����
    internal bool isAggro = false; // ��׷� ���¸� ��Ÿ���� �÷���
    internal Transform target;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("AggroPulling OnTriggerEnter called");

            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < aggroRange && !isAggro)
            {
                isAggro = true;

                Enemy enemy = GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    // ��׷� ���°� �����Ǹ� ��� ������ ���۵˴ϴ�.
                    enemy.SetTarget(other.transform);
                    StartCoroutine("AggroAttack");
                }

                // ��׷� Ǯ���� �Ϸ�Ǹ� ��Ȱ��ȭ
                gameObject.SetActive(false);
            }
        }
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
