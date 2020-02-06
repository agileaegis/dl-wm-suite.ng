1. Start-up
Sensor Back-End
------ --------
| [Startup] |
| Telemetry message |
| ----------------------------> |
| ACK response |
| <---------------------------- |
| Attributes message |
| ----------------------------> |
| ACK response |
| <---------------------------- |
| |


2. Normal operation

Sensor Back-End
------ --------
| [Normal Operation] |
| Telemetry message (every 30min) |
| -------------------------------> |
| ACK response |
| <------------------------------- |
| Attributes message (every 1.5h) |
| -------------------------------> |
| ACK response |
| <------------------------------- |
| |


3. Movement detected

Sensor Back-End
------ --------
| [Movement Detected] |
| Telemetry message (every 3min) |
| -------------------------------> |
| ACK response |
| <------------------------------- |
| |


4. Attributes change
Sensor Back-End
------ --------
| |
| [Config change] |
| Attributes message |
| -------------------------------> |
| Attributes change request |
| <------------------------------- |
| Attributes message |
| -------------------------------> |
| ACK response |
| <------------------------------- |
| |


Telemetry: 

01010145292a2b54ca4d388f5d631f291e7916933309103d000000000556640000164b106500

01 01 0145292a2b54ca 4d388f5d 63 1f 291e7916 93330910 3d00 0000 00 05 56 6400 00 16 4b 10 6500


Version : uint8 (1)					:	01
Message type : uint8 (1)			:	01				--> Telemetry Type Cmd
IMEI : buffer (7)					:	0145292a2b54ca	--> LE: CA542B2A294501	--> Unique
Timestamp : uint32 (4):				:	4d388f5d		--> LE: 5D8F384D --> Hex to Decimal = 1569667149 Convert epoch to human-readable date and vice versa --> epoch Saturday, September 28, 2019 10:39:09 AM
Battery voltage : uint8 (1)			:	63				--> LE: 63 --> Decimal 99 --> Battery = (Volts) = ((battery * 10) + 3000) / 1000; -->  3.99V
Temperature (oC): int8 (1)			:	1f				--> LE: 1F --> Decimal	  --> Temp = 31 oC
Latitude: int32 (4)					:	291e7916		--> LE: 16791E29 --> Decimal 377036329	--> Latitude = Latitude / 10000000   -->  377036329‬ / 10000000 = 37.7036329
Longitude: int32 (4)				:	93330910		--> LE: 10093393 --> Decimal 269038483  --> Longitude = Longitude / 10000000 -->  269038483 / 10000000 = 26.9038483
Altitude (MSL in meters):int16 (2)	:	3d00			--> LE: 003d --> Decimal 61
Speed (SOG * 100km/h) : uint16 (2)	:	0000            --> LE: 0000 --> Decimal 00
Course (COG) : uint8 (1)			:	00				--> LE: 00   --> Decimal 00
Number of satellites : uint8 (1)	:	05				--> LE: 05   --> Decimal 00
Time to fix (FF = no fix):uint8 (1)	:	56				--> LE: 56   --> Decimal 86
Measured Distance(cm): uint16 (2)	:	6400		    --> LE: 0064 --> Decimal 100
Fill Level (%):uint8 (1)			:	00				--> LE: 00   --> Decimal 00
Signal (%):uint8 (1)				:	16				--> LE: 16   --> Decimal 22
Status Flags:uint8 (1)				:	4b				--> LE: 4B   --> Decimal 75
Last Reset Cause:uint8 (1)			:	10				--> LE: 10   --> Decimal 16
Firmware Version:uint16 (2)			:	6500			--> LE: 0065 --> Decimal 101
Attributes:
 01020145292A2B54CA1E00000000000000000000007800041401030A640000000F


ACK (for both Attr + Telemtry)

ACK Package
2-Bytes
Example:
01A0

Version: 01
Message: A0  - The Message Type for the ACK message is 0xA0.
