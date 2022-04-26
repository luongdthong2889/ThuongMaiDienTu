
use DBGiaDung
go

CREATE SCHEMA Sp

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Sp_GetReportProductByYear
	@Year INT
AS
BEGIN
---Sp.GetReportProductByYear 2021
select YEAR(trans.transaction_created) as years, pro.product_name, COUNT(pro.product_id) as total
from TRANS trans join ORDERS od on od.transaction_id = trans.transaction_id
                    join PRODUCT pro on pro.product_id = od.product_id
GROUP BY YEAR(trans.transaction_created), pro.product_name
ORDER BY YEAR(trans.transaction_created)
END
GO