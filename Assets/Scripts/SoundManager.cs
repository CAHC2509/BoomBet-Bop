using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> audioSources;

    private string toggleName;
    private bool boolState;

    private Toggle toggle;

    private void Start()
    {
        toggleName = transform.name;

        toggle = GetComponent<Toggle>();

        boolState = PlayerPrefsUtility.GetBoolState(toggleName);

        toggle.isOn = boolState;
    }

    public void ToggleAudioSources(bool state)
    {
        boolState = state;

        foreach (AudioSource audioSource in audioSources)
            audioSource.mute = boolState;

        PlayerPrefsUtility.SetBoolState(toggleName, boolState);
    }
}
