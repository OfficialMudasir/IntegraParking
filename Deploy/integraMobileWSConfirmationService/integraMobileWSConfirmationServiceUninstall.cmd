copy integraMobileWSConfirmationService.exe.config %SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe.config
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u integraMobileWSConfirmationService.exe
del %SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe.config
@pause
