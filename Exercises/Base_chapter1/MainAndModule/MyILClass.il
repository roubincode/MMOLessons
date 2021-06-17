.assembly extern mscorlib {
    .ver 4:0:0:0
}
.assembly MyAssembly
{
    .ver 1:0:1:0
}
//MyILClass类
.class public MyILClass extends [mscorlib]System.Object{
    //PrintSth方法
    .method public static void PrintSth() cil managed{
        ldstr "hello from IL"
        call void [mscorlib]System.Console::WriteLine(string)
        ret
    }
}