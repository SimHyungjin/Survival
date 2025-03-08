using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData ItemData;

    public Button btn;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Outline outline;

    public UIInventory uIInventory;


    public int index;
    public bool equipped;
    public int quantity;

    private void OnEnable()
    {
        outline.enabled = equipped;
    }
    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = ItemData.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
        outline.enabled = equipped;
    }
    public void Clear()
    {
        ItemData = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnClickBtn()
    {
        uIInventory.SelectItem(index);
    }
}
