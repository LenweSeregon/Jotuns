namespace Jotuns.Core.Editor.PackageGenerator
{
    using System;

    [Serializable]
    public class PackageGeneratorPackageJSONAuthor
    {
        public string name;
        public string email;

        public PackageGeneratorPackageJSONAuthor(string name, string email)
        {
            this.name = name;
            this.email = email;
        }
    }
    
    [Serializable]
    public class PackageGeneratorPackageJSON
    {
        public string name;
        public string displayName;
        public string version;
        public string description;
        public string unity;
        public PackageGeneratorPackageJSONAuthor author;
        public string type;

        public PackageGeneratorPackageJSON(
            string name, string displayName, string version, string description,
            string unity, string authorName, string authorEmail, string type)
        {
            this.name = name;
            this.displayName = displayName;
            this.version = version;
            this.description = description;
            this.unity = unity;
            this.author = new PackageGeneratorPackageJSONAuthor(authorName, authorEmail);
            this.type = type;
        }
    }
}