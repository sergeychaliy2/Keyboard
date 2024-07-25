using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    [Header("BUTTON_COLUMNS"), Space]
    [SerializeField] private Transform[] columnTransforms;

    public List<Button>[] buttonColumns;

    private void Awake()
    {
        buttonColumns = new List<Button>[columnTransforms.Length];

        for (int i = 0; i < columnTransforms.Length; i++)
        {
            buttonColumns[i] = new List<Button>();
            Button[] buttons = columnTransforms[i].GetComponentsInChildren<Button>();
            buttonColumns[i].AddRange(buttons);
        }
    }
}
