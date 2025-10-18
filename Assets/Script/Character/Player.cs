using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player :CharaBase
{
    [Header("Camera Reference")]
    public Transform cameraTransform;

    [SerializeField]
    private Animator animator;

    BoxCollider AttackErea;
    private void Update()
    {

        GroundCheck();
        HandleInput();
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
            animator.SetBool("Move", true);
        }
        else
        {
            moveDirection = Vector3.zero;

            animator.SetBool("Move", false);
        }

        MoveCharacter();
        
        if(Input.GetButtonDown("Jump"))
        {
                Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= 1.5f;
            animator.SetBool("Dash", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= 1.5f;
            animator.SetBool("Dash",false);
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
        }
    }

    //コルーチン
    IEnumerable Attack()
    {
        //攻撃
        animator.SetBool("Attack", true);
        AttackErea.enabled = true;
        yield return null;

        //攻撃終了
        animator.SetBool("Attack", false);
        AttackErea.enabled = false;
        yield return null;
    }

}
