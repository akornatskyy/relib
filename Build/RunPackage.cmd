@echo off

CALL Variables.cmd

msbuild Reusable.Library.msbuild /t:Package
pause