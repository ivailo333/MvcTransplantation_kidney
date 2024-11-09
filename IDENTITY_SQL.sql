DECLARE @tableName NVARCHAR(255);
DECLARE @sql NVARCHAR(MAX);

-- Деклариране на курсора за всички таблици, които имат идентичност (IDENTITY) колона
DECLARE table_cursor CURSOR FOR
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
AND OBJECTPROPERTY(OBJECT_ID(TABLE_NAME), 'TableHasIdentity') = 1;

OPEN table_cursor;
FETCH NEXT FROM table_cursor INTO @tableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Построяване на SQL командата за нулиране на идентичност колоната
    SET @sql = 'DBCC CHECKIDENT (''' + @tableName + ''', RESEED, 0);';
    -- Изпълнение на SQL командата
    EXEC sp_executesql @sql;

    FETCH NEXT FROM table_cursor INTO @tableName;
END;

CLOSE table_cursor;
DEALLOCATE table_cursor;
