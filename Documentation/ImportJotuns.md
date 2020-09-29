# Importing Jotuns

Before diving into the importation process of Jotuns into Unity. There is some point to clarify about Jotuns and the various way that Unity gives to import a package via Package Manager.

### Understanding Jotuns hierarchy
Jotuns is organised in two layers.

The main repository is quite empty by itself, it contains :
* package.json in order to be allow to be considered as a package which is the main package
* Documentation folder which regroup all informations about the library
* Packages folder which regroup actual features package.
    * This package folder always contains at least the Core package containing common functions for sub-packages.  

The Package folder as stated above contains various folder, each representing a functionality for example
   
    Jotuns
        Documentation
        Packages
            Core
            Feature1
            Feature2
            Feature3
        package.json
        
Each package folder follow the same strict hierarchy architecture in order to be nicely considered by Unity as a Package and standardized into Jotuns.
The hierarchy is as follow :

    PackageName
        Documentation
            README.md
        Editor
            Assembly Definition for Editor
        Runtime
            Assembly Definition for Runtime
        Tests
            Editor
                Assembly Definition for Editor tests
            Runtime
                Assembly Definition for Runtime tests
        package.json

It is possible that a package doesn't contains some folder if they doesn't use it like Tests folder. About Assembly Definition they also follow a strict naming convention again in order to stay consistent into Jotuns. 

    com.semicolon.jotuns.packagename
    com.semicolon.jotuns.packagename.editor
    com.semicolon.jotuns.packagename.tests
    com.semicolon.jotuns.packagename.tests.editor
    
This notion of "empty" main package containing all actual package of Jotuns is important because it means that you'll be able to import only some package if you know for example that you'll not use the package X. 

Indeed, we'll see below that it'll be possible to either impor the whole Jotuns package, or only some modules of it.

NOTA BENE : if you decide to import only some packages, you MUST also import at least the Core package. Simply because Jotuns's developers assumes that they can use Core package to developer common features for all packages.
    

### Importing via URL
In Package Manager window, click on "+" button and "Add package from git URL...".

There is multiple way to import from an URL by specifying argument, there is a list of the possibilities
* To get the full master version of Jotuns : https://github.com/LenweSeregon/Jotuns.git
* To get the full branch version of Jotuns : https://github.com/LenweSeregon/Jotuns.git#branch
* To get the full specific version of Jotuns : https://github.com/LenweSeregon/Jotuns.git#X.Y.Z
* To get a specific Jotuns's package from master : https://github.com/LenweSeregon/Jotuns.git?path=/subfolderPath
* To get a specific Jotuns's package from branch : https://github.com/LenweSeregon/Jotuns.git?path=/subfolderPath#branch
* To get a specific Jotuns's package from version : https://github.com/LenweSeregon/Jotuns.git?path=/subfolderPath#X.Y.Z

You must keep in mind that when you import either full version or sub-package of Jotuns from a git URL, the import is mark as read only. It's means that you cannot modify this package, you are only a simple user. In order to modify Jotuns, check below section "Importing via Local disk".

### Importing via Local disk
In Package Manager window , click on "+" button and "Add package from disk..."
You'll need to select a package.json that is the representation of a package in Unity.

Like importing via URL, when importating from disk you'll have the possibly to select either the full library or some modules. (select the package.json of the main directory, or package.json of sub-package located in the package)

When you import from local disk, it means that you have previously clone the project on your computer and that you have a copy of it. You can modify Jotuns directly and push your changes from Jotuns directory.

You must keep in mind that in order to use the Package Generator tool in Core package, you must import the full version of Jotuns. Simply because it is that version that contains the folder with the multiple sub-packages.
