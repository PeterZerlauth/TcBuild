# TcRelease
# A Twincat 3 Library Release and Build tool

- task: CmdLine@2
  displayName: Build Twincat 3 Library 
  condition: succeeded()
  inputs:
        failOnStderr: true
        script: '"C:\source\repos\TcRelease\TcRelease.exe" -c "Release" -v "$(System.DefaultWorkingDirectory)\Tc3_Machine.sln" -p Tc3_Machine -l Tc3_Machine -o "c:\source"'


![grafik](https://user-images.githubusercontent.com/48495545/219626574-c3afa70e-b82a-4424-8f51-657d1cbd2dc3.png)


<a href="https://www.buymeacoffee.com/9wjvwz24g6b"><img src="https://img.buymeacoffee.com/button-api/?text=Buy me a beer&emoji=🍺&slug=9wjvwz24g6b&button_colour=5F7FFF&font_colour=ffffff&font_family=Cookie&outline_colour=000000&coffee_colour=FFDD00" /></a>



Shield: [![CC BY-NC-SA 4.0][cc-by-nc-sa-shield]][cc-by-nc-sa]

This work is licensed under a
[Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License][cc-by-nc-sa].

[![CC BY-NC-SA 4.0][cc-by-nc-sa-image]][cc-by-nc-sa]

[cc-by-nc-sa]: http://creativecommons.org/licenses/by-nc-sa/4.0/
[cc-by-nc-sa-image]: https://licensebuttons.net/l/by-nc-sa/4.0/88x31.png
[cc-by-nc-sa-shield]: https://img.shields.io/badge/License-CC%20BY--NC--SA%204.0-lightgrey.svg
