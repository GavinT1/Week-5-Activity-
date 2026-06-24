using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!IsOwner) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            RequestJumpServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C Key pressed! Attempting auto-aim shoot...");
            RequestShootServerRpc();
        }
    }

    [ServerRpc]
    void RequestJumpServerRpc()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; 
    }

    [ServerRpc]
    void RequestShootServerRpc()
    {
        PlayerMovement[] allPlayers = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        PlayerMovement targetEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (PlayerMovement player in allPlayers)
        {
            if (player == this) continue;

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetEnemy = player;
            }
        }

        if (targetEnemy != null)
        {
            Debug.Log("Server auto-targeted enemy: " + targetEnemy.gameObject.name);

            PlayerHealthUI enemyHealth = targetEnemy.GetComponentInChildren<PlayerHealthUI>();
            DamageTextManager enemyDamageText = targetEnemy.GetComponent<DamageTextManager>();

            if (enemyHealth != null)
            {
                enemyHealth.currentHealth.Value -= 20;
                Debug.Log("Enemy health dropped to: " + enemyHealth.currentHealth.Value);

                if (enemyDamageText != null)
                {
                    enemyDamageText.TakeDamage(20);
                }
            }
        }
        else
        {
            Debug.Log("Server could not find any other players to shoot.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}