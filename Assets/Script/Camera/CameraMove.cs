using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [SerializeField] GameObject cameraRotateObj;
    [SerializeField] CinemachineSplineDolly cinemachineSplineDolly;
  
    Vector2 cameraInput;

    float dollySpeed = 5f;
    float rotateSpeed = 120f;

    //ロックオン機能
    [SerializeField] bool isRockon = true;

    [SerializeField] Transform Player;

    void Update()
    {

        cameraRotateObj.transform.position = Player.transform.position;

        if (cinemachineSplineDolly == null) return;
        VerticalMove();
        HorizontalMove();
    }

    private void HorizontalMove()
    {
        float horizontal = cameraInput.x;

        cameraRotateObj.transform.Rotate(0, horizontal * rotateSpeed * Time.deltaTime, 0);
    }

    private void VerticalMove()
    {
        // 縦入力
        float vertical = cameraInput.y;

        // Splineの長さ取得
        float splineLength = cinemachineSplineDolly.Spline.CalculateLength();

        // 入力値から移動量計算
        float deltaDistance = vertical * dollySpeed * Time.deltaTime;

        // 0～1の値へ変換
        float deltaT = deltaDistance / splineLength;

        // カメラ位置更新
        cinemachineSplineDolly.CameraPosition += deltaT;

        // 範囲制限
        cinemachineSplineDolly.CameraPosition =
            Mathf.Clamp01(cinemachineSplineDolly.CameraPosition);
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        cameraInput = context.ReadValue<Vector2>();
    }

    public void OnRockon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRockon = !isRockon;
        }
    }

}
