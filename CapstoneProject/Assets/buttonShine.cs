using UnityEngine;
using UnityEngine.UI;

public class ButtonShine : MonoBehaviour
{
    private Button button;
    private Animator anim;

    private void OnEnable()
    {
        StepManager.OnStepBroadcasted += ShineButton; // Listen for event
    }

    private void OnDisable()
    {
        StepManager.OnStepBroadcasted -= ShineButton;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        button = GetComponent<Button>();
    }

    private void ShineButton()
    {
        button.GetComponent<Image>().color = Color.red; // Highlight effect
        anim.Play("ButtonOpenState");
    }
}