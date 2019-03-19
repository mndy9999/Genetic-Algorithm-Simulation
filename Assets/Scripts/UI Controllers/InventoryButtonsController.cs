using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtonsController : MonoBehaviour {

    public Button[] buttons;
    public Texture2D[] cursorTextures;
    Button activeButton;

    public void SetActiveMaleSheep() { activeButton = (activeButton == buttons[0]) ? null : buttons[0]; }
    public void SetActiveFemaleSheep() { activeButton = (activeButton == buttons[1]) ? null : buttons[1]; }
    public void SetActiveMaleWolf() { activeButton = (activeButton == buttons[2]) ? null : buttons[2]; }
    public void SetActiveFemaleWolf() { activeButton = (activeButton == buttons[3]) ? null : buttons[3]; }

    private void Update()
    {
        if (activeButton)
            for (int i = 0; i < cursorTextures.Length; i++)
            {
                if (activeButton == buttons[i]) Cursor.SetCursor(cursorTextures[i], Vector2.zero, CursorMode.Auto);
            }

        else { Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); }
    }

    public Button GetActiveButton { get { return activeButton; } }

}
