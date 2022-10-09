using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class TextInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField _textInput;

    public void HandleTextInput()
    {
        HandleUserInput.handleUserInputSelection(int.Parse(_textInput.text));
        _textInput.text = "";
    }
}
