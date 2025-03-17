using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public StatManager statManager;
    private PlayerCondition playerCondition;

    [Header("Moverment")]
    public float jumpPower;                 // 점프파워
    private Vector2 curMovementInput;       // 이동 입력값
    public LayerMask groundLayerMask;       // 땅에 있는지 체크할때 사용

    [Header("Look")]
    public Transform cameraContainer;       // 카메라가 따라다닐 오브젝트
    public float minXLook;                  // 카메라가 아래로 볼수있는 한계
    public float maxXLook;                  // 카메라가 위로 볼수있는 한계
    private float camCurXRot;               // 카메라 X축 회전값
    public float lookSensitivity;           // 마우스 감도
    private Vector2 mouseDelta;             // 마우스 이동값
    public bool canLook = true;             // 카메라 움직일 수 있는지 여부

    public Action inventory;                // 인벤토리 열기
    private float moveSpeed;
    private Rigidbody _rigidbody;
    private Animator animator;

    Action interactionAction;                          // 상호작용 이벤트

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        statManager = GetComponent<StatManager>();
        playerCondition = GetComponent<PlayerCondition>();
        animator = GetComponent<Animator>();
        moveSpeed = statManager.speed;
    }

    // 마우스 커서 화면 중앙에 고정
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        bool isGrounded = IsGrounded();
        animator.SetBool("IsJump", !isGrounded);
    }

    // 이동
    void FixedUpdate()
    {
        Move();
    }


    // 카메라 회전처리
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
        bool isMove = curMovementInput != Vector2.zero;
        animator.SetBool("IsMove", isMove);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        var playerCondition = GetComponent<PlayerCondition>();
        if (context.phase == InputActionPhase.Performed)
        {
            if (playerCondition.curStamina >= 10f)
            {
                moveSpeed *= 2f;
                playerCondition.LoseStamina();
                animator.SetBool("IsRun", true);
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveSpeed /= 2f;
            playerCondition.LoseStamina();
            animator.SetBool("IsRun", false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            animator.SetBool("IsJump", true);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.3f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnInventory(InputAction.CallbackContext Context)
    {
        if (Context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    public void OnInteraction(InputAction.CallbackContext Context)
    {
        if (Context.phase == InputActionPhase.Started)
        {
            IInteractable interactingObject = GameManager.Instance.player.interaction.interactingObject;

            if (interactingObject != null)
            {
                interactionAction = interactingObject.SubscribeMethod;

                interactionAction?.Invoke();
            }
        }
    }
}
