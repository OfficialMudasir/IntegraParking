c:
cd C:\Backups\Scripts\updatedgt

call "C:\Program Files (x86)\curl\curl" "https://sedeapl.dgt.gob.es/IEST_INTER/MICRODATOS/salida/distintivoAmbiental/export_dist_ambiental.zip" -o export_dist_ambiental.zip
IF NOT %ERRORLEVEL% == 0 GOTO END

C:\app\product\12.1.0\client_1\BIN\unzip export_dist_ambiental.zip
IF NOT %ERRORLEVEL% == 0 GOTO END

del export_dist_ambiental.zip
del DISTINTIVO_AMBIENTAL.pdf

C:\app\product\12.1.0\client_1\BIN\sqlldr c##vehicletype/vehicletype@orcl control=loader.ctl log=loader.log bad=loader.bad direct=true
IF NOT %ERRORLEVEL% == 0 GOTO END

del export_distintivo_ambiental.txt	

C:\app\product\12.1.0\client_1\BIN\sqlplus c##vehicletype/vehicletype@orcl @merge.sql >> script.log

:END
echo ERROR!!!!