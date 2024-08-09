using Unity.Netcode;
using UnityEngine;

public class MeleeAttack : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit!" + " From Player: " + OwnerClientId);
        }
    }
}
