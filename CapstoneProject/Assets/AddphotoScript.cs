using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;

public class AddphotoScript : MonoBehaviour
{
    // [SerializeField] private RawImage displayImage;
    //
    // public void OpenFileBrowser()
    // {
    //     FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".png", ".jpg", ".jpeg"));
    //     FileBrowser.SetDefaultFilter(".png");
    //     FileBrowser.ShowLoadDialog((path) => { StartCoroutine(LoadImage(path)); },
    //         null, false, null, "Select Image", "Load");
    // }
    //
    // private IEnumerator LoadImage(string filePath)
    // {
    //     byte[] imageData = File.ReadAllBytes(filePath);
    //     Texture2D texture = new Texture2D(2, 2);
    //     texture.LoadImage(imageData);
    //
    //     displayImage.texture = texture;
    //     displayImage.SetNativeSize();
    //
    //     yield return null;
    // }
}