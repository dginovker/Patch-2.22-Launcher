#define MyAppName "Patch 2.22 Launcher"
#define MyAppExeName "Restarter.exe"
#define MyAppExeVersion "1.0.5.1"
#define MyAppPublishFolder "PatchLauncher"

[Setup]
AppName={#MyAppName}
AppId=Patch 2.22 Launcher
AppVersion={#MyAppExeVersion}
AppVerName={#MyAppName}
WizardStyle=modern
DefaultDirName=C:\{#MyAppName}
DefaultGroupName={#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
VersionInfoDescription=Patch 2.22 Launcher Setup
VersionInfoProductName=Patch 2.22 Launcher
OutputDir=setup
OutputBaseFilename=BFMELauncherSetup
DisableWelcomePage=no
PrivilegesRequired=admin
MissingMessagesWarning=yes
NotRecognizedMessagesWarning=yes
Compression=lzma2
SolidCompression=yes
ShowLanguageDialog=yes
WizardImageFile=setup.bmp
LicenseFile=ReadMe.txt
SetupIconFile=MainIcon.ico
VersionInfoVersion={#MyAppExeVersion}
AppSupportURL=https://discord.com/invite/Q5Yyy3XCuu
AppPublisher=Raphael Vogel
AppPublisherURL=https://github.com/Ravo92

ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: en; MessagesFile: "compiler:Default.isl"
Name: de; MessagesFile: "compiler:Languages\German.isl"

[Messages]
en.BeveledLabel=English
de.BeveledLabel=Deutsch

[CustomMessages]
en.CreateStartMenuIcon=Create start menu entries
en.LaunchAfterInstall=Launch Application after Install

de.CreateStartMenuIcon=Erstelle Verkn�pfungen im Startmen�
de.LaunchAfterInstall=Starte die Anwendung nach der Installation

[Files]
Source: "{#MyAppPublishFolder}\*"; DestDir: "{app}"; Flags: recursesubdirs ignoreversion
Source: "{#MyAppPublishFolder}\{#MyAppExeName}"; DestDir: "{app}"

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#MyAppExeName}"; Tasks: startmenuicon
Name: "{userdesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
;Name: "{userdesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; Check: Not FileExists(ExpandConstant('{userdesktop}\{#MyAppName}.lnk'))

[Tasks]
Name: "startmenuicon"; Description: "{cm:CreateStartMenuIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[InstallDelete]
Type: files; Name: "{app}\PatchLauncherBFME.exe"
Type: files; Name: "{app}\PatchLauncherBFME.dll"
Type: files; Name: "{app}\PatchLauncherBFME.dll.config"
Type: files; Name: "{app}\PatchLauncherBFME.deps.json"
Type: files; Name: "{app}\PatchLauncherBFME.runtimeconfig.json"
Type: filesandordirs; Name: "{app}\PatchLauncherBFME.exe.WebView2"

[UninstallDelete]
Type: filesandordirs; Name: "{app}\Downloads"

[Run]
Filename: {app}\{#MyAppExeName}; Description: {cm:LaunchAfterInstall}; Flags: postinstall shellexec nowait unchecked skipifsilent; Parameters: "--showLauncherUpdateLog"
Filename: {app}\{#MyAppExeName}; Flags: postinstall nowait shellexec skipifnotsilent; Parameters: "--showLauncherUpdateLog"