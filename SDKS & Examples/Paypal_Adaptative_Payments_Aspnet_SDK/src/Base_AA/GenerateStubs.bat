

if not exist Resources\AdaptiveAccounts.WSDL goto nowsdl

del Stub\AdaptiveAccounts.cs /q
echo Generating stub

wsdl /l:CS /o:Stub\AdaptiveAccounts.cs Resources\AdaptiveAccounts.WSDL /n:PayPal.Services.Private.AA
goto end

:nowsdl
echo no wsdl files found.
:end

pause