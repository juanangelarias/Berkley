ECHO OFF
SET server="%1"
SET DestinationPath=\James\
SET DestinationSubFolder=JamesWebUI.Server
SET noPath=^.
SET NetVersion=NET8.0
:: Allow shortcuts of dev/int/tst/prod to work
if "%1"=="int" (set server=USILG01-ISI024)
if /i "%1"=="dev" (set server=USILG01-ISD076)
if /i "%1"=="tst" (set server=USILG01-IST155)
if /i "%1"=="prod" (set server=USILG01-ISP232)
if /i "%1"=="deploy" (
	if [%2] equ [] (
		Echo Enter directory name in Temp Release Scripts
		set /p "tempReleaseDir=DirectoryName: "
	) else (
		set tempReleaseDir=%2
	)
	set server=wrbts\bsg\bsgdata\bsg-it\temp release scripts\
	set DestinationPath=%tempReleaseDir%
	set DestinationSubFolder=
	ECHO Will copy to %server%
)

SET DestPath=%server%%DestinationPath%
ECHO Destination path = %DestPath%

ECHO Compiling and Publishing
c:
pushd JamesWebUI\Server
REM Choose debug or release for the environment
SET Env=debug
dotnet publish -c %Env%  --self-contained -nologo -f %NetVersion% -r win-x64

if "%server:~0,5%" neq "wrbts" (
	ECHO Deploying to %server%
	ECHO Stopping services
	SET server=%server:"=%
	ECHO ON
	sc \\%server:"=% stop JamesTheBondSystem

	ECHO Backing up appsettings.json files to "\\%DestPath%%DestinationSubFolder%\appsettings.json" and "\\%DestPath%%DestinationSubFolder%\client.appsettings.json"
	robocopy "\\%DestPath%%DestinationSubFolder%\" "\\%DestPath%\" appsettings.json /w:5 /r:100000
	copy /Y "\\%DestPath%%DestinationSubFolder%\wwwroot\appsettings.json" /A \\$DestPath\client.appsettings.json /A
)

ECHO Copying program files
robocopy Bin\Debug\%NetVersion%\win-x64\publish "\\%DestPath:"=%%DestinationSubFolder%" /MIR /ETA /w:5 /r:7 /XO /xf *.vshost.* appsettings.json appsettings.Development.json /xd Migrations
REM robocopy wwwroot "\\%DestPath:"=%JamesWebUI.Server\\wwwroot" /S /w:5 /r:7 
popd

if /I "%server:~0,5%" neq "wrbts" (
	ECHO Restoring appsettings.json files
	robocopy "\\%DestPath%\." "\\%DestPath%%DestinationSubFolder%" appsettings.json /w:5 /r:100000
	copy /Y \\$DestPath\client.appsettings.json /A "\\%DestPath%%DestinationSubFolder%\wwwroot\appsettings.json" /A

	ECHO Restarting Services
	ECHO Off
	sc \\%server:"=% start JamesTheBondSystem
	:: Wait 4 seconds (by hacking the ping command) and query status
	ping 127.0.0.1 -n 5 > nul
	ECHO ON
	sc \\%server:"=% query JamesTheBondSystem
)
