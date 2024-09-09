@echo off
set TargetName=s3pe
set ConfigurationName=Release
set base=%TargetName%
rem -%ConfigurationName%
set src=%TargetName%-Source
set MAKENSIS=D:\Program Files (x86)\NSIS\makensis.exe

set out=S:\Sims3\Tools\builds\%TargetName%\
set helpFolder=%out%HelpFiles

set mydate=%date: =0%
set dd=%mydate:~0,2%
set mm=%mydate:~3,2%
set yy=%mydate:~8,2%
set mytime=%time: =0%
set h=%mytime:~0,2%
set m=%mytime:~3,2%
set s=%mytime:~6,2%
set suffix=%yy%-%mm%%dd%-%h%%m%

if EXIST "%MAKENSIS%" goto gotNSIS
echo "Could not find makensis."
goto noNSIS

:gotNSIS:
set nsisv=/V3

if x%ConfigurationName%==xRelease goto REL
set pdb=
goto noREL
:REL:
set pdb=-xr!*.pdb -xr!*.xml
:noREL:


rem there shouldn't be any to delete...
del /q /f %out%%TargetName%*%suffix%.*

pushd ..
7za a -r -t7z -mx9 -ms -xr!.?* -xr!*.suo -xr!zzOld -xr!bin -xr!obj -xr!Makefile -xr!*.Config -xr!"s3pe meshHelper*" "%out%%src%_%suffix%.7z" s3pe "s3pe Helpers"
popd

pushd bin\%ConfigurationName%
echo %suffix% >%TargetName%-Version.txt
attrib +r %TargetName%-Version.txt
del /f /q HelpFiles
xcopy "%helpFolder%\*" HelpFiles /s /i /y
7za a -r -t7z -mx9 -ms -xr!.?* -xr!*vshost* -xr!*.Config %pdb% "%out%%base%_%suffix%.7z" *
del /f %TargetName%-Version.txt
del /f /q HelpFiles
popd

7za x -o"%base%-%suffix%" "%out%%base%_%suffix%.7z"
pushd "%base%-%suffix%"
(
echo !cd %base%-%suffix%
for %%f in (*) do echo File /a %%f
pushd HelpFiles
echo SetOutPath $INSTDIR\HelpFiles
for %%f in (*) do echo File /a HelpFiles\%%f
echo SetOutPath $INSTDIR
popd
pushd Helpers
echo SetOutPath $INSTDIR\Helpers
for %%f in (*) do echo File /a Helpers\%%f
echo SetOutPath $INSTDIR
popd
dir /-c "..\%base%-%suffix%" | find " bytes" | for /f "tokens=3" %%f in ('find /v " free"') do @echo StrCpy $0 %%f
) > ..\INSTFILES.txt

(
for %%f in (*) do echo Delete $INSTDIR\%%f
pushd HelpFiles
for %%f in (*) do echo Delete $INSTDIR\HelpFiles\%%f
echo RmDir HelpFiles
popd
pushd Helpers
for %%f in (*) do echo Delete $INSTDIR\Helpers\%%f
echo RmDir Helpers
popd
) > UNINST.LOG
attrib +r +h UNINST.LOG
popd

"%MAKENSIS%" "/DINSTFILES=INSTFILES.txt" "/DUNINSTFILES=UNINST.LOG" "/DVSN=%suffix%" %nsisv% mknsis.nsi "/XOutFile %out%%base%_%suffix%.exe"

rmdir /s/q %base%-%suffix%
del INSTFILES.txt

:noNSIS:
pause
