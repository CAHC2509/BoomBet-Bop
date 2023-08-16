using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for make the change between different objects (mainly UI), activating and deactivating them, playing a sound and shanking th screen
/// </summary>
public class EnableAndDisableManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource clicAudioSource;
    [SerializeField]
    private UIShake UIShake;
    [SerializeField]
    private List<GameObject> objectsToEnable;
    [SerializeField]
    private List<GameObject> objectsToDisable;

    /// <summary>
    /// Toggle the objects in the lists, play a sound and shake the screen
    /// </summary>
    public void MakeChange()
    {
        if (objectsToDisable.Count > 0)
            foreach (GameObject obj in objectsToDisable)
                obj.SetActive(false);

        if (objectsToEnable.Count > 0)
            foreach (GameObject obj in objectsToEnable)
                obj.SetActive(true);

        clicAudioSource.Play();
        UIShake.Shake();
    }
}
