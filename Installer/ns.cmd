netsh http add urlacl url=http://+:%2/ user=%1
if %ERRORLEVEL%==1 goto Ende
:Ende