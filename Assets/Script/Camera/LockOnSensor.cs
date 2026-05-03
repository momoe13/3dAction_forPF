using System.Collections.Generic;
using UnityEngine;

public class LockOnSensor : MonoBehaviour
{
    public GameObject NowTarget { get;private set; }

    CharaBase charaBase;

   
    private void OnTriggerStay(Collider coll)
    {
        if(coll.gameObject.CompareTag("Target"))
        {
            NowTarget = coll.gameObject;
            charaBase =NowTarget.GetComponentInParent<CharaBase>();
//            Debug.Log("キャラクタークラス" + NowTarget);
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if(coll.gameObject.CompareTag("Target"))
        {
           NowTarget = null;

        }
    }

    public Vector3 GetTargetIconPos()
    {
        return charaBase.GetTargetIconPos();
    }
}
