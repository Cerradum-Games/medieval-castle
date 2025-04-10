using UnityEngine;
using UnityEngine.UI;

public class LifeHUD : MonoBehaviour
{
    public static LifeHUD hpHUD;
    public Text vidaText;
    public Character charPlayer;

    void Start()
    {
        hpHUD = this;
        charPlayer = GetComponent<Character>(); // Encontra o personagem do jogador
    }

    void Update()
    {
        if (charPlayer != null && vidaText != null)
        {
            vidaText.text = "Vida: " + charPlayer.life.ToString();
        }
    }
}
