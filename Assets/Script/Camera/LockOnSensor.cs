using System.Collections.Generic;
using UnityEngine;

public class LockOnSensor : MonoBehaviour
{
    public GameObject NowTarget { get;private set; }

    [SerializeField]
    List<GameObject> enemyList = new();

    private void Update()
    {
        if (enemyList.Count == 0) 
        {
            NowTarget = null;
            return;
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if(coll.gameObject.CompareTag("Target"))
        {
            NowTarget = coll.gameObject;
            enemyList.Add(NowTarget);
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if(coll.gameObject.CompareTag("Target"))
        {
            NowTarget=null;
        }
    }
}
