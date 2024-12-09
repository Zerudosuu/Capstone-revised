using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MixingComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
    private float FillRate = 20f; // Speed at which the slider fills

    [SerializeField]
    private float MaxFillValue = 100f; // Maximum slider value

    private bool IsMeasuring = false;
    private float MeasuredValue = 0f;

    public string getValue;

    public Image itemImage;

    ExperimentManager experimentManager;

    private void Start()
    {
        experimentManager = FindObjectOfType<ExperimentManager>();
        ResetMeasurement();
        RandomizeZoneWidths(); // Randomize zone widths on start
    }

    public void SetItem(Item item)
    {
        this.item = item;
        itemImage.sprite = item.itemPrefab.GetComponent<Image>().sprite;
    }

    public void StartMeasurement()
    {
        ResetMeasurement();
        RandomizeZoneWidths();
    }

    private void Update()
    {
        if (IsMeasuring)
        {
            // Increment the slider value based on FillRate
            MeasuredValue += FillRate * Time.deltaTime;

            // Clamp the measured value to stay within the slider range (0 to 100)
            MeasuredValue = Mathf.Clamp(MeasuredValue, 0f, MaxFillValue);

            // Update the slider value (normalized between 0 and 1)
            Slider.value = MeasuredValue / MaxFillValue;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsMeasuring = true; // Start filling when button is pressed
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsMeasuring = false; // Stop filling when button is released
        CheckMeasurement(); // Check which color zone the slider landed in
    }

    public void RandomizeZoneWidths()
    {
        float parentWidth = ParentObject.rect.width;

        // Generate random widths (percentage of parent width)
        float randomYellow = Random.Range(0.2f, 0.5f); // Yellow gets 20% to 50%
        float randomGreen = Random.Range(0.1f, 0.3f); // Green gets 20% to 30%
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
        float sliderPositionX = MeasuredValue / MaxFillValue * ParentObject.rect.width;

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
        MeasuredValue = 0f;
        Slider.value = 0f;
    }
}
