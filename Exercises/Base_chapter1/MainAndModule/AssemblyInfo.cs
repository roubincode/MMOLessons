using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

[assembly: AssemblyVersion("2.2.0.0")]
[assembly: TargetFrameworkAttribute(".NETFramework,Version=v4.7")]
// 如3.0.6701.9其中
// 3为主版本号在最前面，后面一个0为副版本号，
// 再后面6701为编译生成号，最后面的9为修订号。
// 在设定Assembly的AssemblyVersion属性时，可是使用"*"来声明有编译器生成编译生成好和修订号，
// 例如2.3.*,则编译生成号为2000年1月1日起到编译日期止累计的天数，而修订号则是当天累计的秒数

// 标识着特定语言文件的程序集称为卫星程序集，
// 只是包含一些用于国际化的字符串、图片等等资源，而不包含任何代码。 
// 如果是被其它程序集引用的dll不应该输出为卫星程序集

// 卫星程序集：
//     创建资源文件：MyResource.cn.Resx 或者MyResource.cn.txt
//     使用命令resgen MyResource.cn.resx  MyResource.cn.resources 编译资源          
//     al.exe /culture:cn /out:"cn/HelloWorld.Resources.dll" 
//            /embed:"MyResources.cn.resources" /template:"HelloWorld.exe"          
// 在主程序集中如何访问卫星程序集：
    // System.Resources.ResourceManager resources =
    // new System.Resources.ResourceManager("HelloWorld.Resources.MyResources",
    // System.Reflection.Assembly.GetExecutingAssembly());

    // // Print out the "HelloWorld" resource string
    // Console.WriteLine(resources.GetString("HelloWorld"));
    
    // // Get the new culture name
    // Console.Write(resources.GetString("NextCulture")); 