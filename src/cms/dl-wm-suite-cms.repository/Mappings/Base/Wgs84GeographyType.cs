using System;
using NHibernate.Spatial.Type;

[Serializable]
public class Wgs84GeographyType : PostGisGeometryType
{
    protected override void SetDefaultSRID(GeoAPI.Geometries.IGeometry geometry)
    {
        geometry.SRID = 4326;
    }
}