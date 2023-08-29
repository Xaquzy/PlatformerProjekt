using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    // How fast should the button be pressed down?
    [SerializeField] private float pressDownSpeed = 0.01f;

    [Tooltip("Action to take when button is pressed")]
    public UnityEvent OnPress;

    // Button animation
    private Vector3 nextPosition;
    private Vector3 targetPosition;

    private bool ready = true;

    // Start is called before the first frame update
    private void Start()
    {
        // At first, the target position is the position the button is already at
        targetPosition = transform.localPosition;

        // When pressed, the button should move 0.04m downwards
        nextPosition = targetPosition;
        nextPosition.y -= 0.04f;
    }

    // Update is called once per frame
    private void Update()
    {
        // We always move towards the target position - if we're already there,
        // this will result in no movement
        transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                                                      targetPosition,
                                                      pressDownSpeed * Time.deltaTime);
    }

    // Called by other scripts when they want to press the button
    public void OnHit()
    {
        // Only allow the button to be pressed if it is not already pressed down
        if(ready)
        {
            // Immediately disallow further button presses
            ready = false;

            // Swap target- and next position
            (nextPosition, targetPosition) = (targetPosition, nextPosition);

            // Run whatever action has been configured in the editor
            OnPress.Invoke();
        }
    }

    // Called by other scripts when they want to release the button
    public void Release()
    {
        (nextPosition, targetPosition) = (targetPosition, nextPosition);
        ready = true;
    }
}
