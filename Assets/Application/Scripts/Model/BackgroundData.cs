using UnityEngine;

namespace Application.Scripts.Model
{
    [System.Serializable]
    public class BackgroundData {

        public BackgroundType bgType;
        [Range(3, 20)]
        public int cantMin;
        [Range(3, 20)]
        public int cantMax;
    
    }
}
