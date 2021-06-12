using System;
using System.Reflection;
using System.Runtime.Versioning;
[assembly: AssemblyVersion("1.2.0.0")]
[assembly: TargetFramework("net5.0,Version=v5.0")]
class NetCoreProgram
{
    private static void Main(string[] args)
    {
        MyClass.PrintSth();
        Console.WriteLine("Hello I am NetCoreProgram");
        //MyILClass.PrintSth();
    }
}