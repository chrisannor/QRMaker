# QR Maker

QR Maker is a simple API for generating QR codes from URLS. It uses the QRCoder library to create QR codes and serves them as downloadable files. As of now it supports generating QR codes in svg format.

## Features

- Generate QR codes from URLs
- Download QR codes as SVG files
- Simple API interface

## Getting Started

1. Clone the repository:
2. ```bash
    git clone https://github.com/chrisannor/QRMaker.git
    cd QRMaker
   ```
3. Install the required packages:
   ```bash
   dotnet restore
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

## Usage

The following endpoints are available:

- `GET /qr?url={url}&fileName={fileName}`: Generates a QR code from the provided URL and saves it with the specified file name. The file will be saved in the root directory of the project under the `generatedCodes` folder.
- `GET /qr/download?url={url}&fileName={fileName}`: Downloads the generated QR code file
- `GET /qr/health`: Checks the health status of the API
