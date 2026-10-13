using System.Text.Json;
using GeoJSON.Text.Feature;
using GeoJSON.Text.Geometry;
using SFML.System;

namespace polx {
  class Map {
    public static async Task<Map> Load(string dir) {
      var features = await ParseGeojson(Path.Join(dir, "nations.geojson"));
      foreach (var feature in features) {
        foreach (var point in feature) {

        }
      }
    }

    private static async Task<IEnumerable<IEnumerable<IEnumerable<IEnumerable<Vector2f>>>>> ParseGeojson(string path) {
      using var stream = File.OpenRead(path);
      var collection = await JsonSerializer.DeserializeAsync<FeatureCollection>(stream) ?? throw new Exception($"GeoJSON file missing: {path}");

      return collection.Features.Select((feature) => {
        if (feature.Geometry is MultiPolygon multiPolygon) {
          return multiPolygon.Coordinates;
        } else if (feature.Geometry is Polygon polygon) {
          return [polygon];
        } else {
          throw new Exception($"Invalid geometry type '${feature.Geometry.GetType().Name}' in feature '${feature.Id}'");
        }
      }).Select((polygons) =>
        polygons.Select((polygon) =>
          polygon.Coordinates.Select((lineStr) =>
            lineStr.Coordinates.Select((coord) => LambertProjection(coord, 180))
          )
        )
      );
    }

    /**
    x = λ − λ0
    y = sin φ
    where
    φ is the latitude,
    λ is the longitude and
    λ0 is the central meridian.
    */
    private static Vector2f LambertProjection(IPosition pos, int centerX) {
      return new Vector2f(
        (float)pos.Longitude - centerX,
        (float)Math.Sin(pos.Latitude)
      );
    }
  }
}
