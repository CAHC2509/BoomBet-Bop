using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplossionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject popUpObject;
    [SerializeField]
    private UIShake UIShake;
    [SerializeField]
    private AudioSource explossionAudio;
    [SerializeField]
    private float explossionShakeDuarion = 0.75f;
    [SerializeField]
    private float explossionShakeMagnitude = 50f;

    private float previsousShakeDuration;
    private float previousShakeMagnitude;

    public void StartExplossionEffect()
    {
        previsousShakeDuration = UIShake.shakeDuration;
        previousShakeMagnitude = UIShake.shakeMagnitude;

        UIShake.shakeDuration = explossionShakeDuarion;
        UIShake.shakeMagnitude = explossionShakeMagnitude;

        explossionAudio.Play();
        UIShake.Shake();
    }

    public void EndExplossionEffect()
    {
        UIShake.shakeDuration = previsousShakeDuration;
        UIShake.shakeMagnitude = previousShakeMagnitude;

        popUpObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
