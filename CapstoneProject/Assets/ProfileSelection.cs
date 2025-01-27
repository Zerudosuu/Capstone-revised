using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ProfileSelection : MonoBehaviour
{
    public Image ImageDisplay;
    public TMP_InputField lrnTMPInputField;
    public TMP_InputField nameTMPInputField;
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenPickFileImage()
    {
        if (NativeFilePicker.IsFilePickerBusy())
            return;

        // Pick an image file (PNG or JPG only)
        string[] imageFileTypes = new string[] { "jpg", "png" };

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.Log("Operation cancelled");
            }
            else
            {
                Debug.Log("Picked file: " + path);

                // Load the selected image as a Sprite and display it (if needed)
                LoadImageIntoUI(path);
                DataManager.Instance.gameData.PicturePath = path;
                // Save the path to the game data
            }
        }, imageFileTypes);
    }

    // Function to load the selected image and set it to a UI Image component
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
        if (!string.IsNullOrWhiteSpace(lrnTMPInputField.text) &&
            !string.IsNullOrWhiteSpace(nameTMPInputField.text))
        {
            string lrn = lrnTMPInputField.text.Trim(); // Trim to remove any accidental spaces
            string name = nameTMPInputField.text.Trim();

            DataManager.Instance.gameData.playerLRN = lrn;
            DataManager.Instance.gameData.playerName = name;

            anim.SetTrigger("IsFinished");
        }
        else
        {
            Debug.LogError("LRN and Name fields cannot be empty or whitespace.");
        }
    }
}