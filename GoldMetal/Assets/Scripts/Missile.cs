using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    /* -------------- �̺�Ʈ �Լ� -------------- */
    void Update()
    {
        transform.Rotate(Vector3.right * 90 * Time.deltaTime);
    }
}
