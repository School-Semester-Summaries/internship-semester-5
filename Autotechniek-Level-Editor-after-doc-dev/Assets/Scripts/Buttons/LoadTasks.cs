using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadTasks : MonoBehaviour
{
    const string TAG_TASK_NAME = "TaskName";
    const string TAG_TASK_DESCRIPTION = "TaskDescription";
    const string TAG_TASK_TIPS = "TaskTips";

    public Button LoadButton;
    public List<CarTask> TaskList { get; private set; }
    
    TextMeshProUGUI _taskTips;
    TextMeshProUGUI _taskName;
    TextMeshProUGUI _taskDescription;

    void Start()
    {
        GameModeManager.SetGamemode(Gamemodes.Play);
        TaskList = Data.CarTaskData.LoadCarTasksFromTextFile(); // load cartasks from textfile
        //TaskList = new CarTaskCollection().AllTasks; // HARDCODE: Assign all tasks to player, only use for testing (this is used in the WEBGL build on itch.io)
        LoadButton.GetComponent<Button>().onClick.AddListener(ButtonClick);
        FindTextBoxes();

        void FindTextBoxes()
        {
            _taskName = GameObject.FindGameObjectWithTag(TAG_TASK_NAME).GetComponent<TextMeshProUGUI>();
            _taskDescription = GameObject.FindGameObjectWithTag(TAG_TASK_DESCRIPTION).GetComponent<TextMeshProUGUI>();
            _taskTips = GameObject.FindGameObjectWithTag(TAG_TASK_TIPS).GetComponent<TextMeshProUGUI>();
        }
    }
    void ButtonClick()
    {
        LoadInFirstTask();
    }
    void LoadInFirstTask()
    {
        _taskName.text = TaskList[0].Name;
        _taskDescription.text = TaskList[0].Description;
        _taskTips.text = "";
    }
    public void LoadInTask(CarTask c)
    {
        _taskName.text = c.Name;
        _taskDescription.text = c.Description;
        _taskTips.text = "";
    }
    public void ClearTextBoxes()
    {
        _taskName.text = "";
        _taskDescription.text = "";
        _taskTips.text = "";
    }
    public void Finish()
    {
        _taskName.text = "Je hebt alle taken voltooid";
        _taskDescription.text = "";
        _taskTips.text = "";
    }
    public void SetTaskTip(string s)
    {
        _taskTips.text = s;
    }
}
