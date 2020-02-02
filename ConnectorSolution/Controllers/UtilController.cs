using Connector.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Data;

namespace Connector.Controllers
{
    public class UtilController : HomeController
    {
        public string CheckLogin(string email, string pass, out Usuario user)
        {
            string sRet = string.Empty;
            user = null;
            //
            try
            {
                Encryption cryp = new Encryption();
                pass = cryp.Encrypt(pass);
                //
                user = db.Usuario.Where(a => a.Email.Equals(email) && a.Pass.Equals(pass) && a.Ative == 1).FirstOrDefault();
                //
                if (user != null)
                {
                    ++user.Count;
                    user.Ative = 1;
                    user.Last = DateTime.Now;
                    //
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    //
                    sRet = "ok";
                }
                else
                {
                    sRet = "user";
                }

            }
            catch (Exception exc)
            {
                return exc.Message;
            }

            //
            return sRet;
        }

        public class Encryption
        {
            private const string CryptoKey = "2014073012345678";
            private static Encryption _instance;
            private RijndaelManaged _rijndael;

            public static Encryption Instance
            {
                get
                {
                    return Encryption._instance ?? (Encryption._instance = new Encryption());
                }
            }

            public string Decrypt(string encryptedText)
            {
                return Encoding.UTF8.GetString(this.Decrypt(Convert.FromBase64String(encryptedText), this.GetEncryptor()));
            }

            public string Encrypt(string plainText)
            {
                return Convert.ToBase64String(this.Encrypt(Encoding.UTF8.GetBytes(plainText), this.GetEncryptor()));
            }

            public RijndaelManaged GetEncryptor()
            {
                if (this._rijndael == null)
                {
                    byte[] numArray = new byte[16];
                    byte[] bytes = Encoding.UTF8.GetBytes("2014073012345678");
                    Array.Copy((Array)bytes, (Array)numArray, Math.Min(numArray.Length, bytes.Length));
                    RijndaelManaged rijndaelManaged = new RijndaelManaged();
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    rijndaelManaged.KeySize = 128;
                    rijndaelManaged.BlockSize = 128;
                    rijndaelManaged.Key = numArray;
                    rijndaelManaged.IV = numArray;
                    this._rijndael = rijndaelManaged;
                }
                return this._rijndael;
            }

            private byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
            {
                return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            }

            private byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
            {
                return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }
        }
    }
}