using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private CharacterController controller;

    public Transform cam;

    [Header("=== Keybinds ===")]
    public KeyCode AtkKey = KeyCode.Q;

    [Header("=== Player Health ===")]
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth;
    [Header("=== Player Stamina ===")]
    [SerializeField]
    private StaminaBar stBar;
    [SerializeField]
    private int maxStamina = 100;
    private int currentStamina;
    [SerializeField]
    private int StaminaRefill;

    [Header("=== Dash ===")]
    [SerializeField]
    private float dashCooldown;
    [SerializeField]
    private int DashCost = 25;


    [Header("=== Player Atttack ===")]
    [SerializeField]
    private int AtkDamage = 25;
    [SerializeField]
    private float AtkSpeed = 1.5f;


    [Header("=== Player Movement Settings ===")]
    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    [SerializeField]
    float turnSmoothVelocity;
    [SerializeField]
    private const float gravity = -9.81f;
    [SerializeField]
    private float jump = 1f;
    Vector3 JumpVector;
    [SerializeField]
    private float jumpCooldown;
    [SerializeField]
    private float dashSpeed;

    

    [Header("Ground Check")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    bool isGrounded;
    private bool canDash;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentStamina = maxStamina;
        stBar.SetMaxStamina(maxStamina);
    }

    
    void Update()
    {

        PlayerMovement();
        Attack();
        #region HealthBar
        //Testing Healthbar
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            TakeDamage(25);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            HealDamage(30);
        }
        //Stamina Bar
        if(currentStamina < 0) 
        { 
            currentStamina = 0;
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

        canDash = true;

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

        if(Input.GetKeyDown(KeyCode.Mouse1) && canDash && currentStamina >= DashCost)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

             currentStamina -= DashCost;

            Debug.Log("Current Stamina: " + currentStamina);
            controller.Move(moveDir * dashSpeed * Time.deltaTime);
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            stBar.SetStamina(currentStamina);
            Invoke(nameof(ResetDash), dashCooldown);
            canDash = false;

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

    private void ResetDash()
    {
        canDash = false;
        RefillStaminaBar(StaminaRefill);
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
        Debug.Log("Health: " + currentHealth);
    }

    void HealDamage(int heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
        Debug.Log("Health: " +  currentHealth);
        
    }

   private void RefillStaminaBar(int refill)
    {
        if(isGrounded)
        {
            currentStamina += refill;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            stBar.SetStamina(currentStamina);
        }
    }

    //Testing Ends
    #endregion


}
