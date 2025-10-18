using UnityEngine;

public class CharaBase : MonoBehaviour
{
    protected bool moveFlg;
    protected float moveAngle;
    [SerializeField]
    float speed = 2.0f;

    [Header("移動処理")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    protected Rigidbody rb;
    protected Vector3 moveDirection;// 現在入力またはAIが指定した移動方向
    protected bool isGrounded;         // 地面に接地しているか

    [Header("設地判定")]
    public Transform groundCheck;   // 足元チェック用
    public float groundRadius = 0.3f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 回転はスクリプトで制御する
    }



    protected void GroundCheck()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius);
        }
        Debug.Log(isGrounded);
    }
    protected void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
    protected void MoveCharacter()
    {
        // moveDirectionは派生クラスで設定される（入力 or AIなど）
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.linearVelocity.y; // 重力はRigidbodyに任せる
        rb.linearVelocity = velocity;
        RotateCharacter();
    }

    private void RotateCharacter()
    {
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }


}
