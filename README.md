# RepRap-Phone-Host
A Windows Phone application that allows for Bluetooth control of a RepRap 3D printer.

This application has the ability to open STL files, position them on a print bed, slice the files, render the generated GCode and feed the GCode to a printer. The application also allows for manual control of the printer.

The application is written in C# for the now defunct Windows Phone platform and makes use of the MonoGame framework for 3D rendering. It is not being actively maintained but the code has been released should anyone want to use the code for anything. It might be possible to build something similar for iOS and Android using Xamarin.

The application includes a custom slicer engine written in C#.
