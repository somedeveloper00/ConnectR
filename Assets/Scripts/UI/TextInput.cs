using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class TextInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField _textInput;

    public void HandleTextInput()
    {
        GameManager.instance.HandleUserInput(_textInput.text);
        _textInput.text = "";
    }
}
