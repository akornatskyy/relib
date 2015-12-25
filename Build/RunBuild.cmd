@echo off

CALL Variables.cmd

msbuild Reusable.Library.msbuild /t:Build;Test;Package /p:CCNetNumericLabel=%Build% /p:BuildType=Release
pause