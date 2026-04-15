using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField]UI_Manager manager;

    float rotatY = 20.0f;
    Vector3 nextRotat;

    private void Start()
    {
        nextRotat.y = rotatY;
    }

    private void Update()
    {
        this.gameObject.transform.Rotate(nextRotat*Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name=="Player")
        {
            manager.SetScore();
            Destroy(this.gameObject);
        }
    }
}
