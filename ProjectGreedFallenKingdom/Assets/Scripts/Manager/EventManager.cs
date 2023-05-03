using System;

public static class EventManager
{
    // SCENE LOAD EVENTS
    // Before Scene Unload Load Screen Event
    public static event Action BeforeSceneUnloadLoadingScreenEvent;
    public static void CallBeforeSceneUnloadLoadingScreenEvent()
    {
        if (BeforeSceneUnloadLoadingScreenEvent != null)
        {
            BeforeSceneUnloadLoadingScreenEvent();
        }
    }

    // Before Scene Unload Event
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        if (BeforeSceneUnloadEvent != null)
        {
            BeforeSceneUnloadEvent();
        }
    }

    // After Scene Loaded Event
    public static event Action AfterSceneLoadEvent;
    public static void CallAfterSceneLoadEvent()
    {
        if (AfterSceneLoadEvent != null)
        {
            AfterSceneLoadEvent();
        }
    }

    // After Scene Loaded Load Screen Event
    public static event Action AfterSceneLoadedLoadingScreenEvent;
    public static void CallAfterSceneLoadedLoadingScreenEvent()
    {
        if (AfterSceneLoadedLoadingScreenEvent != null)
        {
            AfterSceneLoadedLoadingScreenEvent();
        }
    }
}
