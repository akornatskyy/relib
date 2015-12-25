@echo off

CALL Variables.cmd

msbuild Reusable.Library.msbuild /t:Clean /p:BuildType=Release
msbuild Reusable.Library.msbuild /t:Clean /p:BuildType=Debug
pause