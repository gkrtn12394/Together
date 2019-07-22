using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Input : MonoBehaviour
{
    public InputField inputText;

    public void OnValueChanged(Text text)
    {
        text.text = inputText.text;
    }
}
