using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour // player has an inventory (an instance of this object)
{
    public List<Items> ItemList { get; private set; } // the items you have
    public Inventory()
    {
        ItemList = new List<Items>();
    }
    public void AddItem(Item item)
    {
        ItemList.Add(item.ItemType);
        AddToUI(item);
    }
    void AddToUI(Item item)
    {
        // Create itemslot at the correct place
        GameObject itemSlot = new GameObject("itemSlot", typeof(RectTransform));
        itemSlot.transform.parent = GameObject.FindGameObjectWithTag("UIItemCollection").transform;

        // Add UI Item component to item slot
        itemSlot.AddComponent<UIItem>();
        UIItem itemSlot_UIItem = itemSlot.GetComponent<UIItem>();
        itemSlot_UIItem.item = item.ItemType;
        itemSlot_UIItem.prefab = Resources.Load("prefabs/" + item.ItemType.ToString()) as GameObject;

        // Add colliders
        itemSlot.AddComponent<BoxCollider2D>();
        BoxCollider2D itemSlot_BoxCollider2D = itemSlot.GetComponent<BoxCollider2D>();
        itemSlot_BoxCollider2D.offset = new Vector2(0, 0);
        itemSlot_BoxCollider2D.size = new Vector2(100, 100);

        // prefab object
        Debug.Log("item.ItemType.ToString(): " + item.ItemType.ToString());
        GameObject prefab = Resources.Load("prefabs/" + item.ItemType.ToString()) as GameObject; // empty for some reason
        GameObject objectWithPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        objectWithPrefab.transform.parent = itemSlot.transform;
        
    } // adds the item into your inventory UI
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