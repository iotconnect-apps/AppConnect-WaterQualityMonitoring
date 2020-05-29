/*******************************************************************

EXEC [dbo].[TelemetrySummary_HourWise_Add]	
	
001	sgh-145 18-02-2020 [Nishit Khakhi]	Added Initial Version to Add Telemetry Summary Day wise for Day
*******************************************************************/

CREATE PROCEDURE [dbo].[TelemetrySummary_HourWise_Add]
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY
	DECLARE @dt DATETIME = GETUTCDATE(), @lastExecDate DATETIME
	SELECT 
		TOP 1 @lastExecDate = CONVERT(DATETIME,[value]) 
	FROM [dbo].[Configuration] 
	WHERE [configKey] = 'telemetry-last-exectime' AND [isDeleted] = 0

	BEGIN TRAN		
		DELETE FROM [dbo].[TelemetrySummary_Hourwise] WHERE (CONVERT(DATE,[date]) BETWEEN CONVERT(DATE,@lastExecDate) AND CONVERT(DATE,@dt))  
		
		INSERT INTO [dbo].[TelemetrySummary_Hourwise]([guid]
		,[deviceGuid]
		,[date]
		,[attribute]
		,[min]
		,[max]
		,[avg]
		,[latest]
		,[sum]
		)
		
		SELECT NEWID(), [guid], DATEADD(HOUR,[HOUR],CAST([Date] AS smalldatetime)), [localName], 0, 0, ValueCount, 0, 0
		FROM (
		-- To Get AVG Value of 'co2','currentin','feedpressure','humidity'
		select D.[guid],A.localName, CONVERT(DATE,A.createdDate) [Date], DATEPART(HOUR,A.createdDate) [Hour], AVG(ROUND(TRY_CONVERT(DECIMAL(18,7),attributeValue),2)) ValueCount
		FROM [IOTConnect].[AttributeValue] A
		INNER JOIN [dbo].[Device] D ON A.[uniqueId] = D.[uniqueId] AND D.[isDeleted] = 0
		WHERE (CONVERT(DATE,A.[createdDate]) BETWEEN CONVERT(DATE,@lastExecDate) AND CONVERT(DATE,@dt)) AND A.localName IN ('nh4','turbidity','no2','orp','do','n03','li','clo4',
					'f','br','mg2','ag','na','cu2','ph','cl','temp','lod','bf4','conv','ca2','k','TDS')
		GROUP BY D.[guid],A.localName, CONVERT(DATE,A.createdDate), DATEPART(HOUR,A.createdDate)
		) A
				
		INSERT INTO [dbo].[TelemetrySummary_Hourwise]([guid]
		,[deviceGuid]
		,[date]
		,[attribute]
		,[min]
		,[max]
		,[avg]
		,[latest]
		,[sum]
		)
		
		SELECT NEWID(), [guid], DATEADD(HOUR,[HOUR],CAST([Date] AS smalldatetime)), [localName], 0, 0, 0, 0, ValueCount
		FROM (
		-- To Get SUM of 'flowrate'
		select D.[guid],A.localName, CONVERT(DATE,A.createdDate) [DATE], DATEPART(HOUR,A.createdDate) [Hour], SUM(ROUND(TRY_CONVERT(DECIMAL(18,7),attributeValue),2)) ValueCount
		FROM [IOTConnect].[AttributeValue] A
		INNER JOIN [dbo].[Device] D ON A.[uniqueId] = D.[uniqueId] AND D.[isDeleted] = 0
		WHERE (CONVERT(DATE,A.[createdDate]) BETWEEN CONVERT(DATE,@lastExecDate) AND CONVERT(DATE,@dt)) AND A.localName IN ('consumption')
		GROUP BY D.[guid],A.localName, CONVERT(DATE,A.createdDate), DATEPART(HOUR,A.createdDate)
		) A
		
	COMMIT TRAN	

	END TRY	
	BEGIN CATCH
	DECLARE @errorReturnMessage nvarchar(MAX)

	SELECT
		@errorReturnMessage = ISNULL(@errorReturnMessage, ' ') + SPACE(1) +
		'ErrorNumber:' + ISNULL(CAST(ERROR_NUMBER() AS nvarchar), ' ') +
		'ErrorSeverity:' + ISNULL(CAST(ERROR_SEVERITY() AS nvarchar), ' ') +
		'ErrorState:' + ISNULL(CAST(ERROR_STATE() AS nvarchar), ' ') +
		'ErrorLine:' + ISNULL(CAST(ERROR_LINE() AS nvarchar), ' ') +
		'ErrorProcedure:' + ISNULL(CAST(ERROR_PROCEDURE() AS nvarchar), ' ') +
		'ErrorMessage:' + ISNULL(CAST(ERROR_MESSAGE() AS nvarchar(MAX)), ' ')

	RAISERROR (@errorReturnMessage
	, 11
	, 1
	)

	IF (XACT_STATE()) = -1 BEGIN
		ROLLBACK TRANSACTION
	END
	IF (XACT_STATE()) = 1 BEGIN
		ROLLBACK TRANSACTION
	END
	END CATCH
END

