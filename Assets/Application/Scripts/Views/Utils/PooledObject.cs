using UnityEngine;

namespace Application.Scripts.Views.Utils
{
    [CreateAssetMenu(fileName = "PooledObject", menuName = "PooledObjects/New")]
    public class PooledObject : ScriptableObject {

        public string Name;
        public GameObject ObjectToPool;
        public int Amount;
    }
}
