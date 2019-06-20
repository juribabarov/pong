using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameSettings : MonoBehaviour
{
    public enum PlayerType
    {
        Human,
        PC
    }

    private PlayerType selectedPlayerTypeLeft;
    private PlayerType selectedPlayerTypeRight;

    private void Start()
    {
        selectedPlayerTypeLeft = PlayerType.Human;
        selectedPlayerTypeRight = PlayerType.Human;
    }

    public void SwitchPlayerLeft(Text targetText)
    {
        if ((int)selectedPlayerTypeLeft < Enum.GetNames(typeof(PlayerType)).Length - 1)
        {
            selectedPlayerTypeLeft++;
        }
        else
        {
            selectedPlayerTypeLeft = 0;
        }

        targetText.text = selectedPlayerTypeLeft.ToString();
    }

    public void SwitchPlayerRight(Text targetText)
    {
        if ((int)selectedPlayerTypeRight < Enum.GetNames(typeof(PlayerType)).Length - 1)
        {
            selectedPlayerTypeRight++;
        }
        else
        {
            selectedPlayerTypeRight = 0;
        }

        targetText.text = selectedPlayerTypeRight.ToString();
    }
}
