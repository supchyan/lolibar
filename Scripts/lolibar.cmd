@echo off

if "%1"=="/h" (
    call :HelpMessage
    goto :eof
)
if "%1"=="/k" (
    echo Killing lolibar...
    taskkill /IM lolibar.exe
    goto :eof
)
if "%1"=="/r" (
    echo Restarting lolibar...
    taskkill /IM lolibar.exe
    start lolibar.lnk
    goto :eof
)
if "%1"=="" (
    echo Starting lolibar...
    start lolibar.lnk
    goto :eof
) 
if not "%1"=="/h" if not "%1"=="/k" if not "%1"=="/r" if not "%1"=="" (
    call :HelpMessage
    goto :eof
)

:HelpMessage
echo [LOLIBAR CLI USAGE]
echo /h     - Shows this message,
echo /k     - Terminates all lolibar processes,
echo /r     - Restarts lolibar process,
echo -----------------------------------------------------------
echo Calling [lolibar] with no flags starts lolibar.exe process.
goto :eof