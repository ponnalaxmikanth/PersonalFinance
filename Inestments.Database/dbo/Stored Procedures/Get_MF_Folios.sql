CREATE PROCEDURE [dbo].[Get_MF_Folios]
	--exec [Get_MF_Folios]
AS
BEGIN
	select * from MF_Folios	order by FolioId
END