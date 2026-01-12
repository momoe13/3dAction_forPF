using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    Collider collider;

    [SerializeField]
    GameObject atkObj;

    [SerializeField]
    Rigidbody rb;


    [SerializeField] float speed = 10f;


    private void OnCollisionEnter(Collision collision)
    {
        ShooterScript.instance.ReleaseBullet(this);
    }


    public IEnumerator Fire(Vector3 direction, int layerNum)
    {
        atkObj.layer = layerNum;
        this.gameObject.layer = layerNum;

        yield return null;


        rb.linearVelocity = Vector3.zero; // 前回の速度を消す
        rb.angularVelocity = Vector3.zero;

        rb.linearVelocity = direction.normalized * speed;
    }
}
