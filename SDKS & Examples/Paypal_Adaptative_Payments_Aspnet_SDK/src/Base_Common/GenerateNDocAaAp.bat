@echo off

:run
echo Generating NDoc 


echo %cd%

set NDOC="Tools\NDoc\NDocConsole.exe"

%NDOC% -documenter=MSDN-CHM aspnet_sdk\src\Base_Common\bin\Release\Base_Common.DLL aspnet_sdk\src\Base_AA\bin\Release\Base_AA.DLL aspnet_sdk\src\Base_AP\bin\Release\Base_AP.DLL

move doc\Documentation.chm "aspnet_sdk\docs\PayPalBaseAPI.chm"

del doc\* /q

del doc\ndoc_msdn_temp\* /q

rd doc\ndoc_msdn_temp

rd doc

:end
