using Microsoft.Maui.Controls.Shapes;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace CustomSchedularControl.Helpers
{
    public class AttachedCollection
    {
        public static BindableProperty StringPathDataProperty = BindableProperty.CreateAttached("StringPathData", typeof(string), typeof(AttachedCollection),
              propertyChanged: delegate (BindableObject bindable, object oldVal, object newVal)
              {
                  if (bindable is Path path && newVal is string val && !string.IsNullOrEmpty(val) && !string.IsNullOrWhiteSpace(val))
                      path.Data = (Geometry)(new PathGeometryConverter().ConvertFromInvariantString(val));
              }, defaultValue: null);

        public static string GetStringPathData(BindableObject view) => (string)view.GetValue(StringPathDataProperty);

        public static void SetStringPathData(BindableObject view, string value)
        {
            view.SetValue(StringPathDataProperty, value);
        }
    }
}