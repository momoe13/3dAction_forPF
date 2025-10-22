using UnityEngine;

public class FollowArea : MonoBehaviour
{
    [SerializeField]    
    bool GetPlayr= false;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetPlayr = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GetPlayr = false;
    }
    public bool SetFlg()
    {
        return GetPlayr; 
    }
}
