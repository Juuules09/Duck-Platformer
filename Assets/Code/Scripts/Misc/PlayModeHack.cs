using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

[InitializeOnLoad]
public static class PlayModeHack
{
    static PlayModeHack()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
        EditorApplication.quitting += Quitting;
    }

    private static void PlayModeStateChanged(PlayModeStateChange playModeStateChange)
    {
        switch (playModeStateChange)
        {
            case PlayModeStateChange.EnteredPlayMode:
                if (ShortcutManager.instance.GetAvailableProfileIds().Contains("Play"))
                {
                    ShortcutManager.instance.activeProfileId = "Play";
                }
                else
                {
                    ShortcutManager.instance.CreateProfile("Play");
                    ShortcutManager.instance.activeProfileId = "Play";

                    foreach (string shortcutId in ShortcutManager.instance.GetAvailableShortcutIds())
                    {
                        if (shortcutId != "Main Menu/Edit/Play")
                        {
                            ShortcutManager.instance.RebindShortcut(shortcutId, ShortcutBinding.empty);
                        }
                    }
                }

                new GameObject("PlayModeHack", typeof(PlayModeHackMonoBehaviour));
                break;

            case PlayModeStateChange.ExitingPlayMode:
                ShortcutManager.instance.activeProfileId = "Default";
                break;
        }
    }

    private static void Quitting()
    {
        ShortcutManager.instance.activeProfileId = "Default";
    }
}

public class PlayModeHackMonoBehaviour : MonoBehaviour
{
    private delegate int EnumThreadWndProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern int EnumThreadWindows(uint dwThreadId, EnumThreadWndProc lpfn, IntPtr lParam);
    [DllImport("kernel32.dll")]
    static extern uint GetCurrentThreadId();
    [DllImport("user32.dll")]
    static extern int GetClassNameA(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern IntPtr GetMenu(IntPtr hWnd);
    [DllImport("user32.dll")]
    private static extern int SetMenu(IntPtr hWnd, IntPtr hMenu);
    [DllImport("user32.dll")]
    private static extern int DrawMenuBar(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int GetWindowLongA(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")]
    private static extern int SetWindowLongA(IntPtr hWnd, int nIndex, int dwNewLong);

    private const int GWL_STYLE = -16;

    private const int WS_CAPTION = 0x00C00000;
    private const int WS_SYSMENU = 0x00080000;

    private IntPtr hFrameWnd;
    private IntPtr hFrameMenu;

    private void Awake()
    {
        int EnumThreadWndProc(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder className = new StringBuilder(64);
            GetClassNameA(hWnd, className, 64);
            if (className.ToString() == "UnityContainerWndClass")
            {
                hFrameWnd = hWnd;
                hFrameMenu = GetMenu(hWnd);
                return 0;
            }
            return 1;
        }

        EnumThreadWindows(GetCurrentThreadId(), EnumThreadWndProc, IntPtr.Zero);

        gameObject.hideFlags = HideFlags.HideInHierarchy;

        GameObject.DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Time.frameCount == 3)
        {
            SetWindowLongA(hFrameWnd, GWL_STYLE, GetWindowLongA(hFrameWnd, GWL_STYLE) & ~(WS_CAPTION | WS_SYSMENU));

            SetMenu(hFrameWnd, IntPtr.Zero);
            DrawMenuBar(hFrameWnd);
        }
    }

    private void OnDestroy()
    {
        SetWindowLongA(hFrameWnd, GWL_STYLE, GetWindowLongA(hFrameWnd, GWL_STYLE) | (WS_CAPTION | WS_SYSMENU));

        SetMenu(hFrameWnd, hFrameMenu);
        DrawMenuBar(hFrameWnd);
    }
}