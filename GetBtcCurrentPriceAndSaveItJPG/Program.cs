using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using Newtonsoft.Json;

class BTCImageGenerator
{
    private static readonly HttpClient client = new HttpClient();

    private static string imageDirectory = "images";

    static async Task Main(string[] args)
    {
        try
        {
            // Fetch the current BTC price from CoinGecko API
            string btcPrice = await GetCurrentBTCPrice();

            // Generate the image with the BTC price
            byte[] imageBytes = GenerateImage(btcPrice);

            // Save the image locally
            SaveImageLocally(imageBytes);

            // Upload the image to the server (replace the previous image)
            await UploadImageToServer(imageBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    private static async Task<string> GetCurrentBTCPrice()
    {
        string url = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd";
        var response = await client.GetStringAsync(url);
        dynamic data = JsonConvert.DeserializeObject(response);
        return data.bitcoin.usd.ToString();
    }

    private static byte[] GenerateImage(string btcPrice)
    {
        int width = 600;
        int height = 600;

        using (var bitmap = new Bitmap(width, height))
        using (var graphics = Graphics.FromImage(bitmap))
        {
            // Set background color to black
            graphics.Clear(Color.Black);

            var font = new Font("Arial", 48, FontStyle.Bold);
            var brush = new SolidBrush(Color.White);

            string btcText = "BTC";
            string priceText = $"${FormatPrice(btcPrice)}";

            var btcTextSize = graphics.MeasureString(btcText, font);
            var priceTextSize = graphics.MeasureString(priceText, font);

            var btcTextPosition = new PointF((width - btcTextSize.Width) / 2, height / 3 - btcTextSize.Height / 2);
            var priceTextPosition = new PointF((width - priceTextSize.Width) / 2, height * 2 / 3 - priceTextSize.Height / 2);

            graphics.DrawString(btcText, font, brush, btcTextPosition);

            graphics.DrawString(priceText, font, brush, priceTextPosition);

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }


    private static string FormatPrice(string price)
    {
        if (decimal.TryParse(price, out decimal result))
        {
            return result.ToString("#,0", CultureInfo.InvariantCulture);
        }
        else
        {
            return price;  // Return as-is if parsing fails
        }
    }

    private static void SaveImageLocally(byte[] imageBytes)
    {
        // Ensure the 'images' directory exists
        if (!Directory.Exists(imageDirectory))
        {
            Directory.CreateDirectory(imageDirectory);
        }

        // Generate a unique filename based on current date/time
        string filename = Path.Combine(imageDirectory, $"btc_price_{DateTime.Now:yyyyMMdd_HHmmss}.jpg");

        // Save the image as a JPG file
        File.WriteAllBytes(filename, imageBytes);
        Console.WriteLine($"Image saved locally at: {filename}");
    }

    private static async Task UploadImageToServer(byte[] imageBytes)
    {
        string serverUrl = "https://yourserver.com/upload";  // Replace with your server URL
        using (var content = new MultipartFormDataContent())
        {
            var byteArrayContent = new ByteArrayContent(imageBytes);
            content.Add(byteArrayContent, "file", "btc_price.jpg");

            // Send the POST request to upload the image
            var response = await client.PostAsync(serverUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Image uploaded successfully to the server.");
            }
            else
            {
                Console.WriteLine("Failed to upload the image to the server.");
            }
        }
    }
}
