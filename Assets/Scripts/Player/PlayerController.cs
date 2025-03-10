using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseSpeed;
    public float curSpeed;
    public float moveSpeed;
    public float dashSpeed;
    public float jumpPower;
    public LayerMask groundLayerMask;
    public float useDashStamina;
    public float useJumpStamina;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    public float camCurXRot;
    public float lookSensitivity;

    public bool _canLook = true;

    public bool _isDash = false;
    public bool _isJump = false;
    public bool _isExhaustion = false;
    public bool _isAttack = false;

    public Action inventory;

    public Rigidbody rb;
    public Animator animator;
    private Camera cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        baseSpeed = moveSpeed;
        curSpeed = baseSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }

    public void Move(Vector2 curMovementInput)
    {
        if (_isAttack)
            return;
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;

        rb.velocity = dir;

        if (rb.velocity.magnitude > 0.1f)
            animator.SetBool("OnMove",true);
        else
            animator.SetBool("OnMove",false);
    }

    public void Dash()
    {
        if (_isExhaustion || _isDash)
            return;
        _isDash = true;
        if (_isDash)
        {
            moveSpeed = curSpeed * dashSpeed;
            animator.SetBool("OnDash", true);
        }
    }
    public void DashEnd()
    {
        if (!_isDash)
            return;
        moveSpeed = curSpeed;
        _isDash = false;
        animator.SetBool("OnDash", false);
    }

    public void Look(Vector2 mouseDelta)
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void Jump()
    {
        if (IsGrounded() && !_isExhaustion)
        {
            _isJump = true;
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            CharacterManager.Instance.Player.condition.stamina.Substract(useJumpStamina);
            animator.SetTrigger("IsJump");
        }
    }

    public bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward*0.2f) +transform.up *0.01f,Vector3.down),
            new Ray(transform.position + (-transform.forward*0.2f) +transform.up *0.01f,Vector3.down),
            new Ray(transform.position + (transform.right*0.2f) +transform.up *0.01f,Vector3.down),
            new Ray(transform.position + (-transform.right*0.2f) +transform.up *0.01f,Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                animator.SetBool("OnGround", true);
                return true;
            }
        }
        animator.SetBool("OnGround",false);
        return false;
    }

    public void Exhaustion()
    {
        if (CharacterManager.Instance.Player.condition.stamina.curValue <= 1 && !_isExhaustion)
        {
            _isExhaustion = true;
            moveSpeed = curSpeed / 2;
            if(_isDash)
            {
                DashEnd();
            }
        }
        if (CharacterManager.Instance.Player.condition.stamina.curValue >= 50 && _isExhaustion)
        {
            _isExhaustion = false;
            moveSpeed = curSpeed;
        }
    }

    public void OnAttack()
    {
        if (CharacterManager.Instance.Player.equipment.curEquip != null && _canLook)
        {
            if (!_isAttack)
            {
                _isAttack = true;
                animator.SetTrigger("IsAttack");
                Invoke("OnCanAttak", CharacterManager.Instance.Player.equipment.curEquip.attackRate);
            }
        }
    }
    void OnCanAttak()
    {
        _isAttack = false;
    }


    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        _canLook = !toggle;
    }
}
