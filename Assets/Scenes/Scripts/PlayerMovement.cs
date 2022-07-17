using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, Interactable
{
    public float moveSpeed;
    public LayerMask foreGroundLayer;
    public LayerMask interactableLayer;

    private Vector2 targetPosition;
    private bool isMoving;
    private bool isInteracting;
    private Vector2 input;
    private Vector2 lastMovementInput;
    

    // Update is called once per frame
    void Update()
    {
        DoMovement();
        DoInteraction();
    }

    private void DoMovement()
    {
        if (!isMoving && !isInteracting)
        {

            var (x, y) = InputManager.Instance.GetBothAxis();
            input.x = x;
            input.y = y;

            if (input.x != 0) input.y = 0;
            if (input != Vector2.zero)
            {
                var destinyPosition = transform.position;
                destinyPosition.x += input.x;
                destinyPosition.y += input.y;

                lastMovementInput = input;

                if (IsWalkable(destinyPosition))
                {
                    StartCoroutine(Move(destinyPosition));
                    targetPosition = destinyPosition;
                }
            }
        }
    }

    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        return Physics2D.OverlapCircle(targetPosition, 0.3f, foreGroundLayer | interactableLayer) == null;
    }

    private Interactable GetInteractable(Vector3 targetPosition)
    {
        var colliders = Physics2D.OverlapCircleAll(targetPosition, 0.3f, foreGroundLayer | interactableLayer);
        foreach (var collider in colliders)
        {
            var component = collider.GetComponent<Interactable>();

            if (component != null) return component;
        }
        return null;
    }

    private void DoInteraction()
    {
        if (InputManager.Instance.IsPressingConfirmation() && !isMoving && !isInteracting)
        {
            var frontTarget = targetPosition;
            frontTarget.x += lastMovementInput.x;
            frontTarget.y += lastMovementInput.y;

            var interactable = GetInteractable(frontTarget);

            if (interactable != null)
            {
                isInteracting = true;
                interactable.Interact(this);
            }
        }
    }

    public void IsDoneInteracting()
    {
        isInteracting = false;
    }

    public void Interact(Interactable source)
    {
        //Ops :)
    }
}