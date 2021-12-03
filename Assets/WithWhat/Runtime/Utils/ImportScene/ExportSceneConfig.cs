using System.Collections.Generic;

namespace WithWhat.Utils.ImportScene
{
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

    public class ExportSceneConfigTransform
    {
        public string Position;
        public string Rotation;
        public string Scale;
    }

    public class ExportSceneConfigTrigger
    {
        public string TriggerName;
        public string Data;
    }

    public class ExportSceneConfigTriggerBox
    {
        public string Center;
        public string Size;
    }

    public class ExportSceneConfigTriggerSphere
    {
        public string Center;
        public float Radius;
    }

    public class ExportSceneConfigTriggerCapsule
    {
        public string Center;
        public float Radius;
        public float Height;
        public int Direction;
    }
}