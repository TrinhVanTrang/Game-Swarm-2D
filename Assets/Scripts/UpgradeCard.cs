using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI levelText;

    private ItemData itemData;
    private int currentLevel;
    private System.Action<ItemData> onSelectedCallback;

    // Hàm để nạp dữ liệu từ Manager vào Thẻ
    public void SetupCard(ItemData data, System.Action<ItemData> onSelected)
    {
        itemData = data;
        currentLevel = data.level;
        onSelectedCallback = onSelected;

        iconImage.sprite = data.icon;
        nameText.text = data.name;
        descriptionText.text = data.description;
        levelText.text = data.level == 1 ? "NEW" : $"Lv.{data.level}";
    }

    // Gắn hàm này vào sự kiện OnClick của Button nâng cấp
    public void OnCardClicked()
    {
        onSelectedCallback?.Invoke(itemData);
    }
}