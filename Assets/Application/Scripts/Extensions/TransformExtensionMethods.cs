using UnityEngine;

namespace Application.Scripts.Extensions
{
    public static class TransformExtensionMethods {

        public static Transform[] GetChildren(this Transform parent)
        {
            Transform[] children = new Transform[parent.childCount];

            for (int i = 0; i < parent.childCount; i++)
            {
                children[i] = parent.GetChild(i);
            }

            return children;
        }
    }
}
