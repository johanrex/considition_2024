using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;

[assembly: Obfuscation(Feature = "encrypt resources", Exclude = false)]
[assembly: Obfuscation(Feature = "rename symbol names with printable characters", Exclude = false)]
[assembly: Obfuscation(Feature = "code control flow obfuscation", Exclude = false)]
[assembly: AssemblyCompany("LocalHost")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0")]
[assembly: AssemblyProduct("LocalHost")]
[assembly: AssemblyTitle("LocalHost")]
[assembly: ApplicationPart("Microsoft.AspNetCore.OpenApi")]
[assembly: ApplicationPart("Swashbuckle.AspNetCore.SwaggerGen")]
[assembly: AssemblyVersion("1.0.0.0")]
