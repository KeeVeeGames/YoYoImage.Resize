using System;
using System.Collections.Generic;
using System.IO;
using ImageMagick;

namespace GMAssetCompiler
{
	// Token: 0x02000003 RID: 3
	public partial class YoYoImage
	{
		// Token: 0x0600000A RID: 10
		public YoYoImage Resize(int _newWidth, int _newHeight)
		{
			YoYoImage ret = new YoYoImage(_newWidth, _newHeight);
			MagickImage image = new MagickImage(MagickColors.Transparent, this.Width, this.Height);
			image.ColorSpace = ColorSpace.sRGB;
			image.ColorType = ColorType.TrueColorAlpha;
			image.Alpha(AlphaOption.Set);
			image.Depth = 8;
			using (IPixelCollection<byte> pixelCollection = image.GetPixels())
			{
				pixelCollection.SetArea(0, 0, this.Width, this.Height, this.mPixels);
			}
			IReadOnlyCollection<IMagickImage<byte>> channels = image.Separate(Channels.All);
			foreach (IMagickImage<byte> magickImage in channels)
			{
				magickImage.Scale(_newWidth, _newHeight);
			}
			image = (MagickImage)new MagickImageCollection(channels).Combine();
			using (IPixelCollection<byte> pixelCollectionResult = image.GetPixels())
			{
				ret.mPixels = pixelCollectionResult.GetArea(0, 0, image.Width, image.Height);
			}
			ret.mImageInfo = new ImageInfo(image.Width, image.Height, 8, true);
			return ret;
		}
	}
}
