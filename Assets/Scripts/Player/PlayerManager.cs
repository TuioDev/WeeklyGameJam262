using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IInteractable
{
    public float moveSpeed;
    public LayerMask foreGroundLayer;
    public LayerMask interactableLayer;
    public float radiusForOverlap = 0.3f;

    public GameObject player;

    private bool isMoving;
    private bool isInteracting;
    private bool isPlayerLocked;
    private bool isSpriteFlipped;
    private Vector2 input;
    private Vector2 lastMovementInput;
    private Vector2 targetPosition;

    private static PlayerManager _Instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<PlayerManager>();
            }

            return _Instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlayerLocked && !isMoving && !isInteracting)
        {
            DoMovement();
            DoInteraction();
        }
    }

    private void DoMovement()
    {
        var (x, y) = InputManager.Instance.GetBothAxis();
        input.x = x;
        input.y = y;

        if (input.x != 0) input.y = 0;
        if (input != Vector2.zero)
        {
            FlipSprite();

            var destinyPosition = player.transform.position;
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

    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        while ((targetPosition - player.transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        player.transform.position = targetPosition;
        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        return Physics2D.OverlapCircle(targetPosition, radiusForOverlap, foreGroundLayer | interactableLayer) == null;
    }

    void FlipSprite()
    {
        if(input.x < 0 && !isSpriteFlipped)
        {
            isSpriteFlipped = true;
            player.GetComponent<SpriteRenderer>().flipX = isSpriteFlipped;
        }
        else if(input.x > 0 && isSpriteFlipped)
        {
            isSpriteFlipped = false;
            player.GetComponent<SpriteRenderer>().flipX = isSpriteFlipped;
        }
    }

    private IInteractable GetInteractable(Vector3 targetPosition)
    {
        var colliders = Physics2D.OverlapCircleAll(targetPosition, radiusForOverlap, foreGroundLayer | interactableLayer);
        foreach (var collider in colliders)
        {
            var component = collider.GetComponent<IInteractable>();

            if (component != null) return component;
        }
        return null;
    }

    private void DoInteraction()
    {
        if (InputManager.Instance.IsPressingConfirmation())
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

    public void Interact(IInteractable source)
    {
        //If other things interact with the player
    }

    public void PlayerLock(bool isPlayerLocked)
    {
        this.isPlayerLocked = isPlayerLocked;
    }
}