using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;

namespace dcbadge.Helpers
{
    public class QRGen
    {
        public string genQRCode64(String qrText)
        {
            
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);

            string retrunText = Convert.ToBase64String(qrCodeImage);

            return retrunText;
        }

        public byte[] genQRCodeByte(String qrText)
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);

            return qrCodeImage;
        }
    }
}
