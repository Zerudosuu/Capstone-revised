using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DG.Tweening;

public class ProfileSelection : MonoBehaviour
{
    public Image ImageDisplay;
    public TMP_InputField lrnTMPInputField;
    public TMP_InputField nameTMPInputField;
    public Animator anim;
    public GameObject PopUpError;
    public TMP_Text errorText;

    private const int LRN_LENGTH = 12; // Example: LRN must be 12 digits

    public float screenHeight;
    public float popUpInitialY;
    public float popUpVisibleY;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Calculate dynamic positions based on screen height
        screenHeight = Screen.height;

        popUpInitialY = screenHeight + 50; // Position offscreen (above the screen)
        popUpVisibleY = screenHeight / 3; // Visible position

        // Set the initial position of the popup error off-screen
        PopUpError.transform.localPosition = new Vector3(0, popUpInitialY, 0);
    }

    public async void OpenPickFileImage()
    {
        if (NativeFilePicker.IsFilePickerBusy())
            return;

        // Request permission to access files
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync();
        Debug.Log("Permission result: " + permission);

        if (permission == NativeFilePicker.Permission.Granted)
        {
            // Pick an image file (PNG or JPG only)
            string[] imageFileTypes = new string[] { "image/jpeg", "image/png", "image/jpg" };

            NativeFilePicker.PickFile((path) =>
            {
                if (path == null)
                {
                    Debug.Log("Operation cancelled");
                }
                else
                {
                    Debug.Log("Picked file: " + path);
                    LoadImageIntoUI(path);
                    DataManager.Instance.gameData.PicturePath = path;
                }
            }, imageFileTypes);
        }
        else
        {
            Debug.LogError("Permission denied. Cannot access files.");
        }
    }

    private void LoadImageIntoUI(string path)
    {
        byte[] imageBytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);

        if (texture.LoadImage(imageBytes)) // Automatically resizes texture
        {
            Sprite imageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            ImageDisplay.sprite = imageSprite;
        }
        else
        {
            Debug.LogError("Failed to load image from path: " + path);
        }
    }

    public void GetTMPInputData()
    {
        string lrn = lrnTMPInputField.text.Trim();
        string name = nameTMPInputField.text.Trim();

        // Validate LRN
        if (string.IsNullOrWhiteSpace(lrn))
        {
            ShowErrorPopup("LRN cannot be empty.");
            return;
        }

        if (!IsNumeric(lrn))
        {
            ShowErrorPopup("LRN must contain numbers only.");
            return;
        }

        if (lrn.Length != LRN_LENGTH)
        {
            ShowErrorPopup($"LRN must be {LRN_LENGTH} digits long.");
            return;
        }

        // Validate Name
        if (string.IsNullOrWhiteSpace(name))
        {
            ShowErrorPopup("Name cannot be empty.");
            return;
        }

        if (!IsValidName(name))
        {
            ShowErrorPopup("Name can only contain letters and spaces.");
            return;
        }

        // Validate Photo
        if (ImageDisplay.sprite == null)
        {
            ShowErrorPopup("Please select a profile photo.");
            return;
        }

        // If all validations pass, save the data
        DataManager.Instance.gameData.playerLRN = lrn;
        DataManager.Instance.gameData.playerName = name;
        PopUpError.SetActive(false);
        anim.SetTrigger("IsFinished");
    }

    private bool IsNumeric(string input)
    {
        return Regex.IsMatch(input, @"^\d+$"); // Check if input contains only digits
    }

    private bool IsValidName(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-Z\s]+$"); // Check if input contains only letters and spaces
    }

    private void ShowErrorPopup(string message)
    {
        errorText.text = message; // Set the error message
        PopUpError.SetActive(true);

        PopUpError.transform.DOLocalMoveY(popUpVisibleY, 0.5f).SetEase(Ease.OutBack) // Slide down
            .OnComplete(() =>
            {
                // Slide back up after 2 seconds
                DOVirtual.DelayedCall(2f,
                    () => { PopUpError.transform.DOLocalMoveY(popUpInitialY, 0.5f).SetEase(Ease.InBack); });
            });
    }
}