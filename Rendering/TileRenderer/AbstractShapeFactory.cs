using Mapster.Common.MemoryMappedTypes;

namespace Mapster.Rendering;

public abstract class AbstractShapeFactory
{
    public abstract Railway createRailway(MapFeatureData feature);
    public abstract PopulatedPlace createPopulatedPlace(MapFeatureData feature);
    public abstract Border createBorder(MapFeatureData feature);
    public abstract Waterway createWaterway(MapFeatureData feature);
    public abstract Road createRoad(MapFeatureData feature);
    public abstract GeoFeature createGeoFeature(MapFeatureData feature);
}