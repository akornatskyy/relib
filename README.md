# Reusable Library

A collection of reusable abstractions for .NET application developer: caching, IoC, pagination, repository, application services, unit of work, background processing, exception trace policy, work item, etc.

Integration with Unity 2.0, EntLib 5.0, ASP.NET MVC 3.0, WatiN 2.0, Memcached 1.4.5.

Modular design for both .net 2.0 and .net 4.

# Development Environment 

* [IE11 on Windows 7](https://dev.windows.com/en-us/microsoft-edge/tools/vms/mac/)
* [Microsoft .NET Framework 4](http://www.microsoft.com/en-us/download/details.aspx?id=17718)
* [Windows SDK for Windows 7 and .NET Framework 4](https://www.microsoft.com/en-us/download/details.aspx?id=8279)
* [Microsoft .NET Framework 4.6](https://www.microsoft.com/en-us/download/details.aspx?id=48137)
* [Microsoft® SQL Server 2008 R2 SP2 - Express Edition](https://www.microsoft.com/en-us/download/details.aspx?id=30438)
* [Visual Studio .NET 2010 Express - Web Developer](https://go.microsoft.com/?linkid=9709969)
* [ASP.NET MVC 3 Tools](http://www.microsoft.com/en-us/download/details.aspx?id=1491)

# Tools and Fixes

There are the following tools used (see *Tools\\requirements*):

* MSBuild Community Tasks
* MSBuildSdkToolsPathRegistryFix.reg
* Microsoft Fxcop 10.0
* FxCopRegistryFix.reg
* Find two *NoWarn* tags with text *2008* and comment them out in file *C:\Windows\Microsoft.NET\Framework\v4.0.30319\Microsoft.CSharp.targets*.

