using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;
using System.Text;

namespace dcbadge.Helpers
{
    public class QRGen
    {
        public string genQRCode64(String qrText)
        {
            Helpers.Sql sql = new Helpers.Sql();
            string retrunText = "";

            if (sql.verifyQR(qrText))
            {

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
                byte[] qrCodeImage = qrCode.GetGraphic(20);

                retrunText = Convert.ToBase64String(qrCodeImage);

            }

            return retrunText;
        }

        public byte[] genQRCodeByte(String qrText)
        {

            Helpers.Sql sql = new Helpers.Sql();
            byte[] qrCodeImage = Encoding.UTF8.GetBytes("");


            if (sql.verifyQR(qrText))
            {

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
                qrCodeImage = qrCode.GetGraphic(20);

            }



            return qrCodeImage;
        }
    }
}
