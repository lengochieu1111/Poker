using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUI_Canvas : RyoMonoBehaviour
{
    public void PressedStartButton()
    {
        AudioManager.Instance.PlayAudio_Pressed();
        UIManager.Instance.StartGame();
    }
}
