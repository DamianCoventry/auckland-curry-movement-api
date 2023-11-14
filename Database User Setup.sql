-- Create Microsoft Entra login
Use master
CREATE LOGIN [damian.coventry_gmail.com#EXT#@damiancoventrygmail.onmicrosoft.com] FROM EXTERNAL PROVIDER

SELECT name, type_desc, type, is_disabled 
FROM sys.server_principals
WHERE type_desc like 'external%'


-- Create user from a Microsoft Entra login
Use auckland-curry-movement
CREATE USER [acm] FROM LOGIN [damian.coventry_gmail.com#EXT#@damiancoventrygmail.onmicrosoft.com]

SELECT name, type_desc, type 
FROM sys.database_principals 
WHERE type_desc like 'external%'
