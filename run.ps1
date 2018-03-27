Start-Process -NoNewWindow dotnet .\bin\Debug\netcoreapp2.0\Deck2MTGA-Web.dll
start http://localhost:5000
pause
ps dotnet | kill
