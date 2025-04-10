using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic; // Garante que usamos Dictionary corretamente

public class CameraZoneManager : MonoBehaviour
{
    public CinemachineConfiner2D confiner;
    private Dictionary<string, CameraZone> cameraZoneDict = new Dictionary<string, CameraZone>();
    private string currentZoneID = "";

    [System.Serializable]
    public class CameraZone
    {
        public string zoneID;
        public UnityEngine.Collider2D boundingShape; // Especifica o namespace correto
    }

    [SerializeField] private CameraZone[] cameraZones;

    private void Awake()
    {
        // Inicializa o dicionário para acesso rápido às zonas
        foreach (var zone in cameraZones)
        {
            if (!cameraZoneDict.ContainsKey(zone.zoneID))
            {
                cameraZoneDict.Add(zone.zoneID, zone);
            }
            else
            {
                Debug.LogWarning($"Zona com ID repetido detectada: {zone.zoneID}");
            }
        }
    }

    public void SwitchToZone(string zoneID)
    {
        // Se já estamos na zona desejada, não faz nada
        if (zoneID == currentZoneID)
        {
            Debug.Log($"Câmera já está na zona: {zoneID}");
            return;
        }

        if (cameraZoneDict.TryGetValue(zoneID, out CameraZone zone))
        {
            confiner.BoundingShape2D = zone.boundingShape;
            confiner.InvalidateCache();
            currentZoneID = zoneID;
            Debug.Log($"Mudando para zona: {zoneID}");
        }
        else
        {
            Debug.LogWarning($"Zona não encontrada: {zoneID}");
        }
    }
}
