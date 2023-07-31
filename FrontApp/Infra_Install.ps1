$watch = [system.diagnostics.stopwatch]::StartNew()
Start-Transcript -OutputDirectory c:\temp\ -IncludeInvocationHeader
# ------------------------------DECLARAÇÕES INICIAIS------------------------------------------

$NAME_RUNNER = "PC_Manzi_aws"
$TAG_LIST_RUNNER = "manzi"
$INSTALL_CHORME = "N"

# --------------------------------------------------------------------------------------------

#### INSTALA O IIS
if ((Get-WindowsFeature Web-Server).InstallState -eq "Installed") {
  Write-Host "O IIS já está instalado"
}else{
  Install-WindowsFeature -name Web-Server -IncludeManagementTools
  Import-Module ServerManager
  Add-WindowsFeature Web-Scripting-Tools
  Import-Module WebAdministration
  Write-Host "IIS instalado com sucesso"
  Start-Sleep -s 10
  Remove-WebSite -Name "Default Web Site"
  Remove-WebAppPool -Name "DefaultAppPool"
}

$pentagroDir = 'C:\WebApp\API'
if (-not(Test-Path -Path $pentagroDir)){
  md C:\WebApp\API
  md C:\WebApp\Files
  Write-Host "Diretorios da aplicacao criados!"
}


# --------------------------------------------------------------------------------------------

#### Path IIS
$env:path = $env:path + ";C:\windows\system32\inetsrv\"

# --------------------------------------------------------------------------------------------

#### CRIA APP POOLS
appcmd.exe add apppool /name:RoboDePontos
appcmd.exe add apppool /name:RoboDePontos.API

# --------------------------------------------------------------------------------------------

##### CRIA O SITE
$pentagroDir = 'C:\WebApp'
appcmd.exe add site /name:RoboDePontos /bindings:http://:5000 /physicalpath:$pentagroDir
appcmd.exe set app "RoboDePontos/" /applicationPool:"RoboDePontos"
Write-Host "Site criado com sucesso"

# --------------------------------------------------------------------------------------------

##### CRIA A APP
ConvertTo-WebApplication -PSPath "IIS:\Sites\RoboDePontos\API" -applicationPool:RoboDePontos.API
Write-Host "App criado com sucesso"

# --------------------------------------------------------------------------------------------

#### Para todas as pools para Deploy
appcmd.exe list apppools /state:Started /xml | appcmd.exe stop apppools /in

# --------------------------------------------------------------------------------------------

#### Verifica se o site existe
$existingSite = Get-IISSite -Name "RoboDePontos" -ErrorAction SilentlyContinue
if (-not($existingSite)){
    Write-Host "O site nao foi criado"
}else{
    Write-Host "Site criado com sucesso"
}

# --------------------------------------------------------------------------------------------

#### Install chocolatey
Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))

# -------------------------------------------------------------------------------------------

#### Set silent install mode
choco feature enable -n=allowGlobalConfirmation

# --------------------------------------------------------------------------------------------

#### Microsoft .NET Runtime 6.0.xx
choco install dotnet-6.0-runtime --force

# --------------------------------------------------------------------------------------------

#### Microsoft .NET 6.0.xx - Windows Server Hosting
choco install dotnet-6.0-windowshosting --force

# --------------------------------------------------------------------------------------------

#### Install URL REWRITE
choco install urlrewrite

# --------------------------------------------------------------------------------------------

if($INSTALL_CHORME = "S"){
  choco install googlechrome
}

# --------------------------------------------------------------------------------------------

#### Check installed DOTNET versions
Write-Host "OS MODULOS NET FRAMEWORK ABAIXO FORAM INSTALADOS COM SUCESSO"
Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP' -recurse |
Get-ItemProperty -name Version,Release -EA 0 |
Where { $_.PSChildName -match '^(?!S)\p{L}'} |
Select PSChildName, Version, Release

# --------------------------------------------------------------------------------------------

#### RUNNER

New-Item -ItemType Directory -Path  "C:\GitLab-Runner\" -Force
Start-Transcript -Path "C:\GitLab-Runner\InstallRunnerErrors.txt" -IncludeInvocationHeader
  if (Get-Service -Name "gitlab-runner" -ErrorAction SilentlyContinue)
      {Stop-Service -Name gitlab-runner; sc.exe delete gitlab-runner}
  Invoke-WebRequest -Uri "https://gitlab-runner-downloads.s3.amazonaws.com/latest/binaries/gitlab-runner-windows-amd64.exe" -OutFile "C:\GitLab-Runner\gitlab-runner.exe"
  Set-Location -Path "C:\GitLab-Runner\"
  & .\gitlab-runner.exe install
  & .\gitlab-runner.exe start
  & .\gitlab-runner.exe register --non-interactive --url https://gitlab.com/ --registration-token GR1348941yi1RTVpVa45jzxvhVxv9 --tag-list $TAG_LIST_RUNNER --executor shell --shell powershell --name $NAME_RUNNER --locked=false
Stop-Transcript

# --------------------------------------------------------------------------------------------
Stop-Transcript

Write-Host "Tempo: $($watch.ElapsedMilliseconds / 1000 / 60) minutos"
