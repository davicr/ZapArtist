using System;
using System.IO;
using PNGCompression;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Newtonsoft.Json.Linq;

namespace ZapArtist
{
    class Program
    {
        static void Main(string[] args)
        {
            // We'll need an image filename as the first argument
            if (args.Length > 2)
            {
                string pngPath = args[0];
                string assetsFolder = args[1];
                string stickerPack = args[2];

                // Acquire file name without extension and the folder where the converted sticker should be stored
                string baseName = Path.GetFileName(pngPath);
                baseName = baseName.Substring(0, baseName.LastIndexOf('.'));
                string webpFolder = Path.Combine(assetsFolder, stickerPack);

                Console.WriteLine("[ * ] Generating sticker from: {0}", pngPath);

                // Does the file exist?
                if (!File.Exists(pngPath))
                {
                    Console.WriteLine("[ ! ] File does not exist");
                    return;
                }

                // Is a PNG?
                if (!pngPath.EndsWith(".png"))
                {
                    Console.WriteLine("[ ! ] File is not a PNG");
                    return;
                }

                // Is a 512x512 image?
                using (var image = System.Drawing.Image.FromFile(pngPath))
                {
                    if (image.Width != 512 || image.Height != 512)
                    {
                        Console.WriteLine("[ ! ] Invalid sticker size, should be 512x512");
                        return;
                    }
                }

                string tmpPath = Path.Combine(webpFolder, baseName + ".tmp");

                // Optimize PNG image
                var compressor = new PNGCompressor();
                compressor.CompressImageLossy(pngPath, tmpPath);

                // Convert to WebP
                var newName = baseName + ".webp";
                using (var imageFactory = new ImageFactory(preserveExifData: false))
                {
                    ISupportedImageFormat webp = new WebPFormat { Quality = 75 };
                    imageFactory.Load(tmpPath);
                    imageFactory.Format(webp);
                    imageFactory.Save(Path.Combine(webpFolder, newName));
                }

                // Delete compressed temporary file
                if (File.Exists(tmpPath))
                    File.Delete(tmpPath);

                // Add to JSON
                JObject stickerJson = JObject.Parse(File.ReadAllText(Path.Combine(assetsFolder, "contents.json")));
                foreach (var pack in stickerJson["sticker_packs"])
                {
                    if (pack["identifier"].ToString() == stickerPack)
                    {
                        JObject sticker = new JObject();
                        sticker["image_file"] = newName;
                        sticker["emojis"] = new JArray();

                        JArray stickers = (JArray)pack["stickers"];
                        stickers.Add(sticker);
                    }
                }
                File.WriteAllText(Path.Combine(assetsFolder, "contents.json"), stickerJson.ToString());
            } else
            {
                Console.WriteLine(".\\ZapArtist.exe <png image> <asset folder> <sticker pack>");
            }
        }
    }
}
