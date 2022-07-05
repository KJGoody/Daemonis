using UnityEngine;
using UnityEngine.UI;

public class ItemAddOptionInfo : MonoBehaviour
{
    private ItemAddOption itemAddOption;

    [SerializeField]
    private Sprite[] QualityImage;
    [SerializeField]
    private Image QualityIcon;
    [SerializeField]
    private Text optionText;

    private string option_Name;

    public void SetAddOptionPrefab(ItemAddOption addoption)
    {
        itemAddOption = addoption;
        option_Name = ItemAddOptionScript.Instance.GetName(itemAddOption.Num);
        SetOption();
    }

    public void SetOption() // 이미지랑 텍스트 설정
    {
        QualityIcon.sprite = QualityImage[itemAddOption.Quality];
        optionText.text = option_Name + " " + itemAddOption.value.ToString("F2");
    }
}
