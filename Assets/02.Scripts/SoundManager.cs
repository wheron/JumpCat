using UnityEngine;

namespace Cat
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource audioSource;

        public AudioClip introbgmClip;
        public AudioClip playBgmClip;

        public AudioClip jumpClip;
        public AudioClip colliderClip;

        public void SetBGMSound(string bgmName)
        {
            if (bgmName == "Intro")
                audioSource.clip = introbgmClip;
            else if (bgmName == "Play")
                audioSource.clip = playBgmClip;

            audioSource.loop = true;
            audioSource.volume = 0.1f;
            audioSource.Play();            
        }

        public void OnJumpSound()
        {
            audioSource.PlayOneShot(jumpClip, 0.5f);
        }
        
        public void OnColliderSound()
        {
            audioSource.PlayOneShot(colliderClip);
        }
    }
}
