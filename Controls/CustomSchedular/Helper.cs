using CommunityToolkit.Maui.Core.Extensions;
using Microsoft.Maui.Controls.Shapes;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace CustomSchedularControl.Controls.CustomSchedular
{
    public static class Helper
    {
        public static Color ReduceAlpha(this Color color)
        {
            return Color.FromRgba(color.GetByteRed(), color.GetByteGreen(), color.GetByteBlue(), (byte)64);
        }

        public const string FilledClock = "M173.721 347.442c95.919 0 173.721-77.802 173.721-173.721S269.64 0 173.721 0 0 77.802 0 173.721s77.802 173.721 173.721 173.721zm-12.409-272.99c0-6.825 5.584-12.409 12.409-12.409s12.409 5.584 12.409 12.409v93.313l57.39 45.912c5.336 4.281 6.204 12.098 1.923 17.434a12.342 12.342 0 0 1-9.679 4.653c-2.73 0-5.46-.869-7.755-2.73L165.966 183.4c-2.916-2.358-4.653-5.894-4.653-9.679V74.452z";
        
        public static BindableProperty StringPathDataProperty = BindableProperty.CreateAttached("StringPathData", typeof(string), typeof(Helper),
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