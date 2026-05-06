using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class CameraMove : MonoBehaviour
{
    [SerializeField] CinemachineSplineDolly cinemachineSplineDolly;
    float dollSpeed = 1;


    Vector3 cameraSpeed;


    //ロックオン機能
    [SerializeField] bool isRockon = true;

    void Update()
    {
        if (cinemachineSplineDolly == null) return;

        // スプラインの長さを取得
        float splineLength = cinemachineSplineDolly.Spline.CalculateLength();

        // 経路上の現在位置（0〜1）を時間経過で進める
        float deltaDistance = dollSpeed * Time.deltaTime;
        float deltaT = deltaDistance / splineLength;

        cinemachineSplineDolly.CameraPosition += deltaT;


    }


    public void OnCamera(InputAction.CallbackContext context)
    {
        cameraSpeed = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);

    }

    public void OnRockon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRockon = !isRockon;
        }
    }

}
