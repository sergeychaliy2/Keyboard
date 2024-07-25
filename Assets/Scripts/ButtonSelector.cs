using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonSelector : MonoBehaviour, IButtonSelector
{
    [Header("INPUT_ACTION"), Space]
    [SerializeField] private InputAction navigationAction;
    [SerializeField] private InputAction confirmSelectionAction;
    [SerializeField] private float navigationDelay = 0.03f;

    [Header("BUTTON_COLUMNS"), Space]
    [SerializeField] private KeyboardManager keyboardManager;

    private int currentColumnIndex = 0;
    private int currentRowIndex = 0;
    private float lastNavigationTime;

    private void OnEnable()
    {
        navigationAction.Enable();
        confirmSelectionAction.Enable();

        navigationAction.performed += OnNavigate;
        confirmSelectionAction.performed += OnConfirmSelection;
    }

    private void OnDisable()
    {
        navigationAction.Disable();
        confirmSelectionAction.Disable();

        navigationAction.performed -= OnNavigate;
        confirmSelectionAction.performed -= OnConfirmSelection;
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        if (Time.time - lastNavigationTime < navigationDelay)
        {
            return;
        }

        Vector2 axisValue = context.ReadValue<Vector2>();

        if (axisValue.x < -0.5f)
        {
            SelectPreviousButtonInColumn();
        }
        else if (axisValue.x > 0.5f)
        {
            SelectNextButtonInColumn();
        }
        else if (axisValue.y > 0.5f)
        {
            SelectPreviousButtonInRow();
        }
        else if (axisValue.y < -0.5f)
        {
            SelectNextButtonInRow();
        }

        lastNavigationTime = Time.time;
    }

    private void OnConfirmSelection(InputAction.CallbackContext context)
    {
        ConfirmSelection();
    }

    public void SelectPreviousButtonInColumn()
    {
        if (keyboardManager.buttonColumns.Length == 0)
            return;

        if (currentRowIndex > 0)
        {
            HighlightButton(currentColumnIndex, currentRowIndex, false);
            currentRowIndex--;
            HighlightButton(currentColumnIndex, currentRowIndex, true);
        }
    }

    public void SelectNextButtonInColumn()
    {
        if (keyboardManager.buttonColumns.Length == 0)
            return;

        if (currentRowIndex < keyboardManager.buttonColumns[currentColumnIndex].Count - 1)
        {
            HighlightButton(currentColumnIndex, currentRowIndex, false);
            currentRowIndex++;
            HighlightButton(currentColumnIndex, currentRowIndex, true);
        }
    }

    public void SelectPreviousButtonInRow()
    {
        if (keyboardManager.buttonColumns.Length == 0)
            return;

        if (currentColumnIndex > 0)
        {
            int newRowIndex = Mathf.Min(currentRowIndex, keyboardManager.buttonColumns[currentColumnIndex - 1].Count - 1);
            HighlightButton(currentColumnIndex, currentRowIndex, false);
            currentColumnIndex--;
            currentRowIndex = newRowIndex;
            HighlightButton(currentColumnIndex, currentRowIndex, true);
        }
    }

    public void SelectNextButtonInRow()
    {
        if (keyboardManager.buttonColumns.Length == 0)
            return;

        if (currentColumnIndex < keyboardManager.buttonColumns.Length - 1)
        {
            int newRowIndex = Mathf.Min(currentRowIndex, keyboardManager.buttonColumns[currentColumnIndex + 1].Count - 1);
            HighlightButton(currentColumnIndex, currentRowIndex, false);
            currentColumnIndex++;
            currentRowIndex = newRowIndex;
            HighlightButton(currentColumnIndex, currentRowIndex, true);
        }
    }

    public void ConfirmSelection()
    {
        if (keyboardManager.buttonColumns.Length > 0 && currentRowIndex < keyboardManager.buttonColumns[currentColumnIndex].Count)
        {
            Button selectedButton = keyboardManager.buttonColumns[currentColumnIndex][currentRowIndex];
            if (selectedButton != null)
            {
                selectedButton.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }
    }

    private void HighlightButton(int columnIndex, int rowIndex, bool highlight)
    {
        if (columnIndex >= 0 && rowIndex >= 0 && columnIndex < keyboardManager.buttonColumns.Length)
        {
            if (rowIndex < keyboardManager.buttonColumns[columnIndex].Count)
            {
                Button button = keyboardManager.buttonColumns[columnIndex][rowIndex];
                if (button != null)
                {
                    PointerEventData eventData = new PointerEventData(EventSystem.current);

                    if (highlight)
                    {
                        button.OnPointerEnter(eventData);
                    }
                    else
                    {
                        button.OnPointerExit(eventData);
                    }
                }
            }
        }
    }
}