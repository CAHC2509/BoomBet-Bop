using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> tutorialScenes;
    [SerializeField]
    private GameObject leftArrow;
    [SerializeField]
    private GameObject rightArrow;

    private int currentSceneIndex = 0; // Index of the currently displayed tutorial scene

    private void Start()
    {
        UpdateArrowVisibility();
    }

    /// <summary>
    /// Display the next tutorial scene in the sequence if available.
    /// </summary>
    public void NextScene()
    {
        if (currentSceneIndex < tutorialScenes.Count - 1)
        {
            tutorialScenes[currentSceneIndex].SetActive(false);
            currentSceneIndex++;
            tutorialScenes[currentSceneIndex].SetActive(true);

            UpdateArrowVisibility();
        }
    }

    /// <summary>
    /// Display the previous tutorial scene in the sequence if available.
    /// </summary>
    public void PreviousScene()
    {
        if (currentSceneIndex > 0)
        {
            tutorialScenes[currentSceneIndex].SetActive(false);
            currentSceneIndex--;
            tutorialScenes[currentSceneIndex].SetActive(true);

            UpdateArrowVisibility();
        }
    }

    /// <summary>
    /// Update the visibility of navigation arrows based on the current scene index.
    /// </summary>
    private void UpdateArrowVisibility()
    {
        leftArrow.SetActive(currentSceneIndex > 0);
        rightArrow.SetActive(currentSceneIndex < tutorialScenes.Count - 1);
    }
}
