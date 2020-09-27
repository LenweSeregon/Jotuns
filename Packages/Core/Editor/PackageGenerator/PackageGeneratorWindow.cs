namespace Jotuns.Core.Editor.PackageGenerator
{
    using System.IO;
    using UnityEngine;
    using UnityEditor;

    public class PackageGeneratorWindow : EditorWindow
    {
        //////////////////////////////////////////
        ///// Constants
        //////////////////////////////////////////

        #region Constants
        private readonly string WINDOW_NAME = "Jotuns - Package generator";
        private readonly string ERROR_PACKAGE_NAME_EMPTY = "Package name cannot be empty";
        private readonly string ERROR_PACKAGE_AUTHOR_EMPTY = "Package Author cannot be empty";
        private readonly string ERROR_PACKAGE_NOT_LOCAL = "Package is coming from a git URL. Need to be imported locally in order to " +
                                                          "modify it and create a new package";
        private readonly string ERROR_PACKAGE_ALREADY_EXISTS = "Package with the same name already exists";

        private readonly string COMPANY = "com.semicolon.jotuns";
        private readonly string PACKAGE_PATH = "Packages/" + "com.semicolon.jotuns";
        private readonly string PACKAGE_DOCUMENTATION_FOLDER_NAME = "Documentation";
        private readonly string PACKAGE_RUNTIME_FOLDER_NAME = "Runtime";
        private readonly string PACKAGE_EDITOR_FOLDER_NAME = "Editor";
        private readonly string PACKAGE_TESTS_FOLDER_NAME = "Tests";
        private readonly string PACKAGE_TESTS_RUNTIME_FOLDER_NAME = "Runtime";
        private readonly string PACKAGE_TESTS_EDITOR_FOLDER_NAME = "Editor";

        #endregion
        
        private string _packageName;
        private string _packageAuthor;
        private bool _generateTests;
        private bool _generateTestsEditor;
        private bool _generateTestsEditorRuntime;

        private bool _generationDone = false;
        private bool _generatorError = false;
        private string _generationDoneMessage;

        [MenuItem ("Window/Jotuns/Core/Package Generator")]
        public static void  ShowWindow () 
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(PackageGeneratorWindow));
            window.titleContent = new GUIContent("Jotuns - Package generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Package generation", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            GUILayout.Label("General settings", EditorStyles.boldLabel);
            _packageName = EditorGUILayout.TextField("Package name", _packageName);
            _packageAuthor = EditorGUILayout.TextField("Package Author", _packageAuthor);
            EditorGUILayout.Space();
            
            GUILayout.Label("Generation settings", EditorStyles.boldLabel);
            _generateTests = EditorGUILayout.Toggle("Generate tests", _generateTests);
            if (_generateTests)
            {
                _generateTestsEditorRuntime = EditorGUILayout.Toggle("Generate tests runtime", _generateTestsEditorRuntime);
                _generateTestsEditor = EditorGUILayout.Toggle("Generate tests editor", _generateTestsEditor);
            }
            
            EditorGUILayout.Space();
            if (_generationDone)
            {
                Color saveColor = GUI.contentColor;
                GUI.contentColor = _generatorError ? Color.red : Color.green;
                GUILayout.Label(_generationDoneMessage, EditorStyles.boldLabel);
                GUI.contentColor = saveColor;
            }
            EditorGUILayout.Space();
            
            if(GUILayout.Button("Generate"))
            {
                if (string.IsNullOrEmpty(_packageName))
                {
                    _generationDone = true;
                    _generatorError = true;
                    _generationDoneMessage = ERROR_PACKAGE_NAME_EMPTY;
                    return;
                }

                if (string.IsNullOrEmpty(_packageAuthor))
                {
                    _generationDone = true;
                    _generatorError = true;
                    _generationDoneMessage = ERROR_PACKAGE_AUTHOR_EMPTY;
                    return;
                }

                string lowerCasePackage = _packageName.ToLower();
                string pathPackage = Path.GetFullPath(PACKAGE_PATH);
                string pathNewPackage = Path.Combine(pathPackage, "Packages", _packageName);

                if (Directory.Exists(pathPackage) == false)
                {
                    _generationDone = true;
                    _generatorError = true;
                    _generationDoneMessage = ERROR_PACKAGE_NOT_LOCAL;
                    return;
                }
                
                if (Directory.Exists(pathNewPackage))
                {
                    _generationDone = true;
                    _generatorError = true;
                    _generationDoneMessage = ERROR_PACKAGE_ALREADY_EXISTS;
                    return;
                }
                
                // Create main directory folder
                Directory.CreateDirectory(pathNewPackage);
                
                // Create package.json file
                CreatePackageJsonFile(pathNewPackage, lowerCasePackage);
                
                // Create Documentation folder
                string pathDocumentation = Path.Combine(pathNewPackage, PACKAGE_DOCUMENTATION_FOLDER_NAME);
                Directory.CreateDirectory(pathDocumentation);

                // Create runtime folder
                string pathRuntime = Path.Combine(pathNewPackage, PACKAGE_RUNTIME_FOLDER_NAME);
                Directory.CreateDirectory(pathRuntime);
                // Create runtime assembly definition
                CreateAssemblyDefinitionAt(pathRuntime, COMPANY + "." + lowerCasePackage, false);

                // Create editor folder
                string pathEditor = Path.Combine(pathNewPackage, PACKAGE_EDITOR_FOLDER_NAME);
                Directory.CreateDirectory(pathEditor);
                // Create editor assembly definition
                CreateAssemblyDefinitionAt(pathEditor, COMPANY + "." + lowerCasePackage + ".editor", true);
                
                // Create tests folder
                if (_generateTests)
                {
                    string pathTests = Path.Combine(pathNewPackage, PACKAGE_TESTS_FOLDER_NAME);
                    Directory.CreateDirectory(pathTests);

                    if (_generateTestsEditor)
                    {
                        // Create Tests editor folder
                        string pathTestEditor = Path.Combine(pathTests, PACKAGE_TESTS_EDITOR_FOLDER_NAME);
                        Directory.CreateDirectory(pathTestEditor);
                        // Create tests editor assembly definition
                        CreateAssemblyDefinitionAt(pathTestEditor, COMPANY + "." + lowerCasePackage + ".tests.editor", true);
                    }

                    if (_generateTestsEditorRuntime)
                    {
                        // Create Tests runtime folder
                        string pathTestRuntime = Path.Combine(pathTests, PACKAGE_TESTS_RUNTIME_FOLDER_NAME);
                        Directory.CreateDirectory(pathTestRuntime);
                        // Create tests runtime assembly definition
                        CreateAssemblyDefinitionAt(pathTestRuntime, COMPANY + "." + lowerCasePackage + ".tests", true);
                    }
                }

                AssetDatabase.Refresh();
            }
        }

        private void CreatePackageJsonFile(string pathRoot, string assemblyName)
        {
            string name = assemblyName;
            string displayName = _packageName;
            string version = "0.0.1";
            string description = _packageName;
            string unity = Application.unityVersion;
            string authorName = _packageAuthor;
            string authorEmail = "";
            string type = "tool";
            PackageGeneratorPackageJSON package = new PackageGeneratorPackageJSON(name, displayName, version,
                description, unity, authorName, authorEmail, type);
            string packageToJson = JsonUtility.ToJson(package, true);
            
            string filepath = Path.Combine(pathRoot, "package.json");
            using (FileStream fs = new FileStream(filepath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(packageToJson);
                }
            }
        }

        private void CreateAssemblyDefinitionAt(string path, string assemblyName, bool forEditor)
        {
            string name = assemblyName;
            string[] references = new string[] { };
            string[] includePlatforms = null;
            string[] excludePlatforms = null;
            bool allowUnsafeCode = false;
            bool overrideReferences = false;
            string[] precompiledReferences = new string[] { };
            bool autoReferenced = true;
            string[] defineConstraints = new string[] { };
            string[] versionDefines = new string[] { };
            bool noEngineReferences = false;

            if (forEditor)
            {
                includePlatforms = new string[] {"Editor"};
                excludePlatforms = new string[] { };
            }
            else
            {
                includePlatforms = new string[] { };
                excludePlatforms = new string[]
                {
                    "Android", "Editor", "iOS", "LinuxStandalone64", "Lumin", "macOSStandalone", "PS4", "Stadia",
                    "Switch", "tvOS", "WSA", "WebGL", "WindowsStandalone32", "WindowsStandalone64", "XboxOne"
                };
            }

            PackageGeneratorAssemblyJSON assembly = new PackageGeneratorAssemblyJSON(name, references,
                includePlatforms, excludePlatforms, allowUnsafeCode, overrideReferences, precompiledReferences,
                autoReferenced, defineConstraints, versionDefines, noEngineReferences);
            string assemblyToJson = JsonUtility.ToJson(assembly, true);
            

            string filepath = Path.Combine(path, assemblyName + ".asmdef");
            using (FileStream fs = new FileStream(filepath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(assemblyToJson);
                }
            }
        }
    }
}

