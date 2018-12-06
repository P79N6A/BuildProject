using System;
using System.Collections;
using UnityEngine;

namespace Sakura
{
    public class PoolItem:MonoBehaviour
    {
        public AssetResource manager;

        public bool isNew = true;
        private bool isDisposed = false;

        public bool recycle(float recycleTime = 0)
        {
            if (Application.isPlaying == false || gameObject == null)
            {
                return false;
            }

            if (manager == null || AbstractApp.PoolContainer == null)
            {
                GameObject.Destroy(this.gameObject,recycleTime);
                return true;
            }

            gameObject.transform.SetParent(AbstractApp.PoolContainer.transform, true);
            gameObject.SetActive(false);
            if (recycleTime > 0)
            {
                StartCoroutine(laterRecycle(recycleTime));
            }
            else
            {
                manager.recycleToPool(this);
            }

            return true;
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            GameObject.Destroy(this.gameObject);
        }

        protected virtual void OnDestroy()
        {
            isDisposed = true;
        }

        private IEnumerator laterRecycle(float recycleTime)
        {
            if (recycleTime > 0)
            {
                yield return new WaitForSeconds(recycleTime);
            }

            manager.recycleToPool(this);
        }
    }
}