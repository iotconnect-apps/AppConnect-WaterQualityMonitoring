/*******************************************************************
DECLARE @count INT
     ,@output INT = 0
	,@fieldName	NVARCHAR(255)
	,@syncDate	DATETIME
EXEC [dbo].[Chart_WaterConsumption]	
	@guid	= 'E0573755-4DE1-42CD-A71F-16403ABDDC4B'
	,@frequency = 'm'
	,@invokinguser  = 'E05A4DA0-A8C5-4A4D-886D-F61EC802B5FD'              
	,@version		= 'v1'              
	,@output		= @output		OUTPUT
	,@fieldname		= @fieldName	OUTPUT	
	,@syncDate		= @syncDate		OUTPUT
SELECT @output status, @fieldName fieldName, @syncDate syncDate

001	SWS-4 06-04-2020 [Nishit Khakhi]	Added Initial Version to represent Operating Hours by Building / Company
*******************************************************************/
CREATE PROCEDURE [dbo].[Chart_WaterConsumption]
(	@guid				UNIQUEIDENTIFIER		
	,@frequency			CHAR(1)				
	,@invokinguser		UNIQUEIDENTIFIER	= NULL
	,@version			nvarchar(10)              
	,@output			SMALLINT			OUTPUT
	,@fieldname			nvarchar(255)		OUTPUT
	,@syncDate			DATETIME			OUTPUT
	,@culture			nvarchar(10)		= 'en-Us'	
	,@enabledebuginfo	CHAR(1)				= '0'
)
AS
BEGIN
    SET NOCOUNT ON

    IF (@enabledebuginfo = 1)
	BEGIN
        DECLARE @Param XML 
        SELECT @Param = 
        (
            SELECT 'Chart_WaterConsumptionByBuilding' AS '@procName' 
            , CONVERT(nvarchar(MAX),@guid) AS '@guid' 
			, CONVERT(nvarchar(2),@frequency) AS '@frequency' 
			, CONVERT(nvarchar(MAX),@version) AS '@version' 
            , CONVERT(nvarchar(MAX),@invokinguser) AS '@invokinguser' 
            FOR XML PATH('Params')
	    ) 
	    INSERT INTO DebugInfo(data, dt) VALUES(Convert(nvarchar(MAX), @Param), GETUTCDATE())
    END                    
    
    BEGIN TRY  

		DECLARE @startDate DATETIME, @endDate DATETIME

		IF @frequency = 'W'
		BEGIN

			IF OBJECT_ID ('tempdb..#wresult') IS NOT NULL BEGIN DROP TABLE #result END
			CREATE TABLE [#wresult] ([DATE] DATETIME, [value] DECIMAL(18,2))

			SET @startDate = GETUTCDATE() - 7
			SET @endDate = GETUTCDATE() 
									
			INSERT INTO [#wresult]
			SELECT CONVERT(Date,T.[date]), SUM([sum]) AS [value] 
			FROM [dbo].[TelemetrySummary_Hourwise] T (NOLOCK)
			INNER JOIN [dbo].[Device] D (NOLOCK) ON T.[deviceGuid] = D.[guid] AND D.[isDeleted] = 0
			WHERE T.[deviceGuid] = @guid AND [attribute] = 'consumption' AND CONVERT(Date,[date]) BETWEEN CONVERT(DATE,@startDate) AND CONVERT(DATE,@endDate)
			GROUP BY CONVERT(Date,T.[date])
			
			SELECT CONCAT(DATENAME(day, DATEADD(DAY, (T.i - 6), @endDate)), ' - ', FORMAT( DATEADD(DAY, (T.i - 6), @endDate), 'ddd')) AS [name]
			, ISNULL(RES.[value],0) AS [waterConsumption]
			FROM (VALUES (6), (5), (4), (3), (2), (1), (0)) AS T(i)
			LEFT OUTER JOIN ( SELECT * FROM [#wresult]) RES ON RES.[DATE] = DATEADD(DAY, (T.i - 6), CAST(@endDate AS Date))
			ORDER BY DATEADD(DAY, (T.i - 6), @endDate)

		END
		ELSE
		BEGIN

			IF OBJECT_ID ('tempdb..#result') IS NOT NULL BEGIN DROP TABLE #result END
			CREATE TABLE #result ([year] INT, [month] TINYINT, [localName] NVARCHAR(1000), [value] DECIMAL(18,2))

			IF OBJECT_ID ('tempdb..#months') IS NOT NULL BEGIN DROP TABLE #months END
			CREATE TABLE [#months] ([date] DATETIME)

			INSERT INTO [#months]
			SELECT CONVERT(DATE, DATEADD(Month, (T.i - 11), GETUTCDATE())) AS [Date]
			FROM (VALUES (11), (10), (9), (8), (7), (6), (5), (4), (3), (2), (1), (0)) AS T(i)

			SELECT @startDate = MIN(CONVERT(DATE, [Date] - DAY([Date]) + 1)), @endDate = MAX(CONVERT(DATE,EOMONTH([Date])))
			FROM [#months]

			INSERT INTO #result
			SELECT DATEPART(YY,[date]),DATEPART(MM,[date]), [attribute], SUM([sum]) AS [value] 
			FROM [dbo].[TelemetrySummary_Hourwise] T (NOLOCK)
			INNER JOIN [dbo].[Device] D (NOLOCK) ON T.[deviceGuid] = D.[guid] AND D.[isDeleted] = 0
			WHERE T.[deviceGuid] = @guid AND [attribute] = 'consumption' AND CONVERT(Date,[date]) BETWEEN CONVERT(DATE,@startDate) AND CONVERT(DATE,@endDate)
			GROUP BY DATEPART(YY,[date]),DATEPART(MM,[date]), [attribute]
			
			SELECT SUBSTRING(DATENAME(MONTH, M.[date]), 1, 3) + '-' + FORMAT(M.[date],'yy') AS [name]
					, ISNULL(R.[value],0) AS [waterConsumption]
			FROM [#months] M
			LEFT OUTER JOIN #result R ON R.[Month] = DATEPART(MM, M.[date]) AND R.[Year] = DATEPART(YY, M.[date]) 
			ORDER BY  M.[date]
			
		END
		
		SET @output = 1
		SET @fieldname = 'Success'   
        SET @syncDate = (SELECT TOP 1 CONVERT(DATETIME,[value]) FROM dbo.[Configuration] (NOLOCK) WHERE [configKey] = 'telemetry-last-exectime')      
	END TRY
	BEGIN CATCH	
		DECLARE @errorReturnMessage nvarchar(MAX)

		SET @output = 0

		SELECT @errorReturnMessage = 
			ISNULL(@errorReturnMessage, '') +  SPACE(1)   + 
			'ErrorNumber:'  + ISNULL(CAST(ERROR_NUMBER() as nvarchar), '')  + 
			'ErrorSeverity:'  + ISNULL(CAST(ERROR_SEVERITY() as nvarchar), '') + 
			'ErrorState:'  + ISNULL(CAST(ERROR_STATE() as nvarchar), '') + 
			'ErrorLine:'  + ISNULL(CAST(ERROR_LINE () as nvarchar), '') + 
			'ErrorProcedure:'  + ISNULL(CAST(ERROR_PROCEDURE() as nvarchar), '') + 
			'ErrorMessage:'  + ISNULL(CAST(ERROR_MESSAGE() as nvarchar(max)), '')
		RAISERROR (@errorReturnMessage, 11, 1)  
 
		IF (XACT_STATE()) = -1  
		BEGIN   
			ROLLBACK TRANSACTION 
		END   
		IF (XACT_STATE()) = 1  
		BEGIN      
			ROLLBACK TRANSACTION  
		END   
		RAISERROR (@errorReturnMessage, 11, 1)   
	END CATCH
END