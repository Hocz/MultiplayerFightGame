using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [SerializeField] private Transform meleeAttack;
    [SerializeField] private float attackRadius = 2f;


    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;

    private bool isGrounded = true;

    private void Update()
    {
        if (!IsOwner) return;

        HandlePlayerMovement();
        HandlePlayerJump();
        HandlePlayerAttack();
        HandlePlatformSwitch();
        HandlePlayerTaunt();
    }

    private void HandlePlayerMovement()
    {
        Vector2 movementDirection = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.A)) movementDirection.x = -1f;
        if (Input.GetKey(KeyCode.D)) movementDirection.x = +1f;

        if (movementDirection != Vector2.zero)
        {
            RequestPlayerMoveServerRpc(movementDirection);
        }
    }

    private void HandlePlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            RequestPlayerJumpServerRpc();
        }
    }

    private void HandlePlayerAttack()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        RequestAttackAimServerRpc(mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            RequestAttackServerRpc(0.5f, mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            RequestAttackServerRpc(-0.5f, mousePosition);
        }
    }

    private void HandlePlatformSwitch()
    {
        if (Input.GetKey(KeyCode.S))
        {
            RequestPlatformSwitchServerRpc();
        }
    }

    private void HandlePlayerTaunt()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            RequestPlayerTauntServerRpc();
        }
    }

    [ServerRpc]
    private void RequestPlayerMoveServerRpc(Vector3 movementDirection, ServerRpcParams serverRpcParams = default)
    {
        Vector3 moveVelocity = movementDirection * movementSpeed;
        rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);
    }

    [ServerRpc]
    private void RequestPlayerJumpServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    [ServerRpc]
    private void RequestAttackAimServerRpc(Vector3 mousePosition, ServerRpcParams serverRpcParams = default) 
    {
        Vector3 directionToMouse = mousePosition - transform.position;
        directionToMouse.Normalize();

        Vector3 attackPosition = transform.position + directionToMouse * attackRadius;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        meleeAttack.position = attackPosition;
        meleeAttack.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }


    [ServerRpc]
    private void RequestAttackServerRpc(float range, Vector3 mousePosition, ServerRpcParams serverRpcParams = default)
    {
        attackRadius += range;

        Vector3 directionToMouse = mousePosition - meleeAttack.position;
        directionToMouse.Normalize();

        RaycastHit2D[] hits = Physics2D.RaycastAll(meleeAttack.position, directionToMouse, range);

        Debug.DrawRay(meleeAttack.position, directionToMouse * range, Color.green, 2f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    hit.rigidbody.AddForce(directionToMouse * 10, ForceMode2D.Impulse);
                    hit.rigidbody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    break;
                }
            }
        }
    }

    [ServerRpc]
    private void RequestPlayerTauntServerRpc(ServerRpcParams serverRpcParams = default)
    {
        animator.Play("Taunt");
    }

    [ServerRpc]
    private void RequestPlatformSwitchServerRpc(ServerRpcParams serverRpcParams = default)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.8f);
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.red, 2f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "OneWayPlatform")
                {
                    IgnorePlatformForServerRpc();
                    break;
                }
            }
        }
    }

    [ServerRpc]
    private void IgnorePlatformForServerRpc(ServerRpcParams serverRpcParams = default)
    {
        gameObject.layer = LayerMask.NameToLayer("IgnoreOneWayPlatform");
        StartCoroutine(ResetPlayerLayer());
    }

    private IEnumerator ResetPlayerLayer()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            isGrounded = true;
        }
    }
}
