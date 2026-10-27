using UnityEngine;
using UnityEngine.Events;

public class UIAnimationKeyboardTest : MonoBehaviour
{
    [Header("Keyboard Settings")]
    public KeyCode triggerKey = KeyCode.P;

    [Header("Events")]
    public UnityEvent onTrigger;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            TriggerUiAnimations();
        }
    }

    public void TriggerUiAnimations()
    {
        onTrigger?.Invoke();
    }
}