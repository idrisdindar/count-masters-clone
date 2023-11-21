using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IdrisDindar.HyperCasual
{
    public class ObjectPool<T> where T : Component
    {
        private T prefab;
        private Transform parentTransform;
        private List<T> pool;

        private List<T> _reservedItems;

        public ObjectPool(T prefab, int initialSize, Transform parentTransform)
        {
            this.prefab = prefab;
            this.parentTransform = parentTransform;

            this.pool = new List<T>(initialSize);
            this._reservedItems = new List<T>();

            for (int i = 0; i < initialSize; i++)
            {
                T newObj = Object.Instantiate(prefab, parentTransform);
                newObj.gameObject.SetActive(false);
                pool.Add(newObj);
            }
        }

        public T Get()
        {
            if (pool.Count > 0)
            {
                T item = pool[0];

                _reservedItems.Add(item);

                pool.RemoveAt(0);

                return item;
            }

            T newObj = Object.Instantiate(prefab, parentTransform);
            _reservedItems.Add(newObj);
            return newObj;
        }

        public List<T> GetActiveObjects()
        {
            return _reservedItems.ToList();
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _reservedItems.Remove(obj);
            pool.Insert(0, obj);
            obj.transform.SetParent(parentTransform);
        }

        public void ReturnAll()
        {
            _reservedItems.ForEach(e =>
            {
                e.gameObject.SetActive(false);
                pool.Insert(0, e);
            });

            _reservedItems.Clear();
        }
    }
}