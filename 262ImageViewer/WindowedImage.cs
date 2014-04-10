using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _262ImageViewer
{
    class WindowedImage
    {

        private Bitmap processImage(int high, int low, Bitmap to_be_processed)
        {
            var image_height = to_be_processed.Height;
            var image_width = to_be_processed.Width;
            Bitmap processed_image = new Bitmap(image_width, image_height);

            for (int i = 0; i <= image_height; i++)
            {
                for (int j = 0; j <= image_width; j++)
                {
                    Color pixel = to_be_processed.GetPixel(i, j);
                    if (pixel.GetBrightness() < low)
                    {
                        Color new_pixel = Color.Black;
                        processed_image.SetPixel(i, j, new_pixel);
                    }
                    else if(pixel.GetBrightness() > high)
                    {
                        Color new_pixel = Color.White;
                        processed_image.SetPixel(i, j, new_pixel);
                    }
                    else
                    {
                        float sf = equate_scale_factor(high, low, pixel);
                        int color_value = (int)(sf * 255);
                        Color new_pixel = Color.FromArgb(color_value, color_value, color_value);
                        processed_image.SetPixel(i, j, pixel);
                    }

                }
            }

            return processed_image;

        }

        private float equate_scale_factor(int high, int low, Color pixel)
        {
            float output = ((pixel.GetBrightness() - low)*high) / (high - low);
            return output;
        }

    }
}
