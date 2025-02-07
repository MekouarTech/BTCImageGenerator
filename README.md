BTC Price Image Generator

A C# project that generates a square JPG image displaying the current Bitcoin (BTC) price. The image features a black background with white text, including "BTC" at the top and the price below it. The price is fetched from an API, and the image is saved locally and uploaded to a server, replacing the previous image.
Features

    Fetches the current BTC price from an API.
    Generates a square JPG image with a black background and white text.
    Displays the label "BTC" and the current price.
    Saves the generated image locally.
    Automatically uploads and replaces the old image on your server.

Technologies Used

    C#
    System.Drawing.Common (for image generation)
    Newtonsoft.Json (for parsing JSON)
    System.Net.Http (for making HTTP requests)

Setup

    Clone the repository:

    git clone https://github.com/yourusername/btc-price-image-generator.git

    Install the necessary NuGet packages:
        System.Drawing.Common
        Newtonsoft.Json

    Configure your server URL for uploading the image in the script.

    Run the script to generate and upload the BTC price image.

Usage

The script will:

    Fetch the current BTC price.
    Generate a square image with the BTC price.
    Save the image locally in the images folder.
    Upload the image to your server, replacing the old image.

License

This project is open-source and available under the MIT License.
