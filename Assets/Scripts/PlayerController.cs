using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource audioSource;

    [Header("Áudios")]
    public AudioClip audioPulo;
    public AudioClip audioCair;
    public AudioClip audioPegar;

    [Header("Movimentação")]
    public float speed = 5.0f;
    public float turnSpeed = 10.0f;

    [Header("Pulo")]
    public float jumpForce = 7.0f;
    public float gravityModifier = 1.5f;
    public bool isOnGround = true;

    [Header("Estado do jogo")]
    public bool gameOver = false;
    public bool hasEnergy = false;

    [Header("Energia")]
    public int energyCount = 0;
    public int energyToWin = 5;
    public TextMeshProUGUI energyText;

    [Header("Efeitos Visuais")]
    public GameObject auraParticle; // arrastar no Inspector

    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();

        Physics.gravity *= gravityModifier;
        playerRb.freezeRotation = true;

        UpdateEnergyUI();
    }

    void Update()
    {
        if (gameOver) return;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Só pode correr se tiver energia
        bool isRunning = hasEnergy && Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? speed * 2f : speed;

        Vector3 inputDirection = (Camera.main.transform.forward * verticalInput + Camera.main.transform.right * horizontalInput);
        inputDirection.y = 0f;
        inputDirection = inputDirection.normalized;

        playerAnim.SetFloat("Speed_f", inputDirection.magnitude * (isRunning ? 1.5f : 1f));

        if (inputDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

            Vector3 move = inputDirection * currentSpeed * Time.deltaTime;
            Vector3 targetPosition = transform.position + move;
            playerRb.MovePosition(targetPosition);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            audioSource.PlayOneShot(audioPulo, 1.0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (gameOver) return;

        if (collision.gameObject.CompareTag("Fram"))
        {
            isOnGround = true;
            playerAnim.SetTrigger("On_ground");
        }
        else if (collision.gameObject.CompareTag("GameOver"))
        {
            gameOver = true;
            Debug.Log("Game Over!");
            playerAnim.SetTrigger("Dead");
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.isKinematic = true;

            audioSource.PlayOneShot(audioCair);

            if (gameManager != null)
                gameManager.ShowGameOver(false);
        }
        else if (collision.gameObject.CompareTag("Win"))
        {
            if (energyCount >= energyToWin)
            {
                Debug.Log("Você venceu!");
                playerRb.velocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
                playerRb.isKinematic = true;

                //playerAnim.Play("WinPose"); // animação de vitória

                if (gameManager != null)
                    gameManager.ShowGameOver(true);
            }
            else
            {
                Debug.Log("Você precisa de mais energia para vencer!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Energy"))
        {
            energyCount++;

            // Ativa aura só na primeira vez
            if (!hasEnergy)
            {
                hasEnergy = true;

                if (auraParticle != null)
                    auraParticle.SetActive(true);

                playerAnim.SetTrigger("Run_trig"); // 🔥 Ativa animação de corrida ao ganhar energia
            }


            audioSource.PlayOneShot(audioPegar);
            Destroy(other.gameObject);

            UpdateEnergyUI();
        }
    }

    void UpdateEnergyUI()
    {
        if (energyText != null)
        {
            energyText.text = $"Energia: {energyCount}/{energyToWin}";
        }
    }
}
