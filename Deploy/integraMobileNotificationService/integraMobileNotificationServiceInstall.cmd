copy integraMobileNotificationService.exe.config %SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe.config
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe integraMobileNotificationService.exe
del %SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe.config
@pause
