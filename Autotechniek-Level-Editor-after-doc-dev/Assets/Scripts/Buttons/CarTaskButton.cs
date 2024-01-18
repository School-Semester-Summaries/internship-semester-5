using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarTaskButton : MonoBehaviour
{
    public Button Button;
    public TextMeshProUGUI ButtonText;
    public CarTask CarTask { get; private set; } // the cartask inside the button
    Color _color; // color of the button

    void Start()
    {
        Button.GetComponent<Button>().onClick.AddListener(ButtonClick);
    }

    void ButtonClick()
    {
        _color = GetComponent<Image>().color;

        if (_color == Color.green)
        {
            Deactivate();
        }
        else if (_color == Color.white)
        {
            Activate();
        }
    }
    async void Activate()
    {
        this.GetComponent<Image>().color = Color.green;
        await Data.CarTaskData.AddDataEntry(CarTask.ID);
    }
    async void Deactivate()
    {
        this.GetComponent<Image>().color = Color.white;
        await Data.CarTaskData.RemoveDataEntry(CarTask.ID);
    }
    public void SetCarTask(CarTask c)
    {
        CarTask = c;
    }
    public void SetButtonText(string s)
    {
        ButtonText.text = s;
    }
    void CheckAvailibilty()
    {
        //add when time left
        //Are all objects that are required for my cartask present on the grid? then enable the user to click this cartask
    }
}
