using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour // player has an inventory (an instance of this object)
{
    const string TAG_UIITEM_COLLECTION = "UIItemCollection";
    public List<Items> ItemList { get; private set; } // the items you have
    public Inventory()
    {
        ItemList = new List<Items>();
    }
    public void AddItem(Item item)
    {
        ItemList.Add(item.ItemType);
        AddToUI(item);

        void AddToUI(Item item)
        {
            // Create itemslot at the correct place
            GameObject itemSlot = new GameObject("itemSlot", typeof(RectTransform));
            itemSlot.transform.parent = GameObject.FindGameObjectWithTag(TAG_UIITEM_COLLECTION).transform;

            // Add UI Item component to item slot
            itemSlot.AddComponent<UIItem>();
            UIItem itemSlot_UIItem = itemSlot.GetComponent<UIItem>();
            itemSlot_UIItem.item = item.ItemType;
            itemSlot_UIItem.prefab = Resources.Load("prefabs/items/" + item.ItemType.ToString()) as GameObject;

            // Add colliders
            itemSlot.AddComponent<BoxCollider2D>();
            BoxCollider2D itemSlot_BoxCollider2D = itemSlot.GetComponent<BoxCollider2D>();
            itemSlot_BoxCollider2D.offset = new Vector2(0, 0);
            itemSlot_BoxCollider2D.size = new Vector2(100, 100);

            // prefab object
            GameObject prefab = Resources.Load("prefabs/items/" + item.ItemType.ToString()) as GameObject; // empty for some reason
            GameObject objectWithPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            objectWithPrefab.transform.parent = itemSlot.transform;

            // remove the box collider of the prefab (otherwise it has two colliders Item & UIItem)
            Transform itemSlot_Child = itemSlot.transform.GetChild(0);
            BoxCollider2D itemSlot_Child_BoxCollider2D = itemSlot_Child.GetComponent<BoxCollider2D>();
            itemSlot_Child_BoxCollider2D.enabled = false;

        } // adds the item into your inventory UI
    }
    public void RemoveItem(Items item)
    {
        if (ItemList.Contains(item)) // remove the item if you have it
        {
            ItemList.Remove(item);
        }
        else // if you dont have an item and you try to remove it it gets logged
        {
            Debug.Log("Player does not own: " + item);
        }
    }
    public bool HasItem(params Items[] itemArray)
    {
        foreach (Items item in itemArray)
        {
            if (!ItemList.Contains(item))
            {
                return false;
            }
        }
        return true;
    } // you can use this method without giving a parameter, if you dont like that see archived method HasItem

    //ARCHIVED METHODS
    //public bool HasItem(Items item, params Items[] itemArray)
    //{
    //    // add the item to the array or whatever
    //    foreach (Items i in itemArray)
    //    {
    //        if (!ItemList.Contains(item))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
}