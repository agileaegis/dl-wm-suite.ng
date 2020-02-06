CREATE EXTENSION ltree;

CREATE EXTENSION postgis;
CREATE EXTENSION postgis_topology;
CREATE EXTENSION postgis_sfcgal;
CREATE EXTENSION fuzzystrmatch;
CREATE EXTENSION address_standardizer;
CREATE EXTENSION address_standardizer_data_us;
CREATE EXTENSION postgis_tiger_geocoder;


SELECT postgis_lib_version();

SELECT ST_AsGeoJSON(ST_Transform(c.location,4326))
from containers as c
 limit 2;



 --cd310632-317a-407d-9f0a-ab3c00ac5f56 Dionysis
-- cd310632-317a-407d-9f0a-ab3c00ac5f56 Tour
SELECT e.id AS employee_id, t.id AS tour_id, t.scheduled_date AS scheduled_date, v.num_plate AS NumberPlate
FROM employees AS e
INNER JOIN employeeroles AS er on e.employeerole_id = er.id
INNER JOIN employeestours AS et on e.id = et.employee_id
INNER JOIN tours AS t on et.tour_id = t.id
INNER JOIN vehicles AS v on t.vehicle_id = v.id
WHERE er.notes = 'DRIVER' AND t.scheduled_date > now()::date - 365;
;
