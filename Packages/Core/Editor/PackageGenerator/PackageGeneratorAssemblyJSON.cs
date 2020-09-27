namespace Jotuns.Core.Editor.PackageGenerator
{
    using System;

    [Serializable]
    public class PackageGeneratorAssemblyJSON
    {
        public string name;
        public string[] references;
        public string[] includePlatforms;
        public string[] excludePlatforms;
        public bool allowUnsafeCode;
        public bool overrideReferences;
        public string[] precompiledReferences;
        public bool autoReferenced;
        public string[] defineConstraints;
        public string[] versionDefines;
        public bool noEngineReferences;

        public PackageGeneratorAssemblyJSON(
            string name, string[] references, string[] includes, string[] excludes, bool allowUnsafe, bool overrideReferences,
            string[] precompiledRef, bool autoReferenced, string[] defineConstraints, string[] versionDefines, bool noEngineRef)
        {
            this.name = name;
            this.references = references;
            this.includePlatforms = includes;
            this.excludePlatforms = excludes;
            this.allowUnsafeCode = allowUnsafe;
            this.overrideReferences = overrideReferences;
            this.precompiledReferences = precompiledRef;
            this.autoReferenced = autoReferenced;
            this.defineConstraints = defineConstraints;
            this.versionDefines = versionDefines;
            this.noEngineReferences = noEngineRef;
        }
    }
}