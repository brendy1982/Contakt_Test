using Antlr.Runtime;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Web.Services.Description;

namespace Json_Test.Controllers
{
    public class BrandiburApiControllerController : Controller
    {
        
            public JsonResult ContactFormApiKey()
            {
            //JsonRequestBehavior.AllowGet Týmto spôsobom sa umožňuje, aby bol výsledok JSON dát
            //prístupný aj prostredníctvom HTTP GET požiadaviek.
            //Nastavením JsonRequestBehavior.AllowGet sa však explicitne indikuje, že GET metóda
            //je povolená pre túto akciu.
            return Json(new ApiKeyValidator().GetNewKeyPair(), JsonRequestBehavior.AllowGet);
            }
    }

    public class ApiKeyValidator
    {
        string key1 { get; set; } = "ajHsy478$!jds7^hskasdiu&42b";
        string key2 { get; set; } = "dHGteu4^*@jdskjdUJ738jas)ah";

        int ticksOffset = 12345;

        public ApiKeyPair GetNewKeyPair()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddMilliseconds(ticksOffset);

            ApiKeyPair ret = new ApiKeyPair()
            {
                MainKey = Encrypt(now.Ticks.ToString(), key1),
                SubKey = Encrypt(later.Ticks.ToString(), key2),
            };

            return ret;
        }

        public bool IsValid(string apiKey1, string apiKey2)
        {
            if (string.IsNullOrEmpty(apiKey1) || string.IsNullOrEmpty(apiKey2))
            {
                return false;
            }
            try
            {
                return IsValid(new ApiKeyPair() { MainKey = apiKey1, SubKey = apiKey2 });
            }
            catch
            {
                return false;
            }
        }
        public bool IsValid(ApiKeyPair keyPair)
        {
            string now = Decrypt(keyPair.MainKey, key1);
            string later = Decrypt(keyPair.SubKey, key2);

            long ticksNow, ticksLater;
            if (!long.TryParse(now, out ticksNow) || !long.TryParse(later, out ticksLater))
            {
                return false;
            }

            DateTime dtNow = new DateTime(ticksNow).AddMilliseconds(ticksOffset);
            DateTime dtLater = new DateTime(ticksLater);
            if (dtNow != dtLater)
            {
                return false;
            }
            if (dtNow < DateTime.Now.AddHours(-5))
            {
                return false;
            }
            return true;
        }
        /*
         MD5CryptoServiceProvider: Vytváří se instance MD5 pro vytvoření hash
        hodnoty klíče. Toto se často používá k odvození klíče pro symetrické šifrování.

        TripleDESCryptoServiceProvider: Vytváří se instance TripleDES pro trojitě klíčované 
        šifrování, což je starší symetrický šifrovací algoritmus.

        Nastavení klíče: Klíč pro TripleDES je nastaven na MD5 hash hodnotu klíče v UTF-8
        kódování. V praxi se používají bezpečnější postupy pro odvozování klíče.

        Nastavení režimu a výplně: Nastavuje se režim šifrování na ECB a výplň na PKCS7
        . ECB nemusí být vždy bezpečný režim pro šifrování velkého množství dat.

        CreateEncryptor(): Vytváří se transformátor pro šifrování.

        Převedení vstupu na pole bytů: Vstupní text se převede na pole bytů v UTF-8 
        kódování.

        Šifrování: Transformátor se používá ke šifrování vstupních dat.

        Převedení na Base64: Šifrovaná data se převedou na Base64 řetězec a 
        výsledek se vrátí.
         */

        public string Encrypt(string text, string key)
        {
            // Vytvoření instance MD5CryptoServiceProvider pro vytvoření hash hodnoty klíče
            using (var md5 = new MD5CryptoServiceProvider())
            {
                // Vytvoření instance TripleDESCryptoServiceProvider pro trojitě klíčované šifrování
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    // Nastavení klíče pro TripleDES na MD5 hash hodnotu klíče
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                    // Nastavení režimu šifrování na ECB (Electronic Codebook)
                    tdes.Mode = CipherMode.ECB;

                    // Nastavení výplně na PKCS7 (Public Key Cryptography Standards #7)
                    tdes.Padding = PaddingMode.PKCS7;

                    // Vytvoření transformátoru pro šifrování
                    using (var transform = tdes.CreateEncryptor())
                    {
                        // Převedení vstupního textu na pole bytů v UTF-8 kódování
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);

                        // Použití transformátoru pro provedení šifrování nad vstupními daty
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);

                        // Převedení zašifrovaných dat na Base64 řetězec a návrat výsledku
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        public string Decrypt(string cipher, string key)
        {
            // Vytvoření instance MD5CryptoServiceProvider pro vytvoření hash hodnoty klíče
            using (var md5 = new MD5CryptoServiceProvider())
            {
                // Vytvoření instance TripleDESCryptoServiceProvider pro trojitě klíčované dešifrování
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    // Nastavení klíče pro TripleDES na MD5 hash hodnotu klíče
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                    // Nastavení režimu dešifrování na ECB (Electronic Codebook)
                    tdes.Mode = CipherMode.ECB;

                    // Nastavení výplně na PKCS7 (Public Key Cryptography Standards #7)
                    tdes.Padding = PaddingMode.PKCS7;

                    // Vytvoření transformátoru pro dešifrování
                    using (var transform = tdes.CreateDecryptor())
                    {
                        // Převedení zašifrovaného textu na pole bytů
                        byte[] cipherBytes = Convert.FromBase64String(cipher);

                        // Použití transformátoru pro provedení dešifrování nad zašifrovanými daty
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                        // Převedení dešifrovaných dat na text v UTF-8 kódování a návrat výsledku
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }

        public class ApiKeyPair
        {
            public string MainKey { get; set; }
            public string SubKey { get; set; }
        }
    }
}