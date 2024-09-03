use IliDigitalTest
select SoldierId from SoldierLocation.SoldierLocation where Active=1 group by SoldierId having count(1)>1