using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemCollection : MonoBehaviour // uiitemcollection is the collection of uiitems, for example the uiitem collection in the level editor or the inventory in the play section
{
    public GameObject SelectedPrefab { get; private set; }
    public bool EraserSelected { get; private set; }
    public Items SelectedItemType { get { return _selectedItem.ItemType; } }
    private Item _selectedItem = new Item();

    UIItem _currentUIItem = null;
    UIItem _previousUIItem = null;
    private bool _firstTime = false; // use this to check if its the first time pressing an uiitem, if it is the first time, it doesn't try to delete border on the previous uititem because there is no previous
    public void SelectItem(GameObject prefab)
    {
        DeselectPreviousItem();
        DeselectEraser();
        _selectedItem = prefab.GetComponent<Item>();
        SelectedPrefab = prefab;
        _previousUIItem = _currentUIItem;

        Debug.Log("Selected Item/Prefab: " + prefab.ToString());
        Debug.Log(prefab.GetComponent<Item>().ItemType);
    }
    public void SelectEraser()
    {
        EraserSelected = true;
        Debug.Log("Eraser Selected");
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
    public void SetUIItem(UIItem uiItem)
    {
        _currentUIItem = uiItem;
    }
}
