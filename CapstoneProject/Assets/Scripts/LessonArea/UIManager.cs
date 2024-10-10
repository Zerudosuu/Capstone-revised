using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("MainMenuButtons")]
    [SerializeField]
    private Button ProfileButton;

    [SerializeField]
    private Button CurrentLessonQuestButton;

    [SerializeField]
    private Button Bag;

    [SerializeField]
    private Button LessonContainer;

    [SerializeField]
    private Button Settings;

    [Header("Windows")]
    [SerializeField]
    private GameObject ProfileWindow;

    [SerializeField]
    private GameObject CurrentLessonWindow;

    [SerializeField]
    private GameObject BagWindow;

    [SerializeField]
    private GameObject LessonContainerWindow;

    [SerializeField]
    private GameObject SettingsWindow;

    [Header("Navbar")]
    [SerializeField] private RectTransform navbar_BG;
    [SerializeField] private bool NavBarAnimationActivate;
    private Button previouslySelectedBtn = null;
    private string previousState = null;
    private float transitionDuration = 0.5f;
    
    [Header("Navbar Button")]
    [SerializeField] private Button profileBtn;
    [SerializeField] private Button CurrentQuestBtn;
    [SerializeField] private Button bagBtn;
    [SerializeField] private Button LessonsBtn;
    [SerializeField] private Button SettingsBtn;

    [Header("Animator")]
    [SerializeField] private Animator navBarAnimator;

    [Header("Others")]
    [SerializeField]
    private GameObject PlayerUI;

   

    void Start()
    {
        BagWindow.SetActive(true);
        ProfileWindow.SetActive(false);

        LessonContainerWindow.SetActive(false);
        SettingsWindow.SetActive(false);

        StartCoroutine(MoveNavBar(bagBtn, "isBag"));
    }

    public void OnBagButtonClick()
    {
        BagWindow.SetActive(true);
        ProfileWindow.SetActive(false);
        CurrentLessonWindow.SetActive(false);
        LessonContainerWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        PlayerUI.SetActive(true);

        StartCoroutine(MoveNavBar(bagBtn, "isBag"));
    }

    public void OnProfileButtonClick()
    {
        ProfileWindow.SetActive(true);
        BagWindow.SetActive(false);
        CurrentLessonWindow.SetActive(false);
        LessonContainerWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        PlayerUI.SetActive(false);

        StartCoroutine(MoveNavBar(profileBtn, "isProfile"));
    }

    public void OnCurrentLessonQuestButtonClick()
    {
        CurrentLessonWindow.SetActive(true);
        BagWindow.SetActive(false);
        ProfileWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        LessonContainerWindow.SetActive(false);
        PlayerUI.SetActive(false);

        StartCoroutine(MoveNavBar(CurrentQuestBtn, "isCurrent"));
    }

    public void OnLessonContainerButtonClick()
    {
        LessonContainerWindow.SetActive(true);
        BagWindow.SetActive(false);
        ProfileWindow.SetActive(false);

        SettingsWindow.SetActive(false);
        PlayerUI.SetActive(false);

        StartCoroutine(MoveNavBar(LessonsBtn, "isLesson"));
    }

    public void OnSettingsButtonClick()
    {
        SettingsWindow.SetActive(true);
        BagWindow.SetActive(false);
        ProfileWindow.SetActive(false);
        CurrentLessonWindow.SetActive(false);
        LessonContainerWindow.SetActive(false);
        PlayerUI.SetActive(false);

        StartCoroutine(MoveNavBar(SettingsBtn, "isSetting"));
    }


    #region Navbar Function

    //this will move the navbar to the selected button
    private IEnumerator MoveNavBar(Button clickBtn, string btnState)
    {
        if (NavBarAnimationActivate)
        {

            BtnInteractable(false);

            RectTransform clickedRect = clickBtn.GetComponent<RectTransform>();

            navBarAnimator.SetTrigger(btnState);
            hideBtn(clickBtn, btnState);

            float targetX = clickedRect.anchoredPosition.x;

            Vector2 startPos = navbar_BG.anchoredPosition;
            Vector2 targetPos = new Vector2(targetX, navbar_BG.anchoredPosition.y);
            float elapsedTime = 0f;

            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / transitionDuration;

                Vector2 currentPos = navbar_BG.anchoredPosition;
                currentPos.x = Mathf.Lerp(startPos.x, targetPos.x, t);
                navbar_BG.anchoredPosition = currentPos;

                yield return null;
            }

            BtnInteractable(true);

            navbar_BG.anchoredPosition = targetPos;
        }
    }
    
    //This will be daynamic function to turn on and off the interactable of the button
    private void BtnInteractable(bool isInteractable)
    {
        profileBtn.interactable = isInteractable;
        CurrentQuestBtn.interactable = isInteractable;
        bagBtn.interactable = isInteractable;
        LessonsBtn.interactable = isInteractable;
        Settings.interactable = isInteractable;
    }


    //This will hide the button when selected and show when diselected
    private void hideBtn(Button btn, string currentState)
    {

        if (previouslySelectedBtn != null)
        {
            previouslySelectedBtn.gameObject.SetActive(true);
            navBarAnimator.ResetTrigger(previousState);
        }

        btn.gameObject.SetActive(false);


        previouslySelectedBtn = btn;
        previousState = currentState;   
    }

    #endregion
}
