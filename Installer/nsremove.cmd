netsh http delete urlacl url=http://+:%1/
if %ERRORLEVEL%==1 goto Ende
:Ende