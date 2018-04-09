using Android.App;
using Android.Widget;
using Android.OS;

using System.Security.Cryptography;
using System.IO;
using System;

namespace lab8
{
    [Activity(Label = "lab8", MainLauncher = true)]
    public class MainActivity : Activity
    {
        byte[] keyBytes;
        byte[] iVBytes;

        EditText sourceText;
        TextView encryptedText;
        TextView decryptedText;
        Button buttonEncrypt;
        Button buttonDecrypt;

        string fileNameCrypted = "file1.crypted";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            sourceText = FindViewById<EditText>(Resource.Id.editText);
            encryptedText = FindViewById<TextView>(Resource.Id.textviewEncrypted);
            decryptedText = FindViewById<TextView>(Resource.Id.textviewDecrypted);
            buttonEncrypt = FindViewById<Button>(Resource.Id.buttonEncrypt);
            buttonDecrypt = FindViewById<Button>(Resource.Id.buttonDecrypt);

            buttonEncrypt.Click += ButtonEncrypt_Click;
            buttonDecrypt.Click += ButtonDecrypt_Click;

            string path = Application.Context.FilesDir.Path;
            string fileKeyPath = Path.Combine(path, "key");
            string fileIVPath = Path.Combine(path, "IV");

            if(!File.Exists(fileKeyPath) || !File.Exists(fileIVPath))
            {
                var algorithm = new RijndaelManaged();

                int bytesForKey = algorithm.KeySize / 8;
                int bytesForIV = algorithm.BlockSize / 8;

                keyBytes = new byte[bytesForKey];
                iVBytes = new byte[bytesForIV];
                new Random().NextBytes(keyBytes);
                new Random().NextBytes(iVBytes);
                File.WriteAllBytes(fileKeyPath, keyBytes);
                File.WriteAllBytes(fileIVPath, iVBytes);
            }
            else
            {
                keyBytes = File.ReadAllBytes(fileKeyPath);
                iVBytes = File.ReadAllBytes(fileIVPath);
            }

        }

        private void ButtonEncrypt_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(sourceText.Text))
            {
                throw new Exception("Source text is empty");
            }
            byte[] DataToEncrypt = System.Text.Encoding.Unicode.GetBytes(sourceText.Text);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.Key = keyBytes;
            cipher.IV = iVBytes;

            string path = Application.Context.FilesDir.Path;
            string fileName = Path.Combine(path, fileNameCrypted);

            FileStream fileStreamOutput = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            ICryptoTransform encryptor = cipher.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(fileStreamOutput, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(DataToEncrypt, 0, DataToEncrypt.Length);

            cryptoStream.Close();
            fileStreamOutput.Close();

            byte[] encryptedBytes = File.ReadAllBytes(fileName);
            string text = Convert.ToBase64String(encryptedBytes, 0, encryptedBytes.Length);
            encryptedText.Text = text;
    
        }

        private void ButtonDecrypt_Click(object sender, EventArgs e)
        {
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.Key = keyBytes;
            cipher.IV = iVBytes;

            string path = Application.Context.FilesDir.Path;
            string fileName = Path.Combine(path, fileNameCrypted);

            MemoryStream memoryStream = new MemoryStream();

            FileStream fileStreamInput = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            cipher = new RijndaelManaged();
            cipher.Key = keyBytes;
            cipher.IV = iVBytes;
            ICryptoTransform decryptor = cipher.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(fileStreamInput, decryptor, CryptoStreamMode.Read);
            cryptoStream.CopyTo(memoryStream);

            cryptoStream.Close();
            fileStreamInput.Close();
            decryptedText.Text = System.Text.Encoding.Unicode.GetString(memoryStream.ToArray());
            memoryStream.Close();
        }
    }
}

