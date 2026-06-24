using Unity.Netcode;
using UnityEngine;

public class NetworkButtons : MonoBehaviour
{
    private void OnGUI()
    {
        if (NetworkManager.Singleton == null)
        {
            return;
        }

        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host (Server + Client)"))
            {
                NetworkManager.Singleton.StartHost();
            }
            
            if (GUILayout.Button("Client (Join Game)"))
            {
                NetworkManager.Singleton.StartClient();
            }
        }
        else
        {
            if (GUILayout.Button("Test Damage Text"))
            {
                DamageTextManager damageScript = Object.FindAnyObjectByType<DamageTextManager>();
                if (damageScript != null)
                {
                    damageScript.TakeDamage(25);
                }
            }
        }
        
        GUILayout.EndArea();
    }
}