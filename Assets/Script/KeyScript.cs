using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField]UI_Manager manager;


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name=="Player")
        {
            manager.SetScore();
            Destroy(this.gameObject);
        }
    }
}
