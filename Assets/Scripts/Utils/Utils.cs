
using Managers;
using PlayerComponents;
using UnityEngine;

public static class Utils
{
    public static void ActiveIfAdmin(GameObject adminSettings, MyPlayerController ownerPlayerController, string callerName)
    {
        // attiva le opzioni admin
        adminSettings.SetActive(false);

        bool isAdmin = GameManager.Instance.GameManagerServerGetIsAdmin(ownerPlayerController);
        Debug.Log($"In {callerName} is {ownerPlayerController.name} (Id: {ownerPlayerController.ObjectId}) admin? {isAdmin}");
        if (isAdmin)
        {
            adminSettings.SetActive(true);
        }
    }

    public static void PlaySound(AudioClip sound, AudioSource audioSource, float volume = 0.5f)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(sound, volume);
    }
}