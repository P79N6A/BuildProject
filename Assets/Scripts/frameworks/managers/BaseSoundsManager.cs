using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sakura
{
    public class BaseSoundsManager : MonoDispatcher
    {
        private const string BGM = "bgm";
        protected float _soundValue = 1.0f;
        protected bool _musicEnable = true;
        protected float _musicValue;

        /// <summary>
        /// 同一时间不能出现两次声音
        /// </summary>
        protected HashSet<string> _soundsOnce=new HashSet<string>();

        private Dictionary<string,SoundClip> _soundsDictionary=new Dictionary<string, SoundClip>();
        private bool _enable = true;

        public float soundValue
        {
            get { return _soundValue; }
            set { _soundValue = value; }
        }

        public new bool enabled
        {
            get { return _enable; }
            set { _enable = value; }
        }

        public virtual void stopSound(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            SoundClip soundClip;
            if (_soundsDictionary.TryGetValue(name, out soundClip))
            {
                soundClip.Stop();
                _soundsOnce.Remove(name);
            }
        }

        public bool musicEnable
        {
            get { return _musicEnable; }
            set
            {
                _musicEnable = value;
                refresh();
            }
        }

        public float musicValue
        {
            get { return _musicValue; }
            set
            {
                _musicValue = value;
                refresh();
            }
        }

        public void playUISound(string name, bool isForce = false)
        {
            playSound(name,isForce,true);
        }

        public SoundClip playSound(string name, bool isForce=false, bool isUI=false)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            SoundClip soundClip = null;
            if (_soundsDictionary.TryGetValue(name, out soundClip))
            {
                soundClip.soundValue = soundValue;
                if (soundClip.isPlaying == false)
                {
                    soundClip.Play();
                }
                else if (isForce)
                {
                    soundClip.time = 0f;
                }
                return soundClip;
            }

            soundClip = getSoundInstance();
            _soundsDictionary.Add(name,soundClip);

            soundClip.soundValue = soundValue;
            soundClip.name = name;
            soundClip.addEventListener(SAEventX.COMPLETE, completeHandle);

            string url = getURL(name, isUI);
            soundClip.load(url);
            return soundClip;
        }

        public virtual string getURL(string uri, bool isUI = false)
        {
            string url = PathDefine.soundPath + "sound/" + uri + PathDefine.U3D;
            if (isUI)
            {
                url = PathDefine.uiPath + "ui/" + uri + PathDefine.U3D;
            }

            return url;
        }

        /**********************************************************************************/

        protected virtual SoundClip getSoundInstance()
        {
            GameObject soundItem=new GameObject("SoundItem");
            soundItem.AddComponent<AudioSource>();
            soundItem.transform.SetParent(AbstractApp.SoundContainer.transform,true);
            return soundItem.AddComponent<SoundClip>();
        }

        protected virtual void refresh()
        {
            SoundClip soundClip;
            if (_soundsDictionary.TryGetValue(BGM, out soundClip))
            {
                if (soundClip == null)
                {
                    _soundsDictionary.Remove(BGM);
                    return;
                }

                if (soundClip.isPlaying)
                {
                    soundClip.soundValue = _musicValue;
                }

                if (_musicEnable)
                {
                    soundClip.Play();
                }
            }
        }

        protected virtual void Start()
        {
            this.enabled = true;
            this.soundValue = 100;
        }

        protected void completeHandle(SAEventX e)
        {
            MonoDispatcher soundClip = (MonoDispatcher) e.target;
            string name = soundClip.name;
            _soundsOnce.Remove(name);
            soundClip.SetActive(false);
        }
    }
}