@echo off
echo Ejecutando pruebas con analisis de cobertura...

REM Limpiamos resultados anteriores
if exist ".\TestResults" rd /s /q ".\TestResults"
if exist ".\CoverageReport" rd /s /q ".\CoverageReport"

REM Ejecutamos las pruebas con Coverlet usando la configuración personalizada
dotnet test ./Coderland.Test/Coderland.Test.csproj --settings ./Coderland.Test/coverlet.runsettings --collect:"XPlat Code Coverage" --results-directory:"./TestResults"

REM Generamos el informe HTML usando reportgenerator global
reportgenerator -reports:"./TestResults/*/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:Html

echo.
echo Informe de cobertura generado en la carpeta CoverageReport
echo Abriendo informe...
start "" ".\CoverageReport\index.html"
