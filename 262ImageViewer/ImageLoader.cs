/* 
 * ImageLoader.cs
 * 
 * Version: 
 *     $Id$ 
 * 
 * Revisions: 
 *     $Log$ 
 */

using System;
using System.Drawing;
using System.IO;

namespace ImageLoader
{
    /*
     * Image interface.
     * Used to allow ImageProxy to load images when necessary.
     */
    public interface Image
    {
        /*
         * Get a given slice of the image.
         * sliceIndex is the pixel index from top-left corner;
         * vertical is true if the slice cuts vertically, else horizontal.
         * 
         * Returns a bitmap where one dimension is 1 pixel,
         *  and the other is the size of the bitmap.
         */
        Bitmap getSlice(int sliceIndex, Boolean vertical);

        /*
         * Get the Bitmap representation of the image.
         */
        Bitmap getImage();

        /*
         * Get the Bitmap's width.
         */
        int getWidth();

        /*
         * Get the Bitmap's height.
         */
        int getHeight();
    }

    /*
     * ImageProxy
     */
    //public class ImageProxy : Image
    //{
    //    
    //}

    /*
     * JPGImage
     */
    public class JPGImage : Image
    {
        /*
         * The image itself.
         */
        private Bitmap image;

        /*
         * Constructor from a Uri.
         */
        public JPGImage(Uri imageURI)
        {
            image = new Bitmap(imageURI.AbsolutePath);
        }

        /*
         * Get the Bitmap representation of the image.
         */
        public Bitmap getImage()
        {
            return image;
        }

        /*
         * Get the Bitmap's width.
         */
        public int getWidth()
        {
            return image.Width;
        }

        /*
         * Get the Bitmap's height.
         */
        public int getHeight()
        {
            return image.Height;
        }

        /*
         * Get a given slice of the image.
         * sliceIndex is the pixel index from top-left corner;
         * vertical is true if the slice cuts vertically, else horizontal.
         * 
         * Returns a bitmap where one dimension is 1 pixel,
         *  and the other is the size of the bitmap.
         */
        public Bitmap getSlice(int sliceIndex, Boolean vertical)
        {
            if (vertical)
            {
                return image.Clone(new Rectangle(sliceIndex, 0, 1, image.Height), image.PixelFormat);
            } 
            else 
            {
                return image.Clone(new Rectangle(0, sliceIndex, image.Width, 1), image.PixelFormat);
            }
        }
    }

    /*
     * ACRImage
     */
    public class ACRImage : Image
    {
        /*
         * The image itself.
         */
        private Bitmap image;

        /*
         * Constructor from a Uri.
         */
        public ACRImage(Uri imageURI)
        {
            image = readACR(imageURI);
        }

        /*
         * Get the Bitmap representation of the image.
         */
        public Bitmap getImage()
        {
            return image;
        }

        /*
         * Get the Bitmap's width.
         */
        public int getWidth()
        {
            return image.Width;
        }

        /*
         * Get the Bitmap's height.
         */
        public int getHeight()
        {
            return image.Height;
        }

        /*
         * Get a given slice of the image.
         * sliceIndex is the pixel index from top-left corner;
         * vertical is true if the slice cuts vertically, else horizontal.
         * 
         * Returns a bitmap where one dimension is 1 pixel,
         *  and the other is the size of the bitmap.
         */
        public Bitmap getSlice(int sliceIndex, Boolean vertical)
        {
            if (vertical)
            {
                return image.Clone(new Rectangle(sliceIndex, 0, 1, image.Height), image.PixelFormat);
            }
            else
            {
                return image.Clone(new Rectangle(0, sliceIndex, image.Width, 1), image.PixelFormat);
            }
        }

        /*
         * The logic for reading in an ACR image to a Bitmap.
         */
        private Bitmap readACR(Uri imageUri) {


            Int64 HEADER_OFFSET = 0x2000;

	        FileStream imageFile = new FileStream(imageUri.AbsolutePath, FileMode.Open);
	        imageFile.Seek(HEADER_OFFSET, SeekOrigin.Begin);

	        int sliceWidth = 256;
	        int sliceHeight = 256;
	    
	        Bitmap sliceBuffer = 
                new Bitmap(sliceWidth, sliceHeight, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale);

	        for ( int i = 0; i < sliceHeight; i++ ) {
	            for ( int j = 0; j < sliceWidth; j++ ) {
		            int pixelHigh = 0;
		            int pixelLow = 0;
		            int pixel;

		            pixelHigh = imageFile.ReadByte();
		            pixelLow = imageFile.ReadByte();
		            pixel = pixelHigh << 4 | pixelLow >> 4;
		    
		            sliceBuffer.SetPixel( j, i, Color.FromArgb(pixel << 16 | pixel << 8 | pixel));
	            }
	        }
            return sliceBuffer;
        }
    }
}
