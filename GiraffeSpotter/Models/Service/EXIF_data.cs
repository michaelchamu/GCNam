using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace GiraffeSpotter.Models.Service
{
    public static class EXIF_data
    {

        /// <summary>
        /// Gets the latitude component from the GPS metadata.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static decimal? GetLatitude(this Image image)
        {
            try
            {
                //PropertyTagGpsLatitudeRef - 'N' or 'S'
                PropertyItem propItemRef = image.GetPropertyItem(1);
                //PropertyTagGpsLatitude
                PropertyItem propItemLat = image.GetPropertyItem(2);
                return ExifGpsToFloat(propItemRef, propItemLat);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the longitude component from the GPS metadata.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static decimal? GetLongitude(this Image image)
        {
            try
            {
                //PropertyTagGpsLongitudeRef - 'E' or 'W'
                PropertyItem propItemRef = image.GetPropertyItem(3);
                //PropertyTagGpsLongitude
                PropertyItem propItemLong = image.GetPropertyItem(4);
                return ExifGpsToFloat(propItemRef, propItemLong);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private static decimal ExifGpsToFloat(PropertyItem propItemRef, PropertyItem propItem)
        {
            uint degrees = GetExifSubValue(propItem, 0);
            uint minutes = GetExifSubValue(propItem, 1);
            uint seconds = GetExifSubValue(propItem, 2);

            decimal dec1 = (decimal)minutes/60;
            decimal dec2 = (decimal)seconds/3600;

            decimal tot = dec1 + dec2;

            decimal coorditate = degrees + tot;
            string gpsRef = System.Text.Encoding.ASCII.GetString(new byte[1] { propItemRef.Value[0] }); //N, S, E, or W
            if (gpsRef == "S" || gpsRef == "W")
                coorditate = 0 - coorditate;
            return coorditate;
        }

        private static uint GetExifSubValue(PropertyItem property, int index)
        {
            int baseIndex = index * 8;
            uint numerator = BitConverter.ToUInt32(property.Value, baseIndex);
            uint denominator = BitConverter.ToUInt32(property.Value, baseIndex + 4);
            return numerator / denominator;
        }

        /// <summary>
        /// The Contents of this MVC 4 Class Was retrieved from http://denwilliams.net/2013/07/17/gps-metadata/ and modified by Donovan Maasz in Mey 2014
        /// Email: donovanmaasz@gmail.com
        /// Email: dmdmaasz6.reg@outlook.com
        /// Email: maaszdonovan@live.com
        /// Tel: +264814239878
        /// </summary>

    }
}