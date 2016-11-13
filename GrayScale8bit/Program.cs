using Mono.Options;
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
			string path = null;
			bool backup = false;

			var optionSet = new OptionSet()
				.Add("f|file=", "Write output to file.", v => path = v)
				.Add("b|backup", "Save Backup File", v => backup = v != null);

			var res = optionSet.Parse(args);

			if(path == null)
			{
				ShowUsage(optionSet);
				return;
			}

			var backupFile = path + ".bak";

			File.Move(path, backupFile);

			var uri = new Uri(backupFile, UriKind.Relative);

			// load
			WriteableBitmap bitmap;

			using (var ms = new MemoryStream(File.ReadAllBytes(backupFile)))
			{
				bitmap = new WriteableBitmap(BitmapFrame.Create(ms));
			}

			var formatConvertedBitmap = new FormatConvertedBitmap(bitmap,
				PixelFormats.Gray8, null, 0);

			using (var fs = new FileStream(path, FileMode.Create))
			{
				BitmapEncoder encoder = new BmpBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(formatConvertedBitmap));
				encoder.Save(fs);
			}

			if(!backup)
			{
				File.Delete(backupFile);
			}
		}

		// Uasgeを表示する
		private static void ShowUsage(OptionSet p)
		{
			Console.Error.WriteLine("Usage:GrayScale8bit [OPTIONS]");
			Console.Error.WriteLine();
			p.WriteOptionDescriptions(Console.Error);
		}
	}
}
