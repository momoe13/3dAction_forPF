using Unity.VisualScripting;
using UnityEngine;

public class Player :CharaBase
{

    [Header("Camera Reference")]
    public Transform cameraTransform;

    private void Update()
    {

        GroundCheck();
        HandleInput();
    }

    private void FixedUpdate()
    {

        MoveCharacter();
        RotateCharacter();
    }

    private void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A,Dキー
        float v = Input.GetAxisRaw("Vertical");   // W,Sキー

        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        if (inputDir.magnitude > 0.1f)
        {
            // カメラの向きを基準にして移動方向を回転
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            moveDirection = (camForward * v + camRight * h).normalized;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

}
