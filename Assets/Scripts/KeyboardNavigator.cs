using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyboardNavigation : MonoBehaviour
{
    public Button[] buttons; // Массив кнопок для навигации
    private int selectedIndex = 0;

    void Start()
    {
        if (buttons.Length > 0)
        {
            SelectButton(buttons[selectedIndex]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SelectPreviousButton();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SelectNextButton();
        }
        if (Input.GetKeyDown(KeyCode.Return)) // Нажатие клавиши Enter для выбора кнопки
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    void SelectButton(Button button)
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
        button.OnSelect(null);
    }

    void SelectNextButton()
    {
        if (buttons.Length == 0) return;

        selectedIndex = (selectedIndex + 1) % buttons.Length;
        SelectButton(buttons[selectedIndex]);
    }

    void SelectPreviousButton()
    {
        if (buttons.Length == 0) return;

        selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
        SelectButton(buttons[selectedIndex]);
    }
}
