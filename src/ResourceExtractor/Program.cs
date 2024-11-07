// See https://aka.ms/new-console-template for more information
using System.Reflection;
using System.Runtime.InteropServices;

void WriteResourceToString(Assembly assembly, string resourceName, string filename)
{
    using (var stream = assembly.GetManifestResourceStream(resourceName))
    {
        if (stream == null)
            throw new ArgumentException("Resource not found", nameof(resourceName));

        using (var fileStream = File.Create(filename))
        {
            stream.CopyTo(fileStream);
        }
    }
}

var assemblyFilename = "C:/temp/app/LocalHost.dll";
var outputDirectory = "C:/temp/assemblyresources/";

var assembly = Assembly.LoadFile(assemblyFilename);
string assemblyName = assembly.GetName().Name;

var resources = assembly.GetManifestResourceNames();

foreach (var resourceName in resources)
{
    string resourceFilename = outputDirectory + resourceName;
    Console.WriteLine($"Writing {resourceName} to {resourceFilename}");
    WriteResourceToString(assembly, resourceName, resourceFilename);
    Console.WriteLine("Done.");
}

//string[] cities = new string[]
//{
//      "gothenburg",
//      "nottingham"
//};
//foreach (var city in cities)
//{
//    string resourceName;

//    resourceName = assemblyName + ".Config." + city + ".awards.json";
//    WriteResourceToString(assembly, resourceName, outputDirectory + resourceName);

//    resourceName = assemblyName + ".Config." + city + ".personalities.json";
//    WriteResourceToString(assembly, resourceName, outputDirectory + resourceName);
//}

Console.WriteLine("Done");


