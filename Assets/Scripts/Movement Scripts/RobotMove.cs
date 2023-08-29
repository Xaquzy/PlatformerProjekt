using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RobotMove : MonoBehaviour
{
    [Tooltip("Movement speed of the robot")]
    public float moveSpeed = 1f;
    [Tooltip("How fast do we fall?")]
    public float gravity = 9.8f;
    [Tooltip("Which things count as buttons?")]
    public LayerMask buttonLayer;
    [Tooltip("Which things count as platforms?")]
    public LayerMask platformLayer;

    [Tooltip("What happens when the robot dies?")]
    public UnityEvent OnDeath;

    // Movement and animation
    private Animator animator;
    private CharacterController controller;
    private bool active = true;
    private float baseWalkSpeed = 2f; // relates to animation speed
    private float animSpeed;
    
    // Moving platforms
    private Transform platform = null;
    private Vector3 platformOffset;

    // Button detection
    private bool onButton = false;
    private Button lastSeenButton = null;

    // audio stuff
    private AudioSource footstepAudio;
    private bool moving = false;
    private float basePitch = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        footstepAudio = GetComponent<AudioSource>();

        // Animation speed should sync with walk speed
        animSpeed = moveSpeed / baseWalkSpeed;

        // Audio speed should sync with walk speed
        footstepAudio.pitch = basePitch / (baseWalkSpeed / moveSpeed);
    }

    // Enable player movement (animation and sound will handle itself)
    public void Activate()
    {
        active = true;
    }

    // Disable player movement, animation, and sound
    public void Deactivate()
    {
        active = false;
        animator.SetFloat("walkSpeed", 0f);
        footstepAudio.Stop();
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            // Get user input from keyboard or gamepad
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            // Moving platform support
            Vector3 baseDirection = Vector3.zero;
            if (platform) {
                baseDirection = platform.TransformPoint(platformOffset) - transform.position;
            }

            // Calculate movement direction from input - rotate 45 degrees to account for camera angle
            Vector3 direction = Quaternion.Euler(0f, 45f, 0f) * new Vector3(h, 0f, v).normalized;

            // Give animator information needed to switch between idle and walk animations
            animator.SetFloat("walkSpeed", direction.magnitude);

            // Handle animation speed and sound based on whether we are walking
            if (direction.magnitude > 0.2f)
            {
                if (!moving)
                {
                    animator.speed = animSpeed;
                    footstepAudio.Play();
                    moving = true;
                }
            }
            else
            {
                if (moving)
                {
                    animator.speed = 1f;
                    footstepAudio.Stop();
                    moving = false;
                }
            }

            // Look in the same direction we want to walk
            transform.LookAt(transform.position + direction);

            // Add gravity
            if (!controller.isGrounded)
            {
                direction.y = -gravity;
            }

            // Actually move
            controller.Move(baseDirection + (moveSpeed * Time.deltaTime * direction));

            // Check if we are on a button
            FindButton();

            // Parent under platform
            FindPlatform();
        }
    }

    // Look for a button under our feet.
    private void FindButton()
    {
        // Make a ray that points down
        Ray downRay = new Ray(transform.position + Vector3.up, Vector3.down);
        RaycastHit hit;

        // Check if it hit a button
        if(Physics.Raycast(downRay, out hit, 10f, buttonLayer))
        {
            // We only act the first time we touch a given button
            if(!onButton)
            {
                OnButtonPress(hit.collider.gameObject);
                onButton = true; // Ensures we only do one button press
            }
        } else // If we are not standing on a button, reset onButton
        {
            if(onButton && lastSeenButton != null)
            {
                lastSeenButton.Release();
                lastSeenButton = null;
            }
            onButton = false;
        }
    }

    // Keep track of which platform we are standing on
    // - used for moving along with moving platforms
    private void FindPlatform()
    {
        // Make a ray that points down
        Ray downRay = new Ray(transform.position + Vector3.up, Vector3.down);
        RaycastHit hit;

        // Check if it hit a platform
        if (Physics.Raycast(downRay, out hit, 10f, platformLayer))
        {
            platform = hit.transform;
            platformOffset = platform.InverseTransformPoint(transform.position);
        }
        else
        {
            platform = null;
        }
    }

    // When we are on a button, call its OnHit method
    private void OnButtonPress(GameObject button)
    {
        Button buttonScript = button.GetComponent<Button>();
        lastSeenButton = buttonScript;
        buttonScript.OnHit();
    }

    // Used only for detecting death by falling
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.name == "CatchFalls")
        {
            OnDeath.Invoke();
        }
    }

    public void Kill()
    {
        OnDeath.Invoke();
    }
}
