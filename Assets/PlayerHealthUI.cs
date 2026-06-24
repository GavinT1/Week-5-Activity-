using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : NetworkBehaviour
{
    [SerializeField] private GameObject healthCanvas; 
    [SerializeField] private Slider healthSlider;

    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(100);

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            healthCanvas.SetActive(false);
            return;
        }

        currentHealth.OnValueChanged += UpdateHealthUI;
        healthSlider.maxValue = 100;
        healthSlider.value = currentHealth.Value;
    }

    private void UpdateHealthUI(int previousValue, int newValue)
    {
        healthSlider.value = newValue;
    }
}