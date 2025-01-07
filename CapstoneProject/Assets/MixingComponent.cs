using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MixingComponent : MonoBehaviour, IPointerClickHandler
{
    public Item item;

    [SerializeField]
    private Slider Slider; // Reference to the slider

    [SerializeField]
    private RectTransform ParentObject; // Parent containing color zones

    [SerializeField]
    private RectTransform RedZone; // Red zone object (Top)

    [SerializeField]
    private RectTransform GreenZone; // Green zone object (Middle)

    [SerializeField]
    private RectTransform YellowZone; // Yellow zone object (Bottom)

    [SerializeField]
    private float MinSpeed = 10f; // Minimum oscillation speed

    [SerializeField]
    private float MaxSpeed = 30f; // Maximum oscillation speed

    private bool IsMeasuring = false;
    private bool IsMovingUp = true; // Direction of slider movement
    private float CurrentSpeed; // Current oscillation speed

    public string getValue;

    public Image itemImage;

    ExperimentManager experimentManager;

    private void Start()
    {
        experimentManager = FindObjectOfType<ExperimentManager>();
        ResetMeasurement();
        RandomizeZoneWidths(); // Randomize zone widths on start
        RandomizeSpeed(); // Set initial random speed
    }

    public void SetItem(Item item)
    {
        this.item = item;
        itemImage.sprite = item.itemPrefab.GetComponent<Image>().sprite;
    }

    private void Update()
    {
        if (IsMeasuring)
        {
            // Oscillate the slider back and forth
            float movement = CurrentSpeed * Time.deltaTime * (IsMovingUp ? 1 : -1);
            Slider.value += movement / ParentObject.rect.width;

            // Reverse direction if the slider reaches the edges
            if (Slider.value >= 1f)
            {
                Slider.value = 1f;
                IsMovingUp = false;
                RandomizeSpeed(); // Change speed on direction reversal
            }
            else if (Slider.value <= 0f)
            {
                Slider.value = 0f;
                IsMovingUp = true;
                RandomizeSpeed(); // Change speed on direction reversal
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsMeasuring)
        {
            Debug.Log("Started measuring.");
            IsMeasuring = true; // Start moving when the button is clicked
        }
        else
        {
            Debug.Log($"Stopped measuring at value: {Slider.value}");
            IsMeasuring = false; // Stop moving when the button is clicked again
            CheckMeasurement(); // Check which color zone the slider landed in
        }
    }

    public void RandomizeZoneWidths()
    {
        float parentWidth = ParentObject.rect.width;

        // Generate random widths (percentage of parent width)
        float randomYellow = Random.Range(0.2f, 0.5f); // Yellow gets 20% to 50%
        float randomGreen = Random.Range(0.1f, 0.3f); // Green gets 10% to 30%
        float remainingForRed = 1f - (randomYellow + randomGreen); // Remaining percentage for Red

        // Normalize to make sure the sum is 100%
        float total = randomYellow + randomGreen + remainingForRed;
        randomYellow /= total;
        randomGreen /= total;
        remainingForRed /= total;

        // Apply the widths to each zone
        float yellowWidth = parentWidth * randomYellow;
        float greenWidth = parentWidth * randomGreen;
        float redWidth = parentWidth * remainingForRed;

        // Position and size the zones within the parent container
        YellowZone.anchorMin = new Vector2(0, 0);
        YellowZone.anchorMax = new Vector2(yellowWidth / parentWidth, 1);
        YellowZone.offsetMin = Vector2.zero;
        YellowZone.offsetMax = Vector2.zero;

        GreenZone.anchorMin = new Vector2(yellowWidth / parentWidth, 0);
        GreenZone.anchorMax = new Vector2((yellowWidth + greenWidth) / parentWidth, 1);
        GreenZone.offsetMin = Vector2.zero;
        GreenZone.offsetMax = Vector2.zero;

        RedZone.anchorMin = new Vector2((yellowWidth + greenWidth) / parentWidth, 0);
        RedZone.anchorMax = new Vector2(1, 1);
        RedZone.offsetMin = Vector2.zero;
        RedZone.offsetMax = Vector2.zero;

        Debug.Log(
            $"Random Widths - Yellow: {randomYellow * 100}%, Green: {randomGreen * 100}%, Red: {remainingForRed * 100}%"
        );
    }

    private void CheckMeasurement()
    {
        float sliderPositionX = Slider.value * ParentObject.rect.width;

        if (sliderPositionX <= YellowZone.rect.width)
        {
            Debug.Log("You landed in the YELLOW zone!");
            getValue = "Yellow";
        }
        else if (
            sliderPositionX > YellowZone.rect.width
            && sliderPositionX <= (YellowZone.rect.width + GreenZone.rect.width)
        )
        {
            Debug.Log("You landed in the GREEN zone!");
            getValue = "Green";
        }
        else
        {
            Debug.Log("You landed in the RED zone!");
            getValue = "Red";
        }

        GetComponentInParent<MeterPanelManager>().ShowMeterResult();
        experimentManager.UpdateScore(getValue);
        ResetMeasurement(); // Reset slider after checking
    }

    private void ResetMeasurement()
    {
        Slider.value = 0f;
        IsMeasuring = false;
        IsMovingUp = true;
    }

    private void RandomizeSpeed()
    {
        CurrentSpeed = Random.Range(MinSpeed, MaxSpeed);
        Debug.Log($"Randomized speed: {CurrentSpeed}");
    }

    public void StartMeasurement()
    {
        ResetMeasurement();
        RandomizeZoneWidths();
        RandomizeSpeed();
    }
}
