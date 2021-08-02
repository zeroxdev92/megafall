using System.Collections;
using UnityEngine;

namespace Application.Scripts.Extensions
{
    public static class AnimatorExtensionMethods {

        public static void SetTriggerOneFrame(this Animator anim, MonoBehaviour coroutineRunner, string trigger)
        {
            coroutineRunner.StartCoroutine(TriggerOneFrame(anim, trigger));
        }

        private static IEnumerator TriggerOneFrame(Animator anim, string trigger)
        {
            anim.SetTrigger(trigger);
            yield return null;
            if (anim != null)
            {
                anim.ResetTrigger(trigger);
            }
        }

        public static string GetCurrentAnimationName(this Animator anim)
        {
            var currAnimName = "";
            foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(clip.name))
                {
                    currAnimName = clip.name.ToString();
                }
            }

            return currAnimName;

        }
    }
}
