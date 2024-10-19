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
    private GameObject StoreWindow;

    [SerializeField]
    private GameObject HomeWindow;

    [SerializeField]
    private GameObject LessonWindow;

    [SerializeField]
    private GameObject BagWindow;

    [SerializeField]
    private GameObject SettingsWindow;

    [Header("Navbar")]
    [SerializeField]
    private RectTransform navbar_BG;

    [SerializeField]
    private bool NavBarAnimationActivate;
    private Button previouslySelectedBtn = null;
    private string previousState = null;
    public float transitionDuration = 0.5f;

    [Header("Navbar Button")]
    [SerializeField]
    private Button profileBtn;

    [SerializeField]
    private Button ShopBtn;

    [SerializeField]
    private Button HomeBtn;

    [SerializeField]
    private Button LessonsBtn;

    [SerializeField]
    private Button CurrentQuestBtn;

    [SerializeField]
    private Button bagBtn;

    [SerializeField]
    private Button SettingsBtn;

    [SerializeField]
    private GameObject subCircle;

    [SerializeField]
    private Sprite[] lessonsSprite;

    private string previousLessonState; // this where the state of the lesson will be stored

    [Header("Animator")]
    [SerializeField]
    private Animator navBarAnimator;

    [Header("Player stats UI")]
    [SerializeField]
    private GameObject PlayerUI;

    void Start()
    {
        OnHomeButtonClick(); // it will start at home window
    }

    public void OnBagButtonClick()
    {
        BagWindow.SetActive(true);
        PlayerUI.SetActive(true);

        StoreWindow.SetActive(false);
        LessonWindow.SetActive(false);
        ProfileWindow.SetActive(false);
        SettingsWindow.SetActive(false);

        StartCoroutine(MoveNavBar(bagBtn, "isBag"));
    }

    public void OnHomeButtonClick()
    {
        HomeWindow.SetActive(true);
        PlayerUI.SetActive(true);

        ProfileWindow.SetActive(false);
        StoreWindow.SetActive(false);
        LessonWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        BagWindow.SetActive(false);
        PlayerUI.SetActive(false);

        StartCoroutine(MoveNavBar(HomeBtn, "isHome"));
    }

    public void OnProfileButtonClick()
    {
        ProfileWindow.SetActive(true);

        HomeWindow.SetActive(false);
        StoreWindow.SetActive(false);
        LessonWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        BagWindow.SetActive(false);
        PlayerUI.SetActive(false);

        StartCoroutine(MoveNavBar(profileBtn, "isProfile"));
    }

    public void OnStoreButtonClick()
    {
        StoreWindow.SetActive(true);

        HomeWindow.SetActive(false);
        ProfileWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        LessonWindow.SetActive(false);
        BagWindow.SetActive(false);
        PlayerUI.SetActive(false);

        StartCoroutine(MoveNavBar(ShopBtn, "isShop"));
    }

    public void OnLessonClick() // this will show the current state of the Lesson
    {
        if (previousLessonState == null)
        {
            previousLessonState = "isLessonContainer";
            LessonState(previousLessonState);
        }
        else
            LessonState(previousLessonState);

        LessonWindow.SetActive(true);

        StartCoroutine(MoveNavBar(LessonsBtn, previousLessonState));
    }

    public void OnSubCircleClick()
    {
        Debug.Log("Button press on State: " + previousLessonState);

        if (previousLessonState == "isLessonContainer")
            LessonState("isCurrentLesson");
        else if (previousLessonState == "isCurrentLesson")
            LessonState("isLessonContainer");
    }

    #region Lesson State
    private void OnCurrentLesson()
    {
        LessonWindow.transform.GetChild(1).gameObject.SetActive(true); // This will show the Current lesson window
        LessonWindow.transform.GetChild(0).gameObject.SetActive(false); // This will hide the Lesson selection window

        ProfileWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        BagWindow.SetActive(false);
        HomeWindow.SetActive(false);
        PlayerUI.SetActive(false);
    }

    private void OnLessonContainer()
    {
        LessonWindow.transform.GetChild(0).gameObject.SetActive(true); // This will show the Lesson selection window
        LessonWindow.transform.GetChild(1).gameObject.SetActive(false); // This will hide the Current lesson window

        ProfileWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        BagWindow.SetActive(false);
        HomeWindow.SetActive(false);
        PlayerUI.SetActive(false);
    }

    #endregion

    public void OnSettingsButtonClick()
    {
        SettingsWindow.SetActive(true);
        HomeWindow.SetActive(false);
        ProfileWindow.SetActive(false);
        StoreWindow.SetActive(false);
        LessonWindow.SetActive(false);
        PlayerUI.SetActive(false);
    }

    #region Navbar Function

    //this will move the navbar to the selected button
    private IEnumerator MoveNavBar(Button clickBtn, string btnState)
    {
        if (NavBarAnimationActivate)
        {
            BtnInteractable(false);

            subCircle.gameObject.SetActive(clickBtn == LessonsBtn);

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
        ShopBtn.interactable = isInteractable;
        bagBtn.interactable = isInteractable;
        LessonsBtn.interactable = isInteractable;
        HomeBtn.interactable = isInteractable;
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

    //This will show the window of lesson if the last state was in the current Lesson or in the Lesson Selection
    private void LessonState(string currentLessonState)
    {
        navBarAnimator.SetTrigger(currentLessonState);

        switch (currentLessonState)
        {
            case "isCurrentLesson":
                OnCurrentLesson(); //Show the current Lesson Window
                LessonsBtn.GetComponent<Image>().sprite = lessonsSprite[0];
                LessonsBtn.GetComponent<Image>().SetNativeSize();
                previousLessonState = currentLessonState;
                return;

            case "isLessonContainer":
                OnLessonContainer(); // Show the Lesson Selection Window
                LessonsBtn.GetComponent<Image>().sprite = lessonsSprite[1];
                LessonsBtn.GetComponent<Image>().SetNativeSize();
                previousLessonState = currentLessonState;
                return;
        }
    }

    #endregion
}
