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
Message type : uint8 (1)			:	01					--> Telemetry Type Cmd
IMEI : buffer (7)					:	0145292a2b54ca		--> Unique
Timestamp : uint32 (4):				:	4d388f5d			--> Hex to Decimal 1295552349 Convert epoch to human-readable date and vice versa
Battery voltage : uint8 (1)			:	63					--> Battery (Volts) = ((battery * 10) + 3000) / 1000; -->  3.63V
Temperature (oC): int8 (1)			:	1f
Latitude: int32 (4)					:	291e7916			--> Latitude = Latitude / 10000000   -->  291e7916 = ‭689862934‬ / 10000000 = 68.9862934
Longitude: int32 (4)				:	93330910			--> Longitude = Longitude / 10000000 -->  93330910 = ‭2469595408‬ / 10000000 = 246.9595408
Altitude (MSL in meters):int16 (2)	:	3d00
Speed (SOG * 100km/h) : uint16 (2)	:	0000
Course (COG) : uint8 (1)			:	00
Number of satellites : uint8 (1)	:	05
Time to fix (FF = no fix):uint8 (1)	:	56
Measured Distance(cm): uint16 (2)	:	6400
Fill Level (%):uint8 (1)			:	00
Signal (%):uint8 (1)				:	16
Status Flags:uint8 (1)				:	4b
Last Reset Cause:uint8 (1)			:	10
Firmware Version:uint16 (2)			:	6500
Attributes:
 01020145292A2B54CA1E00000000000000000000007800041401030A640000000F


ACK (for both Attr + Telemtry)

ACK Package
2-Bytes
Example:
01A0

Version: 01
Message: A0  - The Message Type for the ACK message is 0xA0.
