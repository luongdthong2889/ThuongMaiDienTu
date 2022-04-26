use DBGiaDung
go

select YEAR(trans.transaction_created) as years, pro.product_name, COUNT(pro.product_id) as total
from TRANS trans join ORDERS od on od.transaction_id = trans.transaction_id
                    join PRODUCT pro on pro.product_id = od.product_id
GROUP BY YEAR(trans.transaction_created), pro.product_name
ORDER BY YEAR(trans.transaction_created)