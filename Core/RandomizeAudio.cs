using UnityEngine;

namespace Myth.Core
{
    public class RandomizeAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip[] audioClips;
        [SerializeField] private float minPitch = 0.9f;
        [SerializeField] private float maxPitch = 1.1f;
        
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomAudio()
        {
            int rng = Random.Range(0, audioClips.Length);

            _audioSource.pitch = Random.Range(minPitch, maxPitch);
            _audioSource.clip = audioClips[rng];
            _audioSource.Play();
        }
    }
}