using System;
using System.Reflection;
using System.Runtime.CompilerServices;
[assembly: AssemblyVersion("1.2.0.0")]
class MainProgram
{
    static void Main()
    {
        MyClass.PrintSth();
        MyILClass.PrintSth();
    }
}