using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    [Header("KEYBUTTONS"),Space]
    [SerializeField][Space]
    public List<Button> keyboardButtons;

    private void Start()
    {
        keyboardButtons = new List<Button>(GetComponentsInChildren<Button>());
    }
}