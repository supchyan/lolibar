@echo off

if "%1"=="/k" (
    echo Killing lolibar.exe...
    call :KillLolibar
    goto :eof
)
if "%1"=="/r" (
    echo Restarting lolibar.exe...
    call :KillLolibar
    start lolibar.lnk
    goto :eof
)
if "%1"=="/s" (
    echo Starting lolibar.exe...
    start lolibar.lnk
    goto :eof
)

echo lolibar cli usage:
echo /h      - Shows this message,
echo /k      - Terminates all lolibar.exe processes,
echo /r      - Restarts lolibar.exe process,
echo /s      - Starts lolibar.exe process
goto :eof

:KillLolibar
:loop
taskkill /im "lolibar.exe" >NUL 2>&1 && (
    goto :loop
) || (
    goto :eof
)