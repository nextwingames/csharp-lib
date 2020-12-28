using Nextwin.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Nextwin.Client.Util
{
    /// <summary>
    /// 오디오 관리자, 모든 오디오클립의 이름은 고유해야함
    /// </summary>
    public abstract class AudioManagerBase<TEAudioClip, TEAudioSource> : Singleton<AudioManagerBase<TEAudioClip, TEAudioSource>>
    {
        protected Dictionary<TEAudioClip, AudioClip> _audioClips;
        [SerializeField, Header("Key: AudioSource layer / Value: AudioSource object")]
        protected SerializableDictionary<TEAudioSource, AudioSource> _audioSources;

        protected override void Awake()
        {
            base.Awake();
            LoadAudioClips();
        }

        protected virtual void Start()
        {
            CheckAudioSourcesAssinged();
        }

        /// <summary>
        /// 특정 오디오 소스를 통해 오디오 재생
        /// </summary>
        /// <param name="auidoClipName">재생하려는 오디오 클립 이름</param>
        /// <param name="audioSourceKey">오디오 클립이 재생될 오디오소스 레이어 이름</param>
        public virtual void PlayAudio(TEAudioClip auidoClipName, TEAudioSource audioSourceKey)
        {
            AudioSource source = _audioSources[audioSourceKey];
            source.clip = _audioClips[auidoClipName];
            source.Play();
        }

        /// <summary>
        /// 모든 오디오 일시정지
        /// </summary>
        public virtual void PauseAll()
        {
            foreach(KeyValuePair<TEAudioSource, AudioSource> item in _audioSources)
            {
                item.Value.Pause();
            }
        }

        /// <summary>
        /// 특정 오디오소스 레이어 일시정지
        /// </summary>
        /// <param name="audioSourceKey"></param>
        public virtual void Pause(TEAudioSource audioSourceKey)
        {
            _audioSources[audioSourceKey].Pause();
        }

        /// <summary>
        /// 모든 오디오 재생
        /// </summary>
        public virtual void ResumeAll()
        {
            foreach(KeyValuePair<TEAudioSource, AudioSource> item in _audioSources)
            {
                item.Value.UnPause();
            }
        }

        /// <summary>
        /// 모든 오디오소스 레이어 재생
        /// </summary>
        /// <param name="audioSourceKey"></param>
        public virtual void Resume(TEAudioSource audioSourceKey)
        {
            _audioSources[audioSourceKey].UnPause();
        }

        protected virtual void CheckAudioSourcesAssinged()
        {
            if(_audioSources.Count == 0)
            {
                Debug.LogError("Assign AudioSource.");
            }

            foreach(KeyValuePair<TEAudioSource, AudioSource> item in _audioSources)
            {
                if(item.Value == null)
                {
                    Debug.LogError($"[AudioManager Error] Add AudioSource corresponding to {item.Key}.");
                }
            }
        }

        protected virtual void LoadAudioClips()
        {
            AudioClip[] clips = Resources.LoadAll<AudioClip>("");
            foreach(AudioClip clip in clips)
            {
                _audioClips.Add(EnumConverter.ToEnum<TEAudioClip>(clip.name), clip);
            }
        }
    }
}
