using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 10f;

    // 1. Drag your Player's HandPoint child object into this slot in the Inspector
    public Transform handPoint;

    private Rigidbody2D rb;
    private Animator anim;
    private float inputHorizontal;
    private bool jumpPressed;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(inputHorizontal));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputHorizontal * moveSpeed, rb.linearVelocity.y);

        if (jumpPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
            jumpPressed = false;
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // 2. Picks up the single flashlight and attaches it to the player's hand
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flashlight"))
        {
            // Turn off the trigger collider so it doesn't keep triggering while held
            other.enabled = false;

            // Make the flashlight a child of the player's HandPoint so it moves with him
            other.transform.SetParent(handPoint);

            // Snap its local position and rotation directly into the hand
            other.transform.localPosition = Vector3.zero;
            other.transform.localRotation = Quaternion.identity;
        }
    }
}
