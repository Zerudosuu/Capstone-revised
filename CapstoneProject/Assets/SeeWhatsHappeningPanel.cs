using UnityEngine;
using UnityEngine.UI;

public class SeeWhatsHappeningPanel : MonoBehaviour
{
    public GameObject containerPanel;
    public GameObject[] seeWhatsHappeningChildren;
    public int currentIndex;
    public Button buttonOpen;
    public bool isOpen;
    private Animator anim;

    private void OnEnable()
    {
        StepManager.OnStepBroadcasted += NextPanel; // Subscribe to event
    }

    private void OnDisable()
    {
        StepManager.OnStepBroadcasted -= NextPanel; // Unsubscribe when disabled
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        InitializedSeeWhatsHappeningPanel();
    }


    public void OpenPanel()
    {
        isOpen = !isOpen;
        anim.SetBool("isOpen", isOpen);
    }


    public void InitializedSeeWhatsHappeningPanel()
    {
        int childCount = containerPanel.transform.childCount;
        seeWhatsHappeningChildren = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            seeWhatsHappeningChildren[i] = containerPanel.transform.GetChild(i).gameObject;
            seeWhatsHappeningChildren[i].SetActive(false);
        }

        currentIndex = 0;
        ShowCurrentPanel();
    }

    public void ShowCurrentPanel()
    {
        foreach (GameObject panel in seeWhatsHappeningChildren)
        {
            panel.SetActive(false);
        }

        if (seeWhatsHappeningChildren.Length > 0 && currentIndex < seeWhatsHappeningChildren.Length)
        {
            seeWhatsHappeningChildren[currentIndex].SetActive(true);
        }
    }

    public void NextPanel()
    {
        seeWhatsHappeningChildren[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % seeWhatsHappeningChildren.Length;
        ShowCurrentPanel();
    }
}