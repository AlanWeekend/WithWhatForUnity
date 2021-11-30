using System.Collections.Generic;

namespace WithWhat.Editor
{
    public class EditorSceneConfig
    {
        public string DeviceCode;
        public string PrefabPath;
        public EditorSceneConfigTransform World;
        public EditorSceneConfigTransform Local;
        public List<EditorSceneConfigTrigger> Triggers;
        public List<EditorSceneConfig> Childs;
    }

    public class EditorSceneConfigTransform
    {
        public string Position;
        public string Rotation;
        public string Scale;
    }

    public class EditorSceneConfigTrigger
    {
        public string TriggerName;
        public string Data;
    }

    public class EditorSceneConfigTriggerBox
    {
        public string Center;
        public string Size;
    }

    public class EditorSceneConfigTriggerSphere
    {
        public string Center;
        public float Radius;
    }

    public class EditorSceneConfigTriggerCapsule
    {
        public string Center;
        public float Radius;
        public float Height;
        public int Direction;
    }
}