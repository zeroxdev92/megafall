using System.Collections.Generic;
using System.Linq;
using Application.Scripts.Model;
using Application.Scripts.Views.Gameplay;
using UnityEngine;

namespace Application.Scripts.Views.Utils
{
    public class ObjectPooler : MonoBehaviour
    {

        public static ObjectPooler instance;

        public List<PooledObject> pooledObjects;

        private List<GameObject> pool;

        void Awake()
        {
            instance = this;

            pool = new List<GameObject>();

            for (int i = 0; i < pooledObjects.Count; i++)
            {
                for (int z = 0; z < pooledObjects[i].Amount; z++)
                {
                    GameObject obj = Instantiate(pooledObjects[i].ObjectToPool);
                    obj.name = pooledObjects[i].Name;
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);
                    pool.Add(obj);
                }
            }
        }


        public GameObject GetPooledObject(string _name, bool active = false)
        {
            GameObject[] oList = pool.Where(x => x.name == _name).ToArray();

            for (int i = 0; i < oList.Length; i++)
            {
                if (!oList[i].activeInHierarchy)
                {
                    oList[i].SetActive(active);
                    return oList[i];
                }
            }

            PooledObject pooledObject = pooledObjects.FirstOrDefault(x => x.Name == _name);

            if (pooledObject == null)
            {
                Debug.LogError("Object with name " + _name + " not found");
                return null;
            }

            GameObject obj = Instantiate(pooledObject.ObjectToPool);
            obj.name = _name;
            obj.transform.SetParent(transform);
            obj.SetActive(active);
            pool.Add(obj);

            return obj;
        }

        public bool DisableGameObjects(string _name)
        {
            GameObject[] oList = pool.Where(x => x.name == _name).ToArray();
            bool anyDisabled = false;

            for (int i = 0; i < oList.Length; i++)
            {
                if (oList[i].activeInHierarchy)
                {
                    if (_name.Equals(Constants.PooledObjects.OBSTACLE_COINS))
                    {

                        oList[i].GetComponent<CoinsWithObstacle>().ReturnToPoolPreventSpawnTrigger();
                        anyDisabled = true;
                    }
                    else
                    {
                        oList[i].SetActive(false);
                        anyDisabled = true;
                    }
                }
            }

            return anyDisabled;
        }
    }
}
