using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; 
using UnityEngine.UI; 

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private TextMeshProUGUI coinText; 
    [SerializeField] private AudioSource coinSound;
    [SerializeField] private ParticleSystem coinEffect; 
    [SerializeField] private GameObject gameOverCanvas; 
    [SerializeField] private TextMeshProUGUI gameOverScoreText; 
    [SerializeField] private Button restartButton; 
    private Animator animator;
    private bool isGrounded;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private int coinCount = 0;
    private AudioManager audioManager;
    //private bool isGameOver = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    

    void Update()
    {
        if (gameManager.IsGameOver() || gameManager.IsGameWin()) return;
        {
            HandleMovement();
            HandleJump();
            UpdateAnimation();
            CheckFallDeath();
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            audioManager.PlayJumpSound();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.velocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coinCount++;
            PlayerPrefs.SetInt("Coins", coinCount);
            PlayerPrefs.Save();
            
        }
        
    }

    private void CheckFallDeath()
    {
        // Kiểm tra nếu nhân vật rơi quá thấp (ví dụ: y < -10)
        if (transform.position.y < -10f)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        //isGameOver = true;
        rb.velocity = Vector2.zero; // Dừng chuyển động
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true); // Hiển thị canvas Game Over
            if (gameOverScoreText != null)
                gameOverScoreText.text = "Coins: " + coinCount; // Cập nhật điểm số
        }
        Time.timeScale = 0f; // Tạm dừng game
    }

    /*private void RestartGame()
    {
        Time.timeScale = 1f; // Khôi phục thời gian
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Tải lại scene hiện tại
        // Tùy chọn: Reset số xu nếu muốn
        // PlayerPrefs.SetInt("Coins", 0);
    }*/
}