using UnityEngine;
using UnityEngine.InputSystem;


/*
 * プレイヤーを中心にカメラを回転させる
 * 参考：https://qiita.com/maid_chan/items/2cf76e76288b3657c8e9
 * **/
public class CameraRotat : MonoBehaviour
{
    [SerializeField]
    GameObject playerObject;         //追尾 オブジェクト
    [SerializeField, Range(0, 1)]
    //private Vector2 rotationSpeed;           //回転速度
    float rotationSpeed=1.0f;
    Vector3 lastMousePosition;      //最後のマウス座標
    Vector3 lastTargetPosition;     //最後の追尾オブジェクトの座標


    float zoom;

    void Start()
    {
        zoom = 0.0f;
        lastMousePosition = Input.mousePosition;
        lastTargetPosition = playerObject.transform.position;
    }

    void Update()
    {
        Chase();
        Rotate();
        Zoom();
    }

    //PL追従
    void Chase()
    {
        transform.position += playerObject.transform.position - lastTargetPosition;
        lastTargetPosition = playerObject.transform.position;
    }

    //マウスの左右移動で回転
    void Rotate()
    {
        var newAngle = Vector3.zero;
        //マウスの移動量記録
        Vector3 nowMouseValue = Input.mousePosition - lastMousePosition;

        //回転量計算
        newAngle.x = rotationSpeed * nowMouseValue.x;

        //PLを中心に回転（中心,回転軸,回転角度）
        transform.RotateAround(playerObject.transform.position, Vector3.up, newAngle.x);
        lastMousePosition = Input.mousePosition;

    }


    //拡大縮小
    void Zoom()
    {
        zoom = Input.GetAxis("Mouse ScrollWheel");
        Vector3 offset = new Vector3(0, 0, 0);
        Vector3 pos = playerObject.transform.position - transform.position;

        if (zoom > 0)
        {
            offset = pos.normalized * 1;
        }
        else if (zoom < 0)
        {
            offset = -pos.normalized * 1;

        }
        transform.position = transform.position + offset;
    }

}
