.assembly SecondClassAssembly {}
.assembly extern mscorlib {}
.class public SecondClass extends [mscorlib]system.object{
    .method public static void PrintSth() cil managed{
        ldstr "hello from IL"
        call void [mscorlib]System.Console::WriteLine(string)
        ret
    }
}