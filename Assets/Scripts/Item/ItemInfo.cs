using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractName();
    public string GetInteractDescription();
    public void OnInteract();
}
public class ItemInfo : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractName()
    {
        string str = $"{data.ItemName}";
        return str ;
    }

    public string GetInteractDescription()
    {
        string str = $"{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
