use IliDigitalTest;

if object_id('tempdb..#temp') is not null drop table tempdb..#temp;
delete SoldierInfo.Soldier; 
delete SoldierInfo.[Rank];
delete SoldierInfo.Country;
delete SoldierInfo.SensorType;
delete SoldierLocation.SoldierLocation;
delete SoldierLocation.SourceType;

select top 100000 
	newid() id, 
	'Mike ' + cast(row_number() over (order by column_id) as varchar) [Name], 
	system_type_id SensorTypeId,
	sys.objects.schema_id RankId,
	sys.objects.object_id CountryId,
	'{"Training": "' + sys.objects.name + '"}' TrainingInfo,
	sys.columns.name SensorName
into #temp
from sys.objects, sys.columns, sys.schemas
order by column_id;

insert SoldierInfo.Rank select distinct RankId, 'Rank ' + cast(RankId as varchar) from #temp;
insert SoldierInfo.Country select distinct CountryId, 'Country ' + cast(CountryId as varchar) from #temp;
insert SoldierInfo.SensorType select distinct SensorTypeId, 'Sensor Type ' + cast(SensorTypeId as varchar) from #temp;
/*
select * from SoldierInfo.rank
select * from SoldierInfo.Country
select * from SoldierInfo.SensorType
*/
insert SoldierInfo.Soldier 
select 
	Id,
	[Name],
	RankId, 
	CountryId, 
	TrainingInfo,
	SensorName,
	SensorTypeId
from #temp;
/*
select * from SoldierInfo.Soldier 
*/
insert SoldierLocation.SourceType values (1, 'User');
insert SoldierLocation.SourceType values (2, 'Message');
