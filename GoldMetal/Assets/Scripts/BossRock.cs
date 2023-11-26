using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullet
{
    /* -------------- 컴퍼넌트 변수 -------------- */
    float angularPower = 2;
    float scaleValue = 0.1f;
    bool isShoot;

    Rigidbody _rigidbody;

    /* -------------- 이벤트 함수 -------------- */
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
        
    }
    /* -------------- 기능 함수 -------------- */
    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }
    IEnumerator GainPower()
    {
        while (!isShoot)
        {
            angularPower += 0.2f;
            scaleValue += 0.486f  * Time.deltaTime;
            transform.localScale = Vector3.one * scaleValue;
            _rigidbody.AddTorque(transform.right * angularPower, ForceMode.Acceleration); // 지속적으로 오릴 때 엑셀
            yield return null;
        }
    }
}
