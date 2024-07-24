using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour, IButtonSelector
{
    [Header("INPUT_ACTION"),Space]
    [SerializeField] private KeyboardManager keyboardManager;
    [SerializeField] private InputAction navigationAction;
    [SerializeField] private InputAction confirmSelectionAction;

    private int currentIndex = -1;
    private int previousIndex = -1;

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
        Vector2 axisValue = context.ReadValue<Vector2>();

        if (axisValue.x < -0.5f)
        {
            SelectPreviousButton();
        }
        else if (axisValue.x > 0.5f)
        {
            SelectNextButton();
        }
    }

    private void OnConfirmSelection(InputAction.CallbackContext context)
    {
        ConfirmSelection();
    }

    public void SelectPreviousButton()
    {
        if (keyboardManager.keyboardButtons.Count > 0)
        {
            previousIndex = currentIndex;

            if (currentIndex > 0)
            {
                currentIndex--;
            }
            else if (currentIndex == -1)
            {
                currentIndex = 0;
            }

            HighlightButton(previousIndex);
        }
    }

    public void SelectNextButton()
    {
        if (keyboardManager.keyboardButtons.Count > 0)
        {
            previousIndex = currentIndex;

            if (currentIndex < keyboardManager.keyboardButtons.Count - 1)
            {
                currentIndex++;
            }
            else if (currentIndex == -1)
            {
                currentIndex = 0;
            }

            HighlightButton(previousIndex);
        }
    }

    public void ConfirmSelection()
    {
        if (currentIndex != -1)
        {
            Button selectedButton = keyboardManager.keyboardButtons[currentIndex];
            if (selectedButton != null)
            {
                selectedButton.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }
    }

    private void HighlightButton(int previousIndex)
    {
        if (previousIndex != -1)
        {
            Button previousButton = keyboardManager.keyboardButtons[previousIndex];
            if (previousButton != null)
            {
                previousButton.OnPointerExit(new PointerEventData(EventSystem.current));
            }
        }

        if (currentIndex != -1)
        {
            Button currentButton = keyboardManager.keyboardButtons[currentIndex];
            if (currentButton != null)
            {
                currentButton.OnPointerEnter(new PointerEventData(EventSystem.current));
            }
        }
    }
}
