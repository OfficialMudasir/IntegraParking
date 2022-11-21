
if not exist Resources\AdaptivePayments.WSDL goto nowsdl

del Stub\AdaptivePayments.cs /q

echo Generating stub
wsdl /l:CS /o:Stub\AdaptivePayments.cs Resources\AdaptivePayments.WSDL /n:PayPal.Services.Private.AP

goto end

:nowsdl
echo no wsdl files found.
:end

pause