using Domains.Gameplay.Managers.Scripts;
using Domains.UI_Global.Events;
using MoreMountains.Tools;
using UnityEngine;

namespace Domains.Audio
{
    /// <summary>
    ///     Add this class to a GameObject to have it play a background music when instanciated.
    /// </summary>
    public class BackgroundMusic : MonoBehaviour, MMEventListener<AudioEvent>
    {
        public AudioClip[] SoundClips;


        /// whether or not the music should loop
        [Tooltip("whether or not the music should loop")]
        public bool Loop = true;

        /// the ID to create this background music with
        [Tooltip("the ID to create this background music with")]
        public int ID = 255;

        // Volume in range of 0-1
        [Tooltip("Volume of the background music")] [Range(0f, 1f)]
        public float volume = 1f;

        private AudioSource _audioSource;
        private int _currentClipIndex;

        private bool wasPaused;


        /// <summary>
        ///     Gets the AudioSource associated to that GameObject, and asks the GameManager to play it.
        /// </summary>
        protected virtual void Start()
        {
            // var options = MMSoundManagerPlayOptions.Default;
            // options.ID = ID;
            // options.Loop = Loop;
            // options.Location = Vector3.zero;
            // options.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Music;
            // options.Volume = volume;
            //
            // MMSoundManagerSoundPlayEvent.Trigger(SoundClip, options);
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = false;
            _audioSource.volume = volume;
            _audioSource.playOnAwake = false;

            if (SoundClips != null && SoundClips.Length > 0) PlayNextClip();
        }

        private void Update()
        {
            if (_audioSource == null) return;

            if (PauseManager.Instance != null && PauseManager.Instance.IsPaused())
            {
                wasPaused = true;
                return;
            }

            // Don't trigger next clip if the audio was just paused
            if (!_audioSource.isPlaying && !wasPaused && Loop && SoundClips.Length > 0) PlayNextClip();

            // Reset pause flag once playback resumes
            if (_audioSource.isPlaying) wasPaused = false;
        }

        private void OnEnable()
        {
            this.MMEventStartListening();
        }

        private void OnDisable()
        {
            this.MMEventStopListening();
        }

        public void OnMMEvent(AudioEvent eventType)
        {
            if (eventType.EventType == AudioEventType.ChangeVolume)
                SetVolume(eventType.Value);
            else if (eventType.EventType == AudioEventType.Mute)
                SetVolume(0f);
            else if (eventType.EventType == AudioEventType.Unmute) SetVolume(volume);
        }

        private void PlayNextClip()
        {
            _audioSource.clip = SoundClips[_currentClipIndex];
            _audioSource.Play();

            _currentClipIndex = (_currentClipIndex + 1) % SoundClips.Length;
        }


        public void SetVolume(float newVolume)
        {
            if (newVolume < 0f || newVolume > 1f)
            {
                UnityEngine.Debug.LogError("Volume must be between 0 and 1");
                return;
            }

            volume = newVolume;
        }
    }
}