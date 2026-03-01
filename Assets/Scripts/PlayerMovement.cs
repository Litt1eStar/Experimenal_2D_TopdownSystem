using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private AnimationClip[] idleClips = new AnimationClip[6];
    [SerializeField] private AnimationClip[] walkClips = new AnimationClip[6];

    private Vector2 moveInput = Vector2.zero;
    private Vector2 facingDirection = Vector2.down; //Default facing down   
    private float moveSpeed = 5f;

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
        rb.linearVelocity = moveInput * moveSpeed;
        SetAnimatorParameter();
    }
    private void GetMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(x, y).normalized;
    
        if(moveInput != Vector2.zero)
        {
            facingDirection = moveInput;
        }
    }

    private void SetAnimatorParameter()
    {
        isWalk = moveInput.magnitude > 0;
        direction = isWalk ? GetDirectionIndex(moveInput) : GetDirectionIndex(facingDirection);

        anim.SetBool("isWalk", isWalk);
        anim.SetFloat("Direction", direction);
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
