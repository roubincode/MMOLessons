using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMessageSlot : MonoBehaviour
{
    public Text text;

    [HideInInspector] public ChatMessage message;
    public FontStyle mouseOverStyle = FontStyle.Italic;
    FontStyle defaultStyle;

}
