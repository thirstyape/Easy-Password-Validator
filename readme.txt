This readme is specific to the NuGet deployment and can be ignored if using the source code.

As NuGet does not allow including files with package deployment the bad password lists will run over the web by default.
If this behavior is not desired they can be copied into your project in the following format.

Download https://raw.githubusercontent.com/thirstyape/Easy-Password-Validator/master/BadLists/top-10k-passwords.txt and copy to
\ProjectBuildDirectory\BadLists\top-10k-passwords.txt

Download https://raw.githubusercontent.com/thirstyape/Easy-Password-Validator/master/BadLists/top-100k-passwords.txt and copy to
\ProjectBuildDirectory\BadLists\top-100k-passwords.txt

As long as the output directory of your project has a BadLists\ subfolder containing the two files listed above they will run from the local copy.
