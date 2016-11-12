using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GrayScale8bit
{
	class Program
	{
		static void Main(string[] args)
		{
			var path = args[0];

			File.Move(path, path + ".bak");

			var uri = new Uri(path + ".bak", UriKind.Relative);

			// load
			var bitmap = new BitmapImage(uri);

			var formatConvertedBitmap = new FormatConvertedBitmap(bitmap,
				PixelFormats.Gray8, null, 0);

			using (var fs = new FileStream(path, FileMode.Create))
			{
				BitmapEncoder encoder = new BmpBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(formatConvertedBitmap));
				encoder.Save(fs);
			}
		}
	}
}
