﻿using Mapster.Common.MemoryMappedTypes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Mapster.Rendering;

public static class TileRenderer
{
    public static BaseShape Tessellate(this MapFeatureData feature, ref BoundingBox boundingBox, ref PriorityQueue<BaseShape, int> shapes)
    {
        BaseShape? baseShape = GetShape(feature);
        if (baseShape != null)
        {
            shapes.Enqueue(baseShape, baseShape.ZIndex);
            for (var j = 0; j < baseShape.ScreenCoordinates.Length; ++j)
            {
                boundingBox.MinX = Math.Min(boundingBox.MinX, baseShape.ScreenCoordinates[j].X);
                boundingBox.MaxX = Math.Max(boundingBox.MaxX, baseShape.ScreenCoordinates[j].X);
                boundingBox.MinY = Math.Min(boundingBox.MinY, baseShape.ScreenCoordinates[j].Y);
                boundingBox.MaxY = Math.Max(boundingBox.MaxY, baseShape.ScreenCoordinates[j].Y);
            }
        }

        return baseShape;
    }

    public static Image<Rgba32> Render(this PriorityQueue<BaseShape, int> shapes, BoundingBox boundingBox, int width, int height)
    {
        var canvas = new Image<Rgba32>(width, height);

        // Calculate the scale for each pixel, essentially applying a normalization
        var scaleX = canvas.Width / (boundingBox.MaxX - boundingBox.MinX);
        var scaleY = canvas.Height / (boundingBox.MaxY - boundingBox.MinY);
        var scale = Math.Min(scaleX, scaleY);

        // Background Fill
        canvas.Mutate(x => x.Fill(Color.White));
        while (shapes.Count > 0)
        {
            var entry = shapes.Dequeue();
            entry.TranslateAndScale(boundingBox.MinX, boundingBox.MinY, scale, canvas.Height);
            canvas.Mutate(x => entry.Render(x));
        }

        return canvas;
    }

    public struct BoundingBox
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;
    }

    private static BaseShape GetShape(MapFeatureData feature)
    {
        BaseShape? baseShape = null;
        BaseShapeFactory baseShapeFactory = new BaseShapeFactory();
        if (feature.RenderType == RenderType.ROAD)
        {
            baseShape = baseShapeFactory.createRoad(feature);
        }
        else if (feature.RenderType == RenderType.BORDER)
        {
            baseShape = baseShapeFactory.createBorder(feature);
        }
        else if (feature.RenderType == RenderType.RAILWAY)
        {
            baseShape = baseShapeFactory.createRailway(feature);
        }
        else if (IsPartOfGeofeatureRenderType(feature.RenderType))
        {
            baseShape = baseShapeFactory.createGeoFeature(feature);
        }
        else if (feature.RenderType == RenderType.POPULATED_PLACE)
        {
            baseShape = baseShapeFactory.createPopulatedPlace(feature);
        }
        else if (feature.RenderType == RenderType.WATERWAY)
        {
            baseShape = baseShapeFactory.createWaterway(feature);
        }
        return baseShape;
    }

    private static bool IsPartOfGeofeatureRenderType(RenderType renderType)
    {
        return renderType == RenderType.GEOFEATURE
            || renderType == RenderType.GEOFEATURE_PLAIN
            || renderType == RenderType.GEOFEATURE_DESERT
            || renderType == RenderType.GEOFEATURE_FOREST
            || renderType == RenderType.GEOFEATURE_MOUNTAINS
            || renderType == RenderType.GEOFEATURE_RESIDENTIAL
            || renderType == RenderType.GEOFEATURE_WATER
            || renderType == RenderType.GEOFEATURE_UNKNOWN;

    }
}
