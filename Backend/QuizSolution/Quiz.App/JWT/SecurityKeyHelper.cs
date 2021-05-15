using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Quiz.App.JWT
{
    public  class SecurityKeyHelper    //MYSECRETKEY (APPSETTİNGS.JSON)
    {
        public static SecurityKey CreateSecurityKey(string securityKey) //MYSECRETMYSECRET (APPSETTİNGS.JSON)
        {
            //APPSETTINGSDEKİ BELİRLEMİŞ OLDUĞUMUZ SECURİTYKEY STRİNGİNİ, BYTE'A ÇEVİRİP SYMETRİCSECURİTYKEY OLARAK DÖNDÜREREK
            // JWT TOKENİN KULLANABİLECEĞİ BİR HALE GETİRİYORUZ.

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

        }
    }
}