﻿using System;
using UnityEngine;

namespace Sakura
{
    public class SoundClip:MonoBehaviour
    {
        public bool loop = false;

        private AudioSource _source;
        private float _delay = 0.0f;
        private float _soundValue = 1.0f;
        private bool isLoaded = false;

        public float soundValue
        {
            set
            {
                if (_source != null)
                {
                    _soundValue = value;
                    _source.volume = value;
                }
            }
        }

        public void Stop()
        {
            _delay = 0.0f;
            if (_source)
            {
                _source.Stop();
            }

            if (loop == false)
            {
                recycle();
            }
        }

        public bool isPlaying
        {
            get
            {
                if (_source == null)
                {
                    return false;
                }
                return _source.isPlaying;
            }
        }

        public void Play()
        {
            if (_source.clip == null && isLoaded)
            {
                CallLater.Add(recycle,0.1f);
                return;
            }

            if (_source.clip == null)
            {
                return;
            }

            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }

            if (_source.isPlaying == false)
            {
                _source.loop = loop;
                _source.time = _delay;
                _source.Play();
            }

            if (loop == false)
            {
                if (_source.clip != null)
                {
                    CallLater.Add(recycle,_source.clip.length);
                }
                else
                {
                    recycle();
                }
            }
        }

        public float time
        {
            set
            {
                _delay = value;
                if (_source)
                {
                    _source.time = value;
                }
            }
            get
            {
                if (_source)
                {
                    return _source.time;
                }

                return _delay;
            }
        }

        public void load(string url)
        {
            isLoaded = false;

            AssetResource resource = null;
            if (AssetsManager.routerResourceDelegate != null)
            {
                resource = AssetsManager.routerResourceDelegate(url, name, "sound");
            }

            if (resource == null)
            {
                resource = AssetsManager.getResource(url, LoaderXDataType.PREFAB);
            }
            AssetsManager.bindEventHandle(resource,completeHandle);
            resource.load();
        }



        protected virtual void OnEnable()
        {
            _source = this.GetComponent<AudioSource>();
            _delay = 0.0f;
        }

        protected virtual void OnDisable()
        {
            Stop();
        }

        private void recycle(float d=0)
        {
            _delay = 0;
            this.simpleDispatch(SAEventX.COMPLETE);
        }

        private void completeHandle(SAEventX e)
        {
            isLoaded = true;
            AssetResource resource=e.target as AssetResource;
            AssetsManager.bindEventHandle(resource,completeHandle,false);
            if (e.type == SAEventX.FAILED)
            {
                return;
            }

            if (gameObject.activeSelf == false)
            {
                recycle();
                return;
            }

            AudioClip clip = resource.data as AudioClip;
            if (clip != null)
            {
                _source.clip = clip;
                _source.loop = loop;
                _source.volume = _soundValue;
                _source.Play();
                if (loop == false)
                {
                    CallLater.Add(recycle,_source.clip.length);
                }
            }
        }
    }
}