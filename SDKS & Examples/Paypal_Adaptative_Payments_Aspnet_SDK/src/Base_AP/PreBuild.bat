@echo off
pushd .
cd ..\..
if exist GenerateStubs.bat call GenerateStubs.bat
popd
