-- convert UTF-8 to UCS-2 Big Endian before inserting!!!
BULK INSERT ukraine
FROM 'C:\Users\mr_balya\Documents\GitHub\AxiomaCheckGPS\load\ua.csv'
WITH
(
	FIRSTROW = 2,
	FIELDTERMINATOR = ';',  --CSV field delimiter
	ROWTERMINATOR = '\n',   --Use to shift the control to next row
	ERRORFILE = 'C:\Users\mr_balya\Documents\GitHub\AxiomaCheckGPS\load\byErrorRows.csv',
	TABLOCK
)