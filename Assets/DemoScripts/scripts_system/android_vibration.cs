
using UnityEngine;
using System;

public static class android_vibration
{
    public static void Vibrate(long milliseconds, int amplitude)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using var unityPlayer = new AndroidJavaClass(
                "com.unity3d.player.UnityPlayer");

            using var activity = unityPlayer.GetStatic<AndroidJavaObject>(
                "currentActivity");

            using var vibrator = activity.Call<AndroidJavaObject>(
                "getSystemService", "vibrator");

            if (vibrator == null)
                return;

            using var effectClass = new AndroidJavaClass(
                "android.os.VibrationEffect");

            using var effect = effectClass.CallStatic<AndroidJavaObject>(
                "createOneShot",
                milliseconds,
                Mathf.Clamp(amplitude, 1, 255));

            vibrator.Call("vibrate", effect);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Haptics failed: {e.Message}");
            Handheld.Vibrate();
        }
#endif
    }

    public static void Light() => Vibrate(35, 100);
    public static void Medium() => Vibrate(60, 180);
    public static void Heavy() => Vibrate(120, 255);
}
