using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nextwin
{
    namespace Util
    {
        /// <summary>
        /// 오디오 관리자, 모든 오디오클립의 이름은 고유해야함
        /// </summary>
        public class AudioManager : Singleton<AudioManager>
        {
            private Dictionary<string, AudioClip> _audioClips;
            [SerializeField, Header("Key: AudioSource layer / Value: AudioSource object")]
            private SerializableDictionary<string, AudioSource> _audioSources;

            protected override void Awake()
            {
                base.Awake();
                LoadAudioClips();
            }

            private void Start()
            {
                CheckAudioSourcesAssinged();
            }

            /// <summary>
            /// 특정 오디오 소스를 통해 오디오 재생
            /// </summary>
            /// <param name="audioName">재생하려는 오디오 클립 이름</param>
            /// <param name="audioSourceKey">오디오 클립이 재생될 오디오소스 레이어 이름</param>
            public void PlayAudio(string audioName, string audioSourceKey)
            {
                AudioSource source = _audioSources[audioSourceKey];
                source.clip = _audioClips[audioName];
                source.Play();
            }

            /// <summary>
            /// 모든 오디오 일시정지
            /// </summary>
            public void PauseAll()
            {
                foreach(KeyValuePair<string, AudioSource> item in _audioSources)
                {
                    item.Value.Pause();
                }
            }

            /// <summary>
            /// 특정 오디오소스 레이어 일시정지
            /// </summary>
            /// <param name="audioSourceKey"></param>
            public void Pause(string audioSourceKey)
            {
                _audioSources[audioSourceKey].Pause();
            }

            /// <summary>
            /// 모든 오디오 재생
            /// </summary>
            public void ResumeAll()
            {
                foreach(KeyValuePair<string, AudioSource> item in _audioSources)
                {
                    item.Value.UnPause();
                }
            }

            /// <summary>
            /// 모든 오디오소스 레이어 재생
            /// </summary>
            /// <param name="audioSourceKey"></param>
            public void Resume(string audioSourceKey)
            {
                _audioSources[audioSourceKey].UnPause();
            }

            private void CheckAudioSourcesAssinged()
            {
                if(_audioSources.Count == 0)
                {
                    Debug.LogError("Assign AudioSource.");
                }

                foreach(KeyValuePair<string, AudioSource> item in _audioSources)
                {
                    if(item.Value == null)
                    {
                        Debug.LogError($"[AudioManager Error] Add AudioSource corresponding to {item.Key}.");
                    }
                }
            }

            private void LoadAudioClips()
            {
                AudioClip[] clips = Resources.LoadAll<AudioClip>("");
                foreach(AudioClip clip in clips)
                {
                    _audioClips.Add(clip.name, clip);
                }
            }
        }
    }
}
