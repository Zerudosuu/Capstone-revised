using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Import DOTween namespace

public class AchievementPopUp : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public Achievement Achievement;
    public float animationDuration = 1f; // Time to move the pop-up
    public float stayDuration = 3f; // Time to stay before hiding
    private Vector3 offScreenPosition;
    private Vector3 onScreenPosition;

    private void Start()
    {
        offScreenPosition = new Vector3(transform.position.x, -Screen.height, transform.position.z);
        onScreenPosition = new Vector3(transform.position.x, Screen.height * 0.2f, transform.position.z);

        transform.position = offScreenPosition; // Start off-screen
        UpdateUI();
        AnimatePopUp();
    }

    public void SetAchievement(Achievement achievement)
    {
        Achievement = achievement;
        UpdateUI();
        AnimatePopUp();
    }

    private void UpdateUI()
    {
        if (Achievement != null)
        {
            Title.text = Achievement.id; // Using title for better readability
            Description.text = Achievement.description;
        }
    }

    private void AnimatePopUp()
    {
        // Move from bottom to screen center
        transform.DOMove(onScreenPosition, animationDuration).SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                // Stay for a few seconds, then move back down
                StartCoroutine(HidePopUp());
            });
    }

    private IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(stayDuration);

        // Move back to off-screen position
        transform.DOMove(offScreenPosition, animationDuration).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                Destroy(gameObject); // Optional: Destroy the pop-up after animation
            });
    }
}