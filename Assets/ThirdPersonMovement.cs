using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonMovement : MonoBehaviour
{

    public Rigidbody rb;
    public CharacterController controller;

    public Transform cam;

    [Header("=== Keybinds ===")]
    public KeyCode AtkKey = KeyCode.Q;

    [Header("=== Player utilities ===")]
    public HealthBar healthBar;
    public int maxHealth = 100;
    private int currentHealth;
    public int maxDashes = 2;

    [SerializeField]
    private int AtkDamage = 25;
    [SerializeField]
    private float AtkSpeed = 1.5f;


    [Header("=== Player Movement Settings ===")]
    [SerializeField]
    public float speed = 6f;
    [SerializeField]
    public float turnSmoothTime = 0.1f;
    [SerializeField]
    float turnSmoothVelocity;
    [SerializeField]
    public float gravity = -9.81f;
    [SerializeField]
    public float jump = 1f;
    Vector3 JumpVector;

    [Header("Ground Check")]
    [SerializeField]
    public Transform groundCheck;
    [SerializeField]
    public float groundDistance = 0.4f;
    [SerializeField]
    public LayerMask groundMask;
    [SerializeField]
    bool isGrounded;
    [SerializeField]
    public float jumpCooldown;


    enum PlayerState
    {
        IDLE = 0,
        WALK,
        DASH,
        JUMP,
        ATTACK = 10
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    
    void Update()
    {
        
        PlayerMovement();
        Attack();
        #region HealthBar
        //Testing Healthbar
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            HealDamage(20);
        }
        //Testing Ends
        #endregion
    }


    void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        JumpVector.y += gravity * Time.deltaTime;
        controller.Move(JumpVector * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            JumpVector.y = Mathf.Sqrt(jump * -2f * gravity);
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (isGrounded && JumpVector.y < 0)
        {
            JumpVector.y = gravity;
        }
    }
    private void ResetJump()
    {
        isGrounded = false;
    }

    void Attack()
    {
       if(Input.GetKeyDown(AtkKey))
       {
            Debug.Log("AtkKey was Pressed");

       }
    }

    #region Testing
    //Testing Damage to Player
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    void HealDamage(int heal)
    {
        currentHealth += heal;
        healthBar.SetHealth(currentHealth);
    }
    //Testing Ends
    #endregion
}
