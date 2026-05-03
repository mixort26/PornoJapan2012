using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float JumpForce = 5;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private float spriteChangeDuration = 1.5f;
    [Header("Границы экрана")]
    [SerializeField] private float minY = -5f; // Нижний край (земля)
    [SerializeField] private float maxY = 5f;
    [Header("Настройки замедления")]
    [SerializeField] private float slowMotionTimeScale = 0.5f;
    [SerializeField] private float slowMotionDuration = 2f;
    
    private bool _isTransitioning = false;
    private Rigidbody2D rb;
    public int score;
    private Coroutine spriteCoroutine;
    private Coroutine slowMotionCoroutine;
    // private bool isSlowed = false;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        GameData.Minutes = 1200;
        // GameData.Minutes = 470;
        score = 10;
    }
    
    private void Update()
    {
        // Прыжок работает всегда, независимо от замедления
        if (Keyboard.current.spaceKey.wasPressedThisFrame || 
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            rb.linearVelocity = new Vector2(0, JumpForce);
            ChangeSpriteTemporarily();
        }

        // ОГРАНИЧЕНИЕ ПОЗИЦИИ:
        // Clamp берет текущее значение и не дает ему выйти за пределы min и max
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
        
        // Если игрок коснулся потолка или пола, обнуляем вертикальную скорость, 
        // чтобы он не "накапливал" силу прыжка, прижимаясь к краю
        if (transform.position.y >= maxY && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
        else if (transform.position.y <= minY && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
        
        scoreText.text = score.ToString();
        Debug.Log(GameData.Minutes);
        if (GameData.Minutes >= 1440) GameData.Minutes = 0;
        timeText.text = (GameData.Minutes >= 600 ? "" + GameData.Minutes / 60 : "0" + GameData.Minutes / 60) + ':' +
                        (GameData.Minutes % 60 >= 10 ? GameData.Minutes % 60 : "0" + GameData.Minutes % 60);
        
        if (GameData.Minutes == 475 && !_isTransitioning) {
            _isTransitioning = true;
            GameData.Score = score;
            SceneManager.LoadScene("ToDay");
        }
    }
    
    private void ChangeSpriteTemporarily()
    {
        if (spriteCoroutine != null)
            StopCoroutine(spriteCoroutine);
        
        spriteCoroutine = StartCoroutine(SpriteChangeRoutine());
    }
    
    private IEnumerator SpriteChangeRoutine()
    {
        spriteRenderer.sprite = jumpSprite;
        yield return new WaitForSecondsRealtime(spriteChangeDuration);
        spriteRenderer.sprite = normalSprite;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Проход через трубу! Замедление активировано.");
            StartGlobalSlowMotion();
        }
        // НОВОЕ: сбор ингредиентов
        else if (collision.gameObject.CompareTag("HUI"))
        {
            score++;
            Destroy(collision.gameObject); // Уничтожаем ингредиент
            Debug.Log($"Ингредиент собран! Счет: {score}");
        }
    }
    
    private void StartGlobalSlowMotion()
    {
        if (slowMotionCoroutine != null)
            StopCoroutine(slowMotionCoroutine);
        
        slowMotionCoroutine = StartCoroutine(GlobalSlowMotionRoutine());
    }
    
    private IEnumerator GlobalSlowMotionRoutine()
    {
        // isSlowed = true;
        
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        // isSlowed = false;
    }
    
    private void OnDestroy()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}