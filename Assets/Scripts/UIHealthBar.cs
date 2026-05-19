using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance;
    [Header("Health Settings")]
    // public float maxHealth;
    // private float currentHealth;

    [Header("UI References")]
    public Image fillImage;
    //public Transform playerPos;

    // void FixedUpdate()
    // {
    //     if (playerPos != null)
    //         transform.position = new Vector3(playerPos.position.x,
    //             playerPos.position.y + 0.25f, playerPos.position.z);
    // }

    public void UpdateBar(float currentHealth, float maxHealth)
    {
        // Tính toán phần trăm máu (từ 0.0 đến 1.0)
        float healthPercentage = (float)currentHealth / maxHealth;
        // Cập nhật độ dài của thanh máu
        fillImage.fillAmount = healthPercentage;

        // Cập nhật màu sắc dựa trên phần trăm
        if (healthPercentage > 0.6f)
        {
            fillImage.color = Color.green; // Trên 60% -> Xanh lá
        }
        else if (healthPercentage > 0.3f)
        {
            // Unity không có màu cam mặc định, ta dùng mã RGB (R=1, G=0.5, B=0)
            //fillImage.color = new Color(1f, 0.5f, 0f); // 30% - 60% -> Cam
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.red; // Dưới 30% -> Đỏ
        }
    }

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
