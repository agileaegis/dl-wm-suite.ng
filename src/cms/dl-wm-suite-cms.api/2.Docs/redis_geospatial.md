GEOADD: Adds or updates one or more members to a Geo Set O(log (N)) where N is the number of elements in the sorted set.
GEODIST: Return the distance between two members in the geo spatial index represented by the sorted set O(log(N)).
GEOHASH: Gets valid Geohash strings representing the position of one or more elements from the Geo Sets O(log(N)), where N is the number of elements in the sorted set.
GEOPOS: Return the longitude,latitude of all the specified members of the geo spatial sorted set at key O(log(N)), where N is the number of elements in the sorted set.
GEORADIUS: Return the members of a sorted set populated with geo spatial information using GEOADD, which are within the borders of the area specified with the center location and the maximum distance from the center (the radius) O(N+log(M))
GEORADIUSBYMEMBER: Same as GEORADIUS with the only difference that instead of taking, as the center of the area to query, it takes a longitude and latitude value O(N+log(M))



sorted sets

ZADD: Adds or updates one or more members to a Sorted Set O(log (N))
ZRANGE: Gets the specified range by rank of elements in the Sorted Set O(log (N) +M).
ZRANGEBYSCORE: Gets elements from the Sorted Sets within the range by score given values are in ascending order O(log (N) +M)
ZREVRANGEBYSCORE: Gets elements from the Sorted Sets within the score given O(log (N) +M)
ZREVRANK: The rank of the member in the Sorted Set O (log (N))
ZREVRANGE: Returns the specified range of elements in the Sorted Set O(log (N) + M)
ZREM: Removes the specified members in the Sorted Set O(M*log (N))
ZREMRANGEBYRANK: Removes the members in a Sorted Set within the given indexes O(log (N) * M)
ZREMRANGEBYSCORE: Removes the members in a Sorted Set within the given scores O(log (N) * M)
ZCARD: Gets the number of members in a Sorted Set O(1)
ZCOUNT: Gets the number of members in a Sorted Set within the score boundaries O(log (N) * M)
ZINCRBY: Increases the score of an element in the Sorted Set O(log (N))
ZINTERSTORE: Calculates the common elements in the Sorted Sets given by the specified keys, and stores the result in destination Sorted Set O(N*K) + O (M*log (M))
ZRANK: Gets the index of the element in the Sorted Set O(log (N))
ZSCORE: Returns the score of the member O(1)
ZUNIONSTORE: Computes the union of keys in the given Sorted Set and stores the result in the resultant Sorted Set O(N) + O(M log (M))

list

LPUSH: Prepends the values to the left of the list O(1).
RPUSH: Prepends the values to the right of the list O(1).
LPUSHX: Prepends the values to the left of the list if key exist O(1).
RPUSHX: Prepends the values to the right of the list if key exist O(1).
LINSERT: Inserts a value in the list after position calculated from left O(N).
LSET: Sets the value of an element in a list based on index O(N).
LRANGE: Gets the sub list of elements based on start and end position O(S+E).
LTRIM: Deletes the elements outside the range specified O(N), where N is the length.
RPOP: Removes the last element O(1).
LREM: Removes the element at the index point O(N) , where N is the length.
LPOP: Removes the first element of the list O(1).
LINDEX: Gets the element from the list based on the index O(N) where N is the length of traverse.
LLEN: This command gets the length of the list O(1).
RPOPLPUSH: Operates on two lists source and destination list, takes the last element on the source list and push it to the first element of the destination list O(1).

{
  "latitude": 38.05018593679694,
  "longitude": 23.76426726579666
}

38.056807, 23.771011

{
  "geoPoint": {
    "lat": 38.056807,
    "long": 23.771011
  },
  "pointId": "dmstr-a89b24a8-9a66-40cf-88a3-f0eb0b596759"
}

curl -X GET "http://localhost:6200/api/v1/Maps/geofence/52ddb6a7-396d-4193-8664-2b90b27bb19f" -H "accept: application/json"


http://dev.virtualearth.net/REST/v1/Locations/38.0501859367969,23.7642672657967?o=xml&c=el&key=m0HlwlDQGKnphfEQXFU6~D8hpcOahW3Cl2dx9t2A4dg~Anv8eoE_8QtSX-1SJeQ3RflkKYph91GtHX2MZvyfSSPfv7aQ8gyYm16DMzD1CL2E
http://dev.virtualearth.net/REST/v1/Locations/38.0501859367969,23.7642672657967?o=json&c=el&key=m0HlwlDQGKnphfEQXFU6~D8hpcOahW3Cl2dx9t2A4dg~Anv8eoE_8QtSX-1SJeQ3RflkKYph91GtHX2MZvyfSSPfv7aQ8gyYm16DMzD1CL2E