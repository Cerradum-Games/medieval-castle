using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
//using System.Diagnostics;

//using System.Diagnostics;
using System.Security.Cryptography;

//using System.Diagnostics; 


//using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 vel;
    Vector2 vertical;

    private bool isCrouching = false;
    private float originalColliderHeight;
    private Vector2 originalColliderOffset;

    public Transform floorCollider;
    private CapsuleCollider2D capsuleCollider;
    public LayerMask floorLayer;
    public Transform skin;
    public int comboNumber;
    public float comboTime;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float dashForce = 10f;

    [SerializeField] float speed;
    [SerializeField] float jumpPower;

    [Header("Configurações de Vida")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    [Header("Configurações de Respawn")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 1.0f;
    [SerializeField] private GameObject deathEffectPrefab;

    private bool isRespawning = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    // Events para UI
    public delegate void HealthChangeHandler(int health);
    public static event HealthChangeHandler OnHealthChanged;

    void Start()
    {
        // Obtém a referência ao CapsuleCollider2D do Player
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        comboTime = 0;

        // Inicializar componentes e vida
        spriteRenderer = skin.GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        currentHealth = maxHealth;

        // Verificar se o ponto de respawn foi definido
        if (respawnPoint == null)
        {
            Debug.LogWarning("Ponto de respawn não definido! Usando posição inicial do jogador.");
            respawnPoint = transform;
        }

        // Notificar a UI sobre a vida inicial
        OnHealthChanged?.Invoke(currentHealth);
    }

    void Update()
    {
        // Se estiver em respawn, não executar lógica de movimento
        if (isRespawning)
            return;

        // character actions in game
        move();
        jump();
        crouch();
        dash();
        attack();

        // Verifica a vida do personagem
        if (currentHealth <= 0)
        {
            this.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isRespawning)
        {
            rb.linearVelocity = vel;
        }
    }

    // ------------------------------------------------------------------------------------------------------------------ \\
    private void move()
    {
        vel = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.linearVelocity.y);
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            skin.localScale = new Vector3((Input.GetAxisRaw("Horizontal")) * 5.6f, 5.6f, 5.6f);
            skin.GetComponent<Animator>().SetBool("PlayerRun", true);
        }
        else
        {
            skin.GetComponent<Animator>().SetBool("PlayerRun", false);
        }

    }
    // ------------------------------------------------------------------------------------------------------------------ \\

    private void crouch()
    {
        bool canCrouch = Physics2D.OverlapCircle(floorCollider.position, 0.1f, floorLayer);
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Obter referência ao collider uma vez, no Start()
        if (capsuleCollider == null)
        {
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        if (verticalInput < 0 && floorCollider.GetComponent<FloorCollider>().canCrouch == true)
        {
            skin.GetComponent<Animator>().Play("PlayerCrouch", -1);
            vel = new Vector2(0, rb.linearVelocity.y);

            // Guardar tamanho original (caso ainda não esteja armazenado)
            if (!isCrouching)
            {
                originalColliderHeight = capsuleCollider.size.y;
                originalColliderOffset = capsuleCollider.offset;
                isCrouching = true;
            }

            // Reduzir tamanho do collider pela metade
            float newHeight = originalColliderHeight / 2;

            // IMPORTANTE: Ajustar o offset para manter os pés no mesmo lugar
            // O deslocamento precisa ser para cima em metade da diferença entre a altura original e a nova
            float heightDifference = originalColliderHeight - newHeight;
            Vector2 newOffset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - (heightDifference / 2));

            // Aplicar as mudanças
            capsuleCollider.size = new Vector2(capsuleCollider.size.x, newHeight);
            capsuleCollider.offset = newOffset;

            Debug.Log("Abaixou");
        }
        else if (isCrouching && (verticalInput >= 0 || !canCrouch))
        {
            // Restaurar tamanho original do collider quando não estiver mais agachado
            capsuleCollider.size = new Vector2(capsuleCollider.size.x, originalColliderHeight);
            capsuleCollider.offset = originalColliderOffset;
            isCrouching = false;
            Debug.Log("Levantou");
        }
    }
    // ------------------------------------------------------------------------------------------------------------------ \\

    private void jump()
    {
        bool canJump = Physics2D.OverlapCircle(floorCollider.position, 0.1f, floorLayer);

        if (Input.GetButtonDown("Jump") && floorCollider.GetComponent<FloorCollider>().canJump == true)
        {
            skin.GetComponent<Animator>().Play("PlayerJump", -1);
            rb.linearVelocity = Vector2.zero;
            floorCollider.GetComponent<FloorCollider>().canJump = false;
            rb.AddForce(new Vector2(250f, jumpPower));
        }
    }

    private void dash()
    {
        // Habilita o dash
        dashCooldown  += Time.deltaTime;
        if (Input.GetButtonDown("Fire2") && dashCooldown > 1)
        {
            dashCooldown = 0;
            skin.GetComponent<Animator>().Play("Dash", -1);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(skin.localScale.x * dashForce, 0));
        }
    }

    private void attack()
    {
        comboTime = comboTime + Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && comboTime > 0.5f)
        {
            comboNumber++;
            if (comboNumber > 2)
            {
                comboNumber = 1;
            }
            comboTime = 0;
            skin.GetComponent<Animator>().Play("Attack" + comboNumber, 0);
        }
        if (comboTime >= 1)
        {
            comboNumber = 0;
        }
    }

    // ------------------------------------------------------------------------------------------------------------------ \\

    // Sistema de vida e respawn
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar se colidiu com um obstáculo ou armadilha
        if (other.CompareTag("Hazard") && !isRespawning)
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        // Diminuir a vida
        currentHealth--;

        // Notificar a UI
        OnHealthChanged?.Invoke(currentHealth);

        // Verificar se ainda tem vida
        if (currentHealth <= 0)
        {
            // Game over ou reiniciar nível
            Debug.Log("Game Over - Sem vidas restantes!");
            // A verificação no Update já vai desativar o controlador
        }
        else
        {
            // Iniciar o processo de respawn
            StartCoroutine(RespawnPlayer());
        }

        // Criar efeito visual de morte
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator RespawnPlayer()
    {
        isRespawning = true;

        // Desativar componentes durante o respawn
        spriteRenderer.enabled = false;
        if (playerCollider != null)
            playerCollider.enabled = false;

        // Congelar o rigidbody
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;

        // Aguardar o tempo de respawn
        yield return new WaitForSeconds(respawnDelay);

        // Teleportar para o ponto de respawn
        transform.position = respawnPoint.position;

        // Reativar os componentes
        spriteRenderer.enabled = true;
        if (playerCollider != null)
            playerCollider.enabled = true;

        // Restaurar física
        rb.isKinematic = false;

        isRespawning = false;
    }

    // Método público para definir um novo ponto de respawn (para checkpoints)
    public void SetRespawnPoint(Transform newRespawnPoint)
    {
        if (newRespawnPoint != null)
        {
            respawnPoint = newRespawnPoint;
            Debug.Log("Novo ponto de respawn definido!");
        }
    }

    // Método para debugging ou para outros scripts acessarem
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}