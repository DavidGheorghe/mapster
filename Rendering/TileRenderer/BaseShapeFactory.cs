using Mapster.Common.MemoryMappedTypes;
using Mapster.Rendering;

namespace Mapster.Rendering;

public class BaseShapeFactory : AbstractShapeFactory
{
    public override Border createBorder(MapFeatureData feature)
    {
        return new Border(feature.Coordinates);
    }

    public override GeoFeature createGeoFeature(MapFeatureData feature)
    {
        if (feature.RenderType == RenderType.GEOFEATURE)
        {
            return new GeoFeature(feature.Coordinates, feature);
        }
        else if (feature.RenderType == RenderType.GEOFEATURE_FOREST)
        {
            return new GeoFeature(feature.Coordinates, GeoFeature.GeoFeatureType.Forest);
        }
        else if (feature.RenderType == RenderType.GEOFEATURE_RESIDENTIAL)
        {
            return new GeoFeature(feature.Coordinates, GeoFeature.GeoFeatureType.Residential);
        }
        else if (feature.RenderType == RenderType.GEOFEATURE_PLAIN)
        {
            return new GeoFeature(feature.Coordinates, GeoFeature.GeoFeatureType.Plain);
        }
        else
        {
            return new GeoFeature(feature.Coordinates, GeoFeature.GeoFeatureType.Water);
        }
    }

    public override PopulatedPlace createPopulatedPlace(MapFeatureData feature)
    {
        return new PopulatedPlace(feature.Coordinates, feature);
    }

    public override Railway createRailway(MapFeatureData feature)
    {
        return new Railway(feature.Coordinates);
    }

    public override Road createRoad(MapFeatureData feature)
    {
        return new Road(feature.Coordinates);
    }

    public override Waterway createWaterway(MapFeatureData feature)
    {
        return new Waterway(feature.Coordinates, feature.Type == GeometryType.Polygon);
    }

    
}