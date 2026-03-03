using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Setting")]
    [SerializeField] private float maxMovingSpeed = 5f;
    [SerializeField] private float initialMovingSpeed = 1f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private KeyCode dashKeyCode = KeyCode.LeftShift;

    [Header("Animation Clip")]
    [SerializeField] private AnimationClip[] idleClips = new AnimationClip[6];
    [SerializeField] private AnimationClip[] walkClips = new AnimationClip[6];

    [Header("Animaton String Trigger")]
    [SerializeField] private const string isWalkParameter = "isWalk";
    [SerializeField] private const string directionParameter = "Direction";

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 facingDirection = Vector2.down; //Default facing down   

    private float currentWalkSpeed = 0f;
    private float currentDashTime = 0f;
    private float dashCooldownTimer = 0f;
    private bool canDash = true;
    private bool isWalk = false;
    public int direction = 0; // 0 = Down, 1 = RightDown, 2 = RightUp, 3 = Up, 4 = LeftUp, 5 = LeftDown
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        GetMovementInput();
        rb.linearVelocity = moveInput * currentWalkSpeed;
        SetAnimatorParameter();

        if (Input.GetKeyDown(dashKeyCode) && canDash)
        {
            StartCoroutine(Dash(facingDirection));
        }
    }
    private void GetMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(x, y).normalized;

        if (moveInput != Vector2.zero)
        {
            facingDirection = moveInput;
            if (currentWalkSpeed <= maxMovingSpeed)
            {
                currentWalkSpeed = Mathf.Lerp(currentWalkSpeed, maxMovingSpeed, Time.deltaTime * 5);
            }
        }
        else
        {
            currentWalkSpeed = initialMovingSpeed;
        }
    }

    private IEnumerator Dash(Vector2 direction)
    {
        currentDashTime = dashDuration;
        canDash = false;

        while(currentDashTime > 0f)
        {
            currentDashTime -= Time.deltaTime;
            rb.linearVelocity = direction * dashSpeed;
            yield return null;  
        }

        rb.linearVelocity = Vector2.zero;

        dashCooldownTimer = dashCooldown;
        while (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
            yield return null;
        }

        canDash = true;
    }
    private void SetAnimatorParameter()
    {
        isWalk = moveInput.magnitude > 0;
        direction = isWalk ? GetDirectionIndex(moveInput) : GetDirectionIndex(facingDirection);

        anim.SetBool(isWalkParameter, isWalk);
        anim.SetFloat(directionParameter, direction);
    }

    private int GetDirectionIndex(Vector2 direction)
    {
        int directionIndex = 0;

        float dirX = direction.x;
        float dirY = direction.y;

        bool down = dirX == 0 && dirY < 0;
        bool rightAndRightDown = (dirX > 0 && dirY == 0) || (dirX > 0 && dirY < 0);
        bool rightUp = dirX > 0 && dirY > 0;    
        bool up = dirX == 0 && dirY > 0;
        bool leftUp = dirX < 0 && dirY > 0;
        bool leftAndLeftDown = (dirX < 0 && dirY == 0) || (dirX < 0 && dirY < 0);

        if (down)
        {
            directionIndex = 0;
        }
        else if (rightAndRightDown)
        {
            directionIndex = 1;
        }
        else if (rightUp)
        {
            directionIndex = 2;
        }
        else if (up)
        {
            directionIndex = 3;
        }
        else if (leftUp)
        {
            directionIndex = 4;
        }
        else if (leftAndLeftDown)
        {
            directionIndex = 5;
        }
        return directionIndex;
    }
}
