using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItem : MonoBehaviour
{
    const string FILE_LOCATION_BORDER = "prefabs/border";
    const string TAG_UIITEMCOLLECTION = "UIItemCollection";
    const string TAG_PLAYER = "Player";
    public Items item; // the item inside this uiitem
    public GameObject prefab; // the prefab inside the uiitem
    GameObject _border; // the thin white border around the uiitem. Makes it easy to understand that the uiitem has been selected.
    UIItemCollection _uiItemCollection; // collection of all uiitems
    CarTask1 _carTask1; // minigame 1, might be unnecessary to make a field of this
    private void Start()
    {
        // find ui item collection
        _uiItemCollection = GameObject.FindGameObjectWithTag(TAG_UIITEMCOLLECTION).GetComponent<UIItemCollection>();
    }
    private void OnMouseDown()
    {
        // are you in a cartask?
        if (GameModeManager.Gamemode == Gamemodes.CarTask)
        {
            _carTask1 = (CarTask1)GameObject.FindGameObjectWithTag(TAG_PLAYER).GetComponent<Player>().TaskList[0];
            _carTask1.Minigame_UIItem(prefab, this); // if you interact with a uiitem while being in a cartask, it gets send through to the cartask, potential for better code, you can maybe make it currentCartask, instead of Cartask1 understand?
        }
        _border = CreateBorder(); // give the uiitem a border
        _uiItemCollection.SetUIItem(this); // select current uiitem
        _uiItemCollection.SelectItem(prefab);
    }
    GameObject CreateBorder()
    {
        GameObject border = Instantiate(Resources.Load(FILE_LOCATION_BORDER) as GameObject);
        border.transform.parent = gameObject.transform; // add border to Uiitem
        border.transform.localPosition = Vector3.zero; // put the border in the center of uiitem
        return border;
    }
    public void DeleteBorder()
    {
        Destroy(_border);
    }
}
