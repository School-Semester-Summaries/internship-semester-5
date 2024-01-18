using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIItemCollection : MonoBehaviour // uiitemcollection is the collection of uiitems, for example the uiitem collection in the level editor or the inventory in the play section
{
    public GameObject SelectedPrefab { get; private set; }
    public Items SelectedItemType { get { return _selectedItem.ItemType; } }
    public bool EraserSelected { get; private set; }
    Item _selectedItem = new Item();
    UIItem _currentUIItem = null;
    UIItem _previousUIItem = null;
    bool _firstTime = false; // use this to check if its the first time pressing an uiitem, if it is the first time, it doesn't try to delete border on the previous uititem because there is no previous

    private void Start()
    {
        Debug.Log(GameModeManager.Gamemode);
        if (GameModeManager.Gamemode == Gamemodes.Level_Editor)
        {
            CreateUIInventory();
        }

        void CreateUIInventory()
        {
            GameObject[] prefabs = Resources.LoadAll<GameObject>("prefabs/items");

            foreach (GameObject prefab in prefabs)
            {
                Debug.Log("Create Item");
                CreateSingleUIItem(prefab);
            }

            void CreateSingleUIItem(GameObject prefab)
            {
                // retrive data from prefab
                Item prefabItem = prefab.GetComponent<Item>();

                // create gameobject
                GameObject itemSlot = new GameObject("itemSlot", typeof(RectTransform));
                itemSlot.transform.parent = this.transform; // same as -> itemSlot.transform.parent = GameObject.FindGameObjectWithTag("UIItemCollection")

                // Add UI Item component to item slot
                itemSlot.AddComponent<UIItem>();
                UIItem itemSlot_UIItem = itemSlot.GetComponent<UIItem>();
                itemSlot_UIItem.item = prefabItem.ItemType;
                itemSlot_UIItem.prefab = Resources.Load("prefabs/items/" + prefabItem.ItemType.ToString()) as GameObject;

                // Add Collider
                itemSlot.AddComponent<BoxCollider2D>();
                BoxCollider2D itemSlot_BoxCollider2D = itemSlot.GetComponent<BoxCollider2D>();
                itemSlot_BoxCollider2D.offset = new Vector2(0, 0);
                itemSlot_BoxCollider2D.size = new Vector2(100, 100);

                // Add prefab as child
                GameObject instantiatedPrefab = Instantiate(prefab);
                instantiatedPrefab.transform.parent = itemSlot.transform;

                // Disable the box collider of them item so that you can only click on the UIItem
                Item instantiatedPrefab_Item = instantiatedPrefab.GetComponent<Item>();
                BoxCollider2D instantiatedPrefab_Item_BoxCollider2D = instantiatedPrefab_Item.GetComponent<BoxCollider2D>();
                instantiatedPrefab_Item_BoxCollider2D.enabled = false;

                // exceptions
                if (prefabItem.ItemType == Items.Car)
                {
                    instantiatedPrefab.transform.position = new Vector2(0, 0);
                    instantiatedPrefab.transform.localScale = new Vector2(66.6666f, 100);
                }

            } // this is basically same code as in inventory
        }
    }

    public void SelectItem(GameObject prefab)
    {
        DeselectPreviousItem();
        DeselectEraser();
        if (prefab != null)
        {
            _selectedItem = prefab.GetComponent<Item>();
            SelectedPrefab = prefab;
            Debug.Log("Selected Item/Prefab: " + prefab.ToString());
            Debug.Log(prefab.GetComponent<Item>().ItemType);
        } // if it is null you clicker on the eraser probably
        _previousUIItem = _currentUIItem;
    }
    public void SelectEraser()
    {
        EraserSelected = true;
        Debug.Log("Eraser Selected");
    }
    public void SetUIItem(UIItem uiItem)
    {
        _currentUIItem = uiItem;
    }
    void DeselectEraser()
    {
        EraserSelected = false;
        Debug.Log("Eraser Deselected");
    } // felt like deselecting eraser didn't work too well. you should be able to still remove items while the eraser is "deselected" maybe you can try to fix it but not that important
    void DeselectPreviousItem()
    {
        if (_firstTime)
        {
            _previousUIItem.DeleteBorder();
        }
        else
        {
            _firstTime = true;
        }
    }
}
