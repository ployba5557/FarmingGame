using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public SpriteRenderer overlayRenderer;  // ตัวแสดง Sprite
    public Gradient overlayColor;           // สีของ Overlay
    public AnimationCurve overlayAlpha;     // ความโปร่งใสของ Overlay
    public float dayStart = 6f;
    public float dayEnd = 18f;

    void Update()
    {
        if (TimeController.instance != null)
        {
            float time = TimeController.instance.CurrentTime;
            float t = Mathf.InverseLerp(dayStart, dayEnd, TimeController.instance.CurrentTime);
            UpdateOverlay(t);
        }
    }

    void UpdateOverlay(float t)
    {
        Color c = overlayColor.Evaluate(t);
        float a = overlayAlpha.Evaluate(t);
        c.a = a;
        overlayRenderer.color = c;
    }
}
