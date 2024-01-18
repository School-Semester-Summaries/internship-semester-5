using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Gamemodes
{
    Menu,
    Level_Editor,
    Play,
    CarTask // aka minigame
}

public class GameModeManager 
{
    public static Gamemodes Gamemode { get; private set; } // the current gamemode
    public static void SetGamemode(Gamemodes g)
    {
        Gamemode = g;
    }
}
