using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float JumpForce = 5;
    [SerializeField] private Text scoreText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private float spriteChangeDuration = 1.5f;
    
    [Header("Настройки замедления")]
    [SerializeField] private float slowMotionTimeScale = 0.5f;
    [SerializeField] private float slowMotionDuration = 2f;
    
    private Rigidbody2D rb;
    private int score;
    private Coroutine spriteCoroutine;
    private Coroutine slowMotionCoroutine;
    private bool isSlowed = false;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
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
        isSlowed = true;
        
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isSlowed = false;
    }
    
    private void OnDestroy()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}