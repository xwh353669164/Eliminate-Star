using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarView : MonoBehaviour, IPointerClickHandler
{
    public int Id { get; private set; }
    public int Row { get; private set; }
    public int Column { get; private set; }
    public RectTransform RectTransform { get; private set; }

    private Image image;
    private float feedbackDuration;
    private float feedbackTime;
    private bool inputEnabled;
    private bool feedbackPlaying;

    private void Awake()
    {
        RectTransform = (RectTransform)transform;
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (!feedbackPlaying)
            return;

        feedbackTime += Time.deltaTime;
        float progress = Mathf.Clamp01(feedbackTime / feedbackDuration);
        float pulse = Mathf.Sin(progress * Mathf.PI * 2f) * 0.1f;
        RectTransform.localScale = Vector3.one * (1f + pulse);

        if (progress < 1f)
            return;

        RectTransform.localScale = Vector3.one;
        feedbackPlaying = false;
    }

    public void Bind(StarData data, float invalidDuration)
    {
        Id = data.Id;
        Row = data.Row;
        Column = data.Column;
        feedbackDuration = invalidDuration;
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
        image.raycastTarget = enabled;
    }

    public void SetPosition(Vector2 position)
    {
        RectTransform.anchoredPosition = position;
    }

    public void SetGridPosition(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public void SetScale(float scale)
    {
        RectTransform.localScale = Vector3.one * scale;
    }

    public void PlayInvalidFeedback()
    {
        feedbackTime = 0f;
        feedbackPlaying = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!inputEnabled)
            return;

        EventHandler.CallStarClickEvent(Row, Column);
    }
}
