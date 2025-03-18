using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public StatManager statManager;
    private PlayerCondition playerCondition;

    [Header("Moverment")]
    public float jumpPower;                 // �����Ŀ�
    private Vector2 curMovementInput;       // �̵� �Է°�
    public LayerMask groundLayerMask;       // ���� �ִ��� üũ�Ҷ� ���?

    [Header("Look")]
    public Transform cameraContainer;       // ī�޶� ����ٴ�?������Ʈ
    public float minXLook;                  // ī�޶� �Ʒ��� �����ִ� �Ѱ�
    public float maxXLook;                  // ī�޶� ���� �����ִ� �Ѱ�
    private float camCurXRot;               // ī�޶� X�� ȸ����
    public float lookSensitivity;           // ���콺 ����
    private Vector2 mouseDelta;             // ���콺 �̵���
    public bool canLook = true;             // ī�޶� ������ �� �ִ��� ����

    private float moveSpeed;
    private Rigidbody _rigidbody;
    private Animator animator;


    Action interactionAction;               // ��ȣ�ۿ� �̺�Ʈ
    private Equip_Item equipItem;           // Equip������ ��������

    private float lastAttackTime;           // ������ ���� �ð�
    private float attackCooldown = 2f;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        statManager = GetComponent<StatManager>();
        playerCondition = GetComponent<PlayerCondition>();
        animator = GetComponent<Animator>();
        equipItem = GetComponentInChildren<Equip_Item>();
        moveSpeed = statManager.speed;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        bool isGrounded = IsGrounded();
        animator.SetBool("IsJump", !isGrounded);
    }

    // �̵�
    void FixedUpdate()
    {
        Move();
    }


    // ī�޶� ȸ��ó��
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

    public void CameraLook()
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        PlayerEquip playerEquip = GetComponent<PlayerEquip>();

        if (context.phase == InputActionPhase.Started && playerEquip.equippedItem != null && playerEquip.equippedItem.gameObject.activeInHierarchy)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                animator.SetTrigger("IsEquip");
                playerEquip.attackAction?.Invoke();
                playerEquip.equippedItem.StartEquipInteraction();

            }
        }
    }

    public void SlowSpeed()
    {
        moveSpeed /= 2f;
    }
}
