using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BNG
{
    public class VRKeyboard : MonoBehaviour
    {

        public UnityEngine.UI.InputField AttachedInputField;

        public bool UseShift = false;

        [Header("Sound FX")]
        public AudioClip KeyPressSound;

        List<VRKeyboardKey> KeyboardKeys;

        void Awake()
        {
            KeyboardKeys = transform.GetComponentsInChildren<VRKeyboardKey>().ToList();
        }

        public void PressKey(string key)
        {
            if (AttachedInputField != null)
            {
                UpdateInputField(key);
            }
            else
            {
                Debug.Log("Pressed Key : " + key);
            }
        }

        public void UpdateInputField(string key)
        {
            string currentText = AttachedInputField.text;
            int caretPosition = AttachedInputField.caretPosition;

            // Formatted key based on short names
            string formattedKey = key.ToLower() == "space" ? " " : key;

            // Find KeyCode Sequence
            if (formattedKey.ToLower() == "backspace")
            {
                if (currentText.Length > 0) // Check if there is any text to remove
                {
                    currentText = currentText.Remove(currentText.Length - 1, 1); // Remove last character
                }
            }
            else if (formattedKey.ToLower() == "enter")
            {
                // Handle Enter key if needed
                // UnityEngine.EventSystems.ExecuteEvents.Execute(AttachedInputField.gameObject, null, UnityEngine.EventSystems.ExecuteEvents.submitHandler);
            }
            else if (formattedKey.ToLower() == "shift")
            {
                ToggleShift();
                return; // Skip other operations when Shift is pressed
            }
            else
            {
                // Add text to the end of the current text
                currentText += formattedKey;
            }

            // Apply the text change and update caret position to the end
            AttachedInputField.text = currentText;
            AttachedInputField.caretPosition = currentText.Length; // Move caret position to the end

            PlayClickSound();

            // Keep Input Focused
            if (!AttachedInputField.isFocused)
            {
                AttachedInputField.Select();
            }
        }

        public virtual void PlayClickSound()
        {
            if (KeyPressSound != null)
            {
                VRUtils.Instance.PlaySpatialClipAt(KeyPressSound, transform.position, 1f, 0.5f);
            }
        }

        public void ToggleShift()
        {
            UseShift = !UseShift;

            foreach (var key in KeyboardKeys)
            {
                if (key != null)
                {
                    key.ToggleShift();
                }
            }
        }

        public void AttachToInputField(UnityEngine.UI.InputField inputField)
        {
            AttachedInputField = inputField;
        }
    }
}
