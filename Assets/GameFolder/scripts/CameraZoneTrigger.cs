using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    public string zoneID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica se o jogador entrou na zona
        {
            FindObjectOfType<CameraZoneManager>().SwitchToZone(zoneID);
        }
    }
}
