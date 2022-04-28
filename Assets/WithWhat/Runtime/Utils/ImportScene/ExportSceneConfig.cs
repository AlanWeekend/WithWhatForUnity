using System;
using System.Collections.Generic;

namespace WithWhat.Utils.ImportScene
{
    [Serializable]
    public class ExportSceneConfig
    {
        public string GameObjectName;
        public string PrefabPath;
        public bool ActiveSelf;
        public ExportSceneConfigTransform World;
        public ExportSceneConfigTransform Local;
        public List<ExportSceneConfigTrigger> Triggers;
        public List<ExportSceneConfig> Childs;
    }

    [Serializable]
    public class ExportSceneConfigTransform
    {
        public string Position;
        public string Rotation;
        public string Scale;
    }

    [Serializable]
    public class ExportSceneConfigTrigger
    {
        public string TriggerName;
        public string Data;
    }

    [Serializable]
    public class ExportSceneConfigTriggerBox
    {
        public string Center;
        public string Size;
    }

    [Serializable]
    public class ExportSceneConfigTriggerSphere
    {
        public string Center;
        public float Radius;
    }

    [Serializable]
    public class ExportSceneConfigTriggerCapsule
    {
        public string Center;
        public float Radius;
        public float Height;
        public int Direction;
    }
}