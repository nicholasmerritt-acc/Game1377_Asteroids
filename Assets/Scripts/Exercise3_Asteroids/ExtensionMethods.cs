using UnityEngine;

public static class ExtensionMethods
{
    /// <summary>
    /// Get the length of the currently playing clip
    /// </summary>
    /// <param name="animator"></param>
    /// <returns></returns>
    public static float GetCurrentClipLength(this Animator animator)
    {
        AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
        return clipInfo.clip.length;
    }

}