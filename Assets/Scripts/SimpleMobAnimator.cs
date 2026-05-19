using UnityEngine;

public class SimpleMobAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] animationFrames; // Mảng chứa các frame ảnh (ví dụ: 2-4 frame walk)
    [SerializeField] private float frameRate = 0.15f;  // Thời gian đổi frame (giây)

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // Reset lại khi quái vật được lôi ra từ Pool
        currentFrame = 0;
        timer = 0f;
        if (animationFrames.Length > 0)
        {
            spriteRenderer.sprite = animationFrames[0];
        }
    }

    void Update()
    {
        if (animationFrames.Length <= 1) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate; // Giữ lại thời gian thừa để animation mượt hơn
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            spriteRenderer.sprite = animationFrames[currentFrame];
        }
    }
}