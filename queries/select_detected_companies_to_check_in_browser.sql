select 
REPLACE(convert(varchar(32), morion_lat), ',', '.') + ' ' + REPLACE(convert(varchar(32), morion_lng), ',', '.') as [morion GPS]
, REPLACE(convert(varchar(32), google_lat), ',', '.') + ' ' + REPLACE(convert(varchar(32), google_lng), ',', '.') as [google GPS]
, REPLACE(convert(varchar(32), yandex_lat), ',', '.') + ' ' + REPLACE(convert(varchar(32), yandex_lng), ',', '.') as [yandex GPS]
, dbo.distance(morion_lat, morion_lng, google_lat, google_lng) as [mor_goo distance]
, dbo.distance(morion_lat, morion_lng, yandex_lat, yandex_lng) as [mor_yan distance]
, dbo.distance(google_lat, google_lng, yandex_lat, yandex_lng) as [goo_yan distance]
, country + ', ' + region + ' область, ' + city + ', ' + street_type + ' ' + street + ', ' + building as [address]
, google_error
, yandex_error
From ukraine 
where google_lat is not null
or google_error is not null
or yandex_lat is not null
or yandex_error is not null
Order by [mor_goo distance] desc

/*
UPDATE ukraine 
SET google_lat = null, google_lng = null, google_error = null
WHERE google_lat is not null

UPDATE ukraine 
SET yandex_lat = null, yandex_lng = null, yandex_error = null
WHERE yandex_lat is not null


*/