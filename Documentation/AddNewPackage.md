# Adding a new package to Jotuns

## Manually create a new package

To create a new package manually, you'll have to follow a strict nomenclature.

First of all, go into JotunsEngine/Package and create a new folder corresponding to your package name.

Inside this folder, you'll need to respect this hierarchy

    PackageName
        Documentation
            README.md (Documentation about the package)
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

If you know that you'll not drive any testing on your package, you can remove the tests folder. Same goes for Editor. But for safety just create this hierarchy in order to upgrade easily the package in future needs.

For Assembly Definition files, respect the following convention :
    
    com.semicolon.jotuns.packagename
    com.semicolon.jotuns.packagename.editor
    com.semicolon.jotuns.packagename.tests
    com.semicolon.jotuns.packagename.tests.editor
    
Once you've done that, you can start adding content to your package into the correct folders depending on your needs.

## Using the editor extension from Jotuns

Jotuns comes with an editor extension in Core package, that's allow Jotuns's user to create a new package by only specifying the package name and some options.

Take care, even if the generation is made automatically, you need to specify into each Assembly Definition file which assembly definition reference needs to be added into the assembly file for example. The main obvious one is referencing the core package into the package runtime assembly created.

To access the generator : Window > Jotun > Core > Package Generator


