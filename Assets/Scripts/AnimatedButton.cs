using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Анимированная кнопка с увеличением при наведении
/// Совместима с Unity 2022.3.62a3
/// </summary>
[RequireComponent(typeof(Button))]
public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Настройки анимации")]
    [SerializeField] private float hoverScale = 1.1f;          // Размер при наведении (1.1 = увеличение на 10%)
    [SerializeField] private float pressScale = 0.95f;         // Размер при нажатии (0.95 = уменьшение на 5%)
    [SerializeField] private float animationDuration = 0.15f;  // Длительность анимации (сек)

    // ДОБАВЛЕНО: ссылка на AudioSource, выбирается в инспекторе
    [Header("Настройки звука")]
    [SerializeField] private AudioSource audioSource;          // Источник звука (перетащить объект с AudioSource)
    [SerializeField] private AudioClip clickSound;             // Звук клика (необязательно, если задан на AudioSource)

    // Компоненты
    private Button button;
    private RectTransform rectTransform;
    
    // Исходные значения
    private Vector3 originalScale;
    
    // Состояния
    private bool isHovered = false;
    private bool isPressed = false;
    private Coroutine currentAnimationCoroutine;

    void Awake()
    {
        // Получаем компоненты
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        
        // Сохраняем исходный размер
        originalScale = rectTransform.localScale;
    }

    void OnEnable()
    {
        // Сбрасываем состояние при включении
        ResetButtonState();
    }

    void OnDisable()
    {
        // Останавливаем анимацию при отключении
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
        }
    }

    #region Обработчики событий указателя (мышь)

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;
        
        isHovered = true;
        StopCurrentAnimation();
        currentAnimationCoroutine = StartCoroutine(AnimateScale(originalScale * hoverScale, animationDuration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        
        if (!isPressed)
        {
            StopCurrentAnimation();
            currentAnimationCoroutine = StartCoroutine(AnimateScale(originalScale, animationDuration));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) return;
        
        isPressed = true;

        // ДОБАВЛЕНО: проигрываем звук при нажатии
        PlayClickSound();

        StopCurrentAnimation();
        currentAnimationCoroutine = StartCoroutine(AnimateScale(originalScale * pressScale, animationDuration * 0.5f));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        StopCurrentAnimation();
        
        // Возвращаемся к состоянию наведения или исходному
        if (isHovered)
        {
            currentAnimationCoroutine = StartCoroutine(AnimateScale(originalScale * hoverScale, animationDuration));
        }
        else
        {
            currentAnimationCoroutine = StartCoroutine(AnimateScale(originalScale, animationDuration));
        }
    }

    #endregion

    #region Анимационная корутина

    private IEnumerator AnimateScale(Vector3 targetScale, float duration)
    {
        Vector3 startScale = rectTransform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            // SmoothStep для плавности
            t = t * t * (3f - 2f * t);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        rectTransform.localScale = targetScale;
        currentAnimationCoroutine = null;
    }

    #endregion

    #region Вспомогательные методы

    // ДОБАВЛЕНО: метод воспроизведения звука клика
    private void PlayClickSound()
    {
        if (audioSource == null) return;

        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            audioSource.Play();
        }
    }

    private void StopCurrentAnimation()
    {
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
        }
    }

    private void ResetButtonState()
    {
        isHovered = false;
        isPressed = false;
        rectTransform.localScale = originalScale;
    }

    /// <summary>
    /// Программно нажать кнопку с анимацией
    /// </summary>
    public void PressButton()
    {
        if (!button.interactable) return;
        StartCoroutine(SimulatePress());
    }

    private IEnumerator SimulatePress()
    {
        OnPointerDown(null);
        yield return new WaitForSecondsRealtime(animationDuration * 0.5f);
        OnPointerUp(null);
        // Вызываем событие клика
        button.onClick.Invoke();
    }

    /// <summary>
    /// Установить интерактивность кнопки
    /// </summary>
    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
        if (!interactable)
        {
            ResetButtonState();
        }
    }

    #endregion

    #region Валидация в редакторе

    #if UNITY_EDITOR
    private void OnValidate()
    {
        // Ограничения значений
        hoverScale = Mathf.Max(1f, hoverScale);
        pressScale = Mathf.Clamp(pressScale, 0.1f, 1f);
        animationDuration = Mathf.Max(0.01f, animationDuration);
    }
    #endif

    #endregion
}