# MMORPG Lessons课程项目
这是ET5.0 MMORPG课程的教学项目，包含每章每节的前后端项目代码（前端不含场景，模型，动画等资源，会在课程中提供下载）  
    请注意：main分支是整体进度的项目代码  
           具体每章节项目代码对应的分支是：chapter1.1是一章1节开始，chapter1.19是一章1节完成

## 使用指南（指南是基于win，mac，linux下自行解决）
1.下载 vs code system版本，找到扩展面板，搜索C#扩展安装好扩展。  
2.安装.NetCore SDK，下载：.NetCore2.2  (建议2.2.300+，3.0以下版本，不能跨大版本。ETCore5.0 不支持netcore3)  
3.设置netsdk的windows环境变量，在用户变量中设置  
配置环境变量名： MSBuildSdksPath  
环境变量值：C:\Program Files\dotnet\sdk\2.2.300\Sdks （根据你自己的安装目录）  
4.安装 .NETFramework  
比如打开项目报错缺少 .NETFFramework4.7.1，就找到 下载页面选择 4.7.1  下载页面上的 Developer Pack  
5.指定项目使用的netcore运行时版本  
通过 global.json 文件，定义运行时使用的 .NET Core SDK 版本  
命令行到达项目的根目录：dotnet new globaljson --sdk-version 2.2.300，在你的项目中创建一个global.json 文件  
6.用vs code打开项目，根据提示完成一次"Restore"包还原操作。  
如果打开运行过项目，把你的解决方案中的所有项目中的obj目录全部删除，打开code重新Restore

### 前端运行  
1 客户端要求unity2019.4.1以上
### 后端运行  
1 用Visaul Studio 打开Server解决方案编译运行，或者命令行运行生成的App.dll  
2 需要单独编译Server/Hotfix/Server.Hotfix.csproj　(用命令行或用visaul studio单独编译都可以)  

### 网络游戏开发教学MMORPG课程案例教学
https://www.taikr.com/course/1076

## 交流与讨论：  
[肉饼学习交流网站：](http://www.taikr.com) http://www.taikr.com  
__讨论QQ群 : 695494071__  
__作者邮箱 : liaoxiangning@taikr.com

