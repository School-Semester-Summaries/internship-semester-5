using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    const string LEVEL_EDITOR_SCENE = "Level Editor"; // same problem
    const string PLAY_SCENE = "Play"; // You can probably improve this by using tags or idk what you can do for scenes
    Text textObject;
    bool isHovering;

    void Start()
    {
        textObject = GetComponent<Text>();
    }

    // dit moet geen text zijn
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isHovering)
        {
            if (textObject.text == "Spelen") // This is bad, replace text == "" by tag == "" or something else. because if you change the text inside the textobject it doestn work anymore
            {
                SceneManager.LoadScene(PLAY_SCENE);
            }
            if (textObject.text == "Level Editor") // same problem 
            {
                SceneManager.LoadScene(LEVEL_EDITOR_SCENE);
                GameModeManager.SetGamemode(Gamemodes.Level_Editor);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        textObject.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        textObject.color = Color.black;
    }

}
