/****** Script for SelectTopNRows command from SSMS  ******/
  
  Use Bus_info3
  
  
  
  --Update Connections
  --Set [Order] = 22
  --where RouteId = 101 and Arrive = 0 and Id = 3564
  
  select * from Stations
  where Id in (4063,4066)
  
  --Update Connections
  --Set PolyLine = 'gpziSmjv`Af@M|Aa@VIRGZItAEx@B', Distance = 198
  --where Id = 3598
  
  --6
  --Update Connections
  --Set PolyLine = 'gpziSmjv`Af@M|Aa@VIRGZItAEx@B', Distance = 198
  --where Id = 3598
  
  --15
  --Update Connections
  --Set PolyLine = 'uoziSy}y`AYiDU_CI{@]eDI_AQyB', Distance = 400
  --where Id = 3607
  
  --16
  --Update Connections
  --Set PolyLine = 'ksziS_tz`AKiAYiCMwAQyAWwCQsB', Distance = 370
  --where Id = 3608
  
  --17
  --Update Connections
  --Set PolyLine = 'yvziSoh{`AKmAEc@QaBMO', Distance = 131
  --where Id = 3609
  
  --18
  --Update Connections
  --Set PolyLine = 'kxziSso{`AOEIE]ISIOIOK[QKEICMEe@GMAM@]H@J', Distance = 167
  --where Id = 3610
  
  --19
  --Update Connections
  --Set PolyLine = 's`{iS{q{`AAKq@Nc@JKBkCr@TrATfAJd@', Distance = 258
  --where Id = 3611
  
  --20
  --Update Connections
  --Set PolyLine = 'mf{iSki{`AFXd@`CDTN~@DRaD`A', Distance = 248
  --where Id = 3612
  
  --21
  --Update Connections
  --Set PolyLine = 'gi{iSc_{`AyDhAkEtA{@X', Distance = 266
  --where Id = 3613
  
  --22
  --Update Connections
  --Set PolyLine = 'iw{iSkyz`AwBp@[?c@}AQi@', Distance = 167
  --where Id = 3564
  
  --56
  --Update Connections
  --Set PolyLine = 'siijSsa_aAM{BAc@SkEa@mGGaANGHAJ?H@zDd@z@x@RLhA|@r@xAU^KPnA`D', Distance = 828
  --where Id = 3614
  
  select * from Connections
  where RouteId = 101 and Arrive = 0