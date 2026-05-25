using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIExperienceBar : MonoBehaviour
{
    public static UIExperienceBar instance;

    [Header("UI References")]
    public Image xpFillImage; // Kéo thả XPBar_Fill vào đây trên Inspector
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI coinAmountText;

    [Header("Experience Stats")]
    //public int currentLevel = 1;
    private float timer = 0f;
    private long coinAmount = 0;

    void UpdateTimeDisplay()
    {
        // Định dạng thời gian thành Phút:Giây
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void UpdateExperienceBar(long xp1, long xp2)
    {
        //Debug.Log("Gain exp: " + xp1 + " / " + xp2);
        if (xpFillImage != null)
        {
            // Tính toán tỷ lệ phần trăm (từ 0.0 đến 1.0)
            xpFillImage.fillAmount = (float)xp1 / xp2;
        }
    }

    public void SetLevelText(int level, long xpToNextLevel)
    {
        //Debug.Log("Level up to lv" + level);
        //currentLevel++;
        // Update level UI
        levelText.text = "LV" + level.ToString();
        UpdateExperienceBar(0, xpToNextLevel);

    }

    public void UpdateCoinAmountUI( int coins){
        coinAmount+=coins;
        coinAmountText.text = coinAmount.ToString();
    }

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateExperienceBar(0, 10);
        levelText.text = "LV1";
        coinAmountText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        UpdateTimeDisplay();
    }

}
