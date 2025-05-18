@echo off

if "%1"=="/k" (
    echo Killing lolibar.exe...
    taskkill /IM lolibar.exe
    goto :eof
)
if "%1"=="/r" (
    echo Restarting lolibar.exe...
    taskkill /IM lolibar.exe
    start lolibar.lnk
    goto :eof
)
if "%1"=="/s" (
    echo Starting lolibar.exe...
    start lolibar.lnk
    goto :eof
) 
if not "%1"=="/k" if not "%1"=="/r" if not "%1"=="/s" (
    echo lolibar cli usage:
    echo /h      - Shows this message,
    echo /k      - Terminates all lolibar.exe processes,
    echo /r      - Restarts lolibar.exe process,
    echo /s      - Starts lolibar.exe process
    goto :eof
)