using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    Animator loadingAnim;

    private void Awake()
    {
        loadingAnim = GetComponent<Animator>();
    }

    public IEnumerator loadingNextScene(string scene)
    {
        // Trigger the loading animation
        loadingAnim.SetTrigger("Load");

        // Wait for the duration of the "Load" animation
        yield return new WaitForSeconds(loadingAnim.runtimeAnimatorController.animationClips[1].length);

        // Load the next scene
        SceneManager.LoadSceneAsync(scene);
    }

}
