@set VDIRNAME=ASPNET_SDK_Platform_Samples_AA
@echo off
CreateVirtualDirectory %VDIRNAME% .
echo adding Virtual Directory in IIS for PayPal
iisreset
explorer http://localhost/%VDIRNAME%/Samples/Calls.aspx
goto end
:end
