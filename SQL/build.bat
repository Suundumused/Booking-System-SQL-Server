@echo off

SET SERVER_INSTANCE="DESKTOP-NLHVPUD"
SET DATABASE_NAME="master"
SET OUTPUT_LOG="Logs\build_result.txt"

sqlcmd -S %SERVER_INSTANCE% -d %DATABASE_NAME% -i "Schemas\DataBase.sql" -o %OUTPUT_LOG% -E

SET DATABASE_NAME="HotelNetworkDB"

sqlcmd -S %SERVER_INSTANCE% -d %DATABASE_NAME% -i "Schemas\Tables.sql" -o %OUTPUT_LOG% -E
sqlcmd -S %SERVER_INSTANCE% -d %DATABASE_NAME% -i "Schemas\Indexes.sql" -o %OUTPUT_LOG% -E

for %%G in (Schemas\Functions\*.sql) do sqlcmd -S %SERVER_INSTANCE% -d %DATABASE_NAME% -i %%G -o %OUTPUT_LOG% -E
for %%G in (Schemas\Procedures\*.sql) do sqlcmd -S %SERVER_INSTANCE% -d %DATABASE_NAME% -i %%G -o %OUTPUT_LOG% -E
for %%G in (Schemas\Triggers\*.sql) do sqlcmd -S %SERVER_INSTANCE% -d %DATABASE_NAME% -i %%G -o %OUTPUT_LOG% -E
for %%G in (Schemas\Views\*.sql) do sqlcmd -S %SERVER_INSTANCE% -d %DATABASE_NAME% -i %%G -o %OUTPUT_LOG% -E

sqlcmd -S %SERVER_INSTANCE% -d %DATABASE_NAME% -i "Schemas\Hotels.sql" -o %OUTPUT_LOG% -E

IF %ERRORLEVEL% NEQ 0 (
    echo An error occurred during SQL script execution. Check %OUTPUT_LOG%
    pause
) ELSE (
    echo SQL creation script executed successfully.
)

pause