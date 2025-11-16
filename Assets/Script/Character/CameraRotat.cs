using UnityEngine;
using UnityEngine.InputSystem;


/*
 * プレイヤーを中心にカメラを回転させる
 * 参考：https://qiita.com/maid_chan/items/2cf76e76288b3657c8e9
 * https://qiita.com/OKUDAYUKI08/items/1bd658d9eaf5c39cd99a
 * **/
public class CameraRotat : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;         //追尾 オブジェクト
    [SerializeField]
    private Vector2 rotationSpeed;           //回転速度
    private Vector3 lastMousePosition;      //最後のマウス座標
    private Vector3 lastTargetPosition;     //最後の追尾オブジェクトの座標


    private float zoom;

    void Start()
    {
        zoom = 0.0f;
        lastMousePosition = Input.mousePosition;
        lastTargetPosition = playerObject.transform.position;
    }

    void Update()
    {

        Rotate();
        Zoom();
    }


    void Rotate()
    {

        transform.position += playerObject.transform.position - lastTargetPosition;
        lastTargetPosition = playerObject.transform.position;
        //マウスカーソルで左右視点移動
        float mx = Input.GetAxis("Mouse X");//カーソルの横の移動量を取得
        if (Mathf.Abs(mx) > 0.001f) // X方向に一定量移動していれば横回転
        {
            transform.RotateAround(transform.position, Vector3.up, mx); // 回転軸はplayerオブジェクトのワールド座標Y軸

        }


        //if (Input.GetMouseButton(1))
        //{

        //    Vector3 nowMouseValue = Input.mousePosition - lastMousePosition;

        //    var newAngle = Vector3.zero;
        //    newAngle.x = rotationSpeed.x * nowMouseValue.x;
        //    newAngle.y = rotationSpeed.y * nowMouseValue.y;

        //    transform.RotateAround(playerObject.transform.position, Vector3.up, newAngle.x);
        //    transform.RotateAround(playerObject.transform.position, transform.right, -newAngle.y);
        //}

        //lastMousePosition = Input.mousePosition;
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
