netsh http add urlacl url=http://+:%2/ user=%1 delegate=yes
if %ERRORLEVEL%==1 goto Ende
:Ende