/*******************************************************************
DECLARE @output INT = 0
		,@fieldName				nvarchar(255)
EXEC [dbo].[CompanyStatistics_Get]
	 @guid				= '2D442AEA-E58B-4E8E-B09B-5602E1AA545A'	
	,@invokingUser  	= '7D31E738-5E24-4EA2-AAEF-47BB0F3CCD41'
	,@version			= 'v1'
	,@output			= @output		OUTPUT
	,@fieldName			= @fieldName	OUTPUT	
               
 SELECT @output status,  @fieldName AS fieldName    
 
001	SAQM-1 17-03-2020 [Nishit Khakhi]	Added Initial Version to Get Company Statistics
*******************************************************************/

CREATE PROCEDURE [dbo].[CompanyStatistics_Get]
(	 @guid				UNIQUEIDENTIFIER	
	,@invokingUser		UNIQUEIDENTIFIER	= NULL
	,@version			NVARCHAR(10)
	,@output			SMALLINT		  OUTPUT
	,@fieldName			NVARCHAR(255)	  OUTPUT	
	,@culture			NVARCHAR(10)	  = 'en-Us'
	,@enableDebugInfo	CHAR(1)			  = '0'
)
AS
BEGIN
    SET NOCOUNT ON
	IF (@enableDebugInfo = 1)
	BEGIN
        DECLARE @Param XML
        SELECT @Param =
        (
            SELECT 'CompanyStatistics_Get' AS '@procName'
			, CONVERT(nvarchar(MAX),@guid) AS '@guid'			
	        , CONVERT(nvarchar(MAX),@invokingUser) AS '@invokingUser'
			, CONVERT(nvarchar(MAX),@version) AS '@version'
			, CONVERT(nvarchar(MAX),@output) AS '@output'
            , CONVERT(nvarchar(MAX),@fieldName) AS '@fieldName'
            FOR XML PATH('Params')
	    )
	    INSERT INTO DebugInfo(data, dt) VALUES(Convert(nvarchar(MAX), @Param), GETUTCDATE())
    END
    Set @output = 1
    SET @fieldName = 'Success'

    BEGIN TRY
		
		;WITH CTE_Facilities
		AS (	SELECT [companyGuid], COUNT(1) [totalCount], SUM(CASE WHEN [type] = 'indoor' THEN 1 ELSE 0 END) AS [totalIndoorZones], SUM(CASE WHEN [type] = 'outdoor' THEN 1 ELSE 0 END) AS [totalOutdoorZones]
				FROM [dbo].[Entity] (NOLOCK) 
				WHERE [companyGuid] = @guid AND [isDeleted] = 0 
				 and [Guid] not in (select entityGuid from [dbo].[Company] where [Guid]=@Guid) 
				GROUP BY [companyGuid]
		)		
		SELECT [guid]
				, L.[totalCount] AS [totalFacilities]
				, L.[totalIndoorZones] + L.[totalOutdoorZones]  AS [totalZones]
				, 0 AS [totalAlerts]
				, L.[totalIndoorZones] AS [totalIndoorZones]
				, L.[totalOutdoorZones] AS [totalOutdoorZones]
		FROM [dbo].[Company] C (NOLOCK) 
		LEFT JOIN CTE_Facilities L ON C.[guid] = L.[companyGuid]
		WHERE C.[guid]=@guid AND C.[isDeleted]=0
		
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

