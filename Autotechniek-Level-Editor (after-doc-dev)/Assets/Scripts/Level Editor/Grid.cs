using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid : MonoBehaviour
{
    int _tileSize = 100; // change this number to 50 or 200 and use the level editor to instantly understand what it does,
                         // basically _tileSize = width and _tileSize = length of a grid square
                         // so _tileSize = 200 means 200x200 squares for grid
    UIItemCollection _uiItemCollection;
    private void Start()
    {
        _uiItemCollection = GameObject.FindGameObjectWithTag("UIItemCollection").GetComponent<UIItemCollection>();
    }
    public void OnMouseDown()
    {
        // get the mouse location in the game
        Vector3 mousePosition = GetMousePositionInScene();
        PlaceItemOnScreen(mousePosition);
    }
    async void PlaceItemOnScreen(Vector3 mousePosition)
    {
        double x = mousePosition.x, y = mousePosition.y; // get the mouseposition

        // EXPLANATION FORMULA -> Math.Floor(x / _tileSize) * _tileSize + (_tileSize / 2);
        // get the center point of the grid tile you clicked in
        // for example, the mouseposition is [48, 138]. this means x = 48. _tileSize is 100 because the grid background i used has tiles of 100px. lets fill in the formula,
        // Math.Floor(48 / 100) * 100 + (100 / 2)
        // Math.Floor(0.48) * 100 + 50
        // 0 * 100 + 50
        // 50 is the final answer, this means the selected prefab/item will be instantiated on x = 50, now we also need the y
        // Math.Floor(138 / 100) * 100 + (100 / 2)
        // Math.Floor(1.38) * 100 + 50
        // 1 * 100 + 50
        // 150 is the final answer, so your selected prefab/item will be instantiated on the coordinates [50, 150]
        // for the final time here is the formula summarised
        // 
        // FIRST PART: Math.Floor(x / _tileSize) * _tileSize -> correct the mouse position so it is on the bottom left of a grid square
        // SECOND PART: + (_tileSize / 2); -> instead of bottom left, get the center position of the grid square. 
        // hope you understand it now

        x = Math.Floor(x / _tileSize) * _tileSize + (_tileSize / 2); 
        y = Math.Floor(y / _tileSize) * _tileSize + (_tileSize / 2);
        Debug.Log(x + ", " + y);

        Vector3 pos = new Vector3((float)x, (float)y, 0); // create position with new coordinates
        Instantiate(_uiItemCollection.SelectedPrefab, pos, new Quaternion()); // instantiate the new item on the correct location

        // autosave
        await Data.GridData.AutoSaveToTextFile(_uiItemCollection.SelectedPrefab, pos); // save it in the text file
    }
    Vector3 GetMousePositionInScene()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
