using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField] private PlatformElevator targetElevator;
    [SerializeField] private bool oneTimeUse = false;
    [SerializeField] private string playerTag = "Player";

    private bool hasBeenUsed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar se � o jogador e se o elevador foi definido
        if (collision.CompareTag(playerTag) && targetElevator != null)
        {
            // Verificar se j� foi usado (se for oneTimeUse)
            if (oneTimeUse && hasBeenUsed)
                return;

            // Ativar o elevador
            targetElevator.StartElevator();
            hasBeenUsed = true;
        }
    }

    // M�todo para resetar o trigger (caso seja necess�rio)
    public void ResetTrigger()
    {
        hasBeenUsed = false;
    }
}