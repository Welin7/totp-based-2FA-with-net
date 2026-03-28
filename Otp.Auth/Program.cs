using OtpNet;
using QRCoder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

//Uma chave secreta é gerada para cada usuário - criptografado em repouso
byte[] secretKey = KeyGeneration.GenerateRandomKey();
string base32Secret = Base32Encoding.ToString(secretKey);

const string issuer = "MyAppOtpAuth";
const string user = "user_test@gmail.com";

app.MapGet("otp/qrcode", () =>
{
    string escapedIssuer = Uri.EscapeDataString(issuer);
    string escapedUser = Uri.EscapeDataString(user);
    
    string otpUri =        
        $"""
         otpauth://totp/{escapedIssuer}:{escapedUser}?secret={base32Secret}&issuer={escapedIssuer}&digits=6&period=30
         """;

    using var qrGenerator = new QRCodeGenerator();
    using var qrCodeData = qrGenerator.CreateQrCode(otpUri, QRCodeGenerator.ECCLevel.Q);
    using var qrCode = new PngByteQRCode(qrCodeData);
    byte[] qrCodeImage = qrCode.GetGraphic(20);

    return Results.File(qrCodeImage, "image/png");
});

app.Run();

