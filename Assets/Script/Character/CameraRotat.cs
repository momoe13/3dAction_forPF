using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotat : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 0.05f;
    float mouse_X=0f;
    float mouse_Y = 0f;
    [SerializeField]
    private GameObject player;
    /*
     * マウスで縦に動く
     * 
     * **/
    void Update()
    {
        mouse_Y = Input.GetAxis("Mouse Y") * rotationSpeed;
        transform.RotateAround(player.transform.position, transform.right, mouse_Y); // X軸に対して回転させる処理

    }
}
