using Unity.Netcode;
using UnityEngine;
using TMPro;

public class DamageTextManager : NetworkBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;

    public void TakeDamage(int damageAmount)
    {
        if (!IsServer) return;
        
        ShowDamageTextClientRpc(damageAmount, transform.position);
    }

    [ClientRpc]
    private void ShowDamageTextClientRpc(int damage, Vector3 spawnPosition)
    {
        GameObject textInstance = Instantiate(damageTextPrefab, spawnPosition + Vector3.up * 2f, Quaternion.identity);
        textInstance.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
        
        Destroy(textInstance, 1f); 
    }
}