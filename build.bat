@echo off
echo Compiling VRMenuMod...
csc /target:library /out:VRMenuMod.dll /reference:Libraries\BepInEx.dll;Libraries\UnityEngine.dll VRMenuMod.cs
if %ERRORLEVEL% EQU 0 (
    echo Compile succeeded.
) else (
    echo Compile failed.
)
pause
