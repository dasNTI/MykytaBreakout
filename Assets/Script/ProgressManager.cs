using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static GameSave State;
    public static string SaveName = "Save";
    
    public static void Load()
    {

    }

    public static void Save()
    {

    }

    public static void New()
    {
        State = new GameSave();
    }
}

[System.Serializable]
public class GameSave
{
    public bool Inventory_Hook = false;
    public bool Inventory_Mop = false;
    public bool Inventory_MopPlusHook = false;
    public bool Inventory_AtticHook = false;
    public bool Inventory_Shoelace = false;

    public bool Hotelroom_ClosetFirstOpened = false;
    public bool Hotelroom_ClosetOpened = false;
    public bool Hotelroom_HookTaken = false;
    public bool Hotelroom_ShoelaceTaken = false;

    public bool Hallway_AtticOpened = false;

    public bool Attic_LadderTaken = false;
}

