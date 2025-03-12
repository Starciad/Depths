# ================================================= #
# Depths Build Pipeline                             #
# ================================================= #

# Clear the console window
Clear-Host

# ---------------------- General Settings ---------------------- #
$gameName      = "Depths"
$gameVersion   = "v0.0.0.0"
$outputDirRoot = "..\..\Publish"

# ---------------------- Beauty2 Settings ---------------------- #
$beauty2Dir      = "Libraries"
# List of files to ignore (separated by semicolons)
$beauty2Ignored  = "SDL2.dll;libSDL2-2.0.so.0;libopenal.so.1;libopenal.1.dylib;libSDL2.dylib;soft_oal.dll;"

# ---------------------- Project Paths ---------------------- #
$windowsDXProject  = "..\..\Projects\Depths.Game\Depths.WindowsDX.Game\Depths.WindowsDX.Game.csproj"
$webGLProject      = "..\..\Projects\Depths.Game\Depths.WebGL.Game\Depths.WebGL.Game.csproj"

# ---------------------- Clean Output Directory ---------------------- #
if (Test-Path $outputDirRoot) {
    Remove-Item -Path $outputDirRoot -Recurse -Force
    Write-Output "Existing publish directory found and removed to ensure a clean build."
}

# ---------------------- Restore Projects ---------------------- #
Write-Host "Restoring .NET projects..."

dotnet restore "..\..\Projects\Depths.WindowsDX.Project.sln"
dotnet restore "..\..\Projects\Depths.WebGL.Project.sln"

if ($LASTEXITCODE -ne 0) {
    Write-Error "Restore failed. Exiting..."
    exit 1
}
Write-Host "Restore completed successfully."

# ---------------------- Function Definitions ---------------------- #

# Function to publish a project for a specified runtime
function Publish-Project {
    param (
        [string]$projectName,
        [string]$projectPath,
        [string]$platform
    )

    $publishDir = "$outputDirRoot\$gameName.$gameVersion.$projectName.$platform"
    Write-Host "Publishing '$projectPath' for platform '$platform'..."
    
    dotnet publish $projectPath -c Release -r $platform --output $publishDir
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Publishing failed for platform '$platform'."
        return
    }
    
    # Organize the published output with beauty2
    nbeauty2 --usepatch --loglevel Detail $publishDir $beauty2Dir $beauty2Ignored
    Write-Host "Publishing and organization completed for platform '$platform'."
}

# Function to publish a Blazor (WebGL) project (runtime independent)
function Publish-BlazorProject {
    param (
        [string]$projectName,
        [string]$projectPath
    )

    # Note: WebGL publishing may not be tied to a specific runtime.
    $publishDir = "$outputDirRoot\$gameName.$gameVersion.$projectName.web"
    Write-Host "Publishing '$projectPath'..."
    
    dotnet publish $projectPath -c Release --output $publishDir
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Publishing failed for WebGL project."
        return
    }
    
    Write-Host "WebGL publishing completed."
}

# ---------------------- Publishing Steps ---------------------- #

# Publish WindowsDX project for win-x64
Write-Host "Publishing Depths (WindowsDX) for Win-x64..."
Publish-Project -projectName "windowsdx" -projectPath $windowsDXProject -platform "win-x64"

# Publish WebGL project (Blazor)
Write-Host "Publishing Depths (WebGL)..."
Publish-BlazorProject -projectName "webgl" -projectPath $webGLProject

Write-Host "All publishing processes have been completed."
