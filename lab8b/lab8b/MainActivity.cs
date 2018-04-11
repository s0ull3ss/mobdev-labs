using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using System;
using System.Globalization;
using System.Security.Cryptography;

namespace lab8b
{
    [Activity(Label = "lab8b", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button load;
        Button save;
        Button clear;
        EditText sourceText;
        String filename1 = "encryptedtext.txt";
        string path;
        string fileNameCrypted;

        byte[] keyBytes;
        byte[] iVBytes;
        TextView encryptedText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            sourceText = FindViewById<EditText>(Resource.Id.editText);
            load = FindViewById<Button>(Resource.Id.Load);
            save = FindViewById<Button>(Resource.Id.Save);
            clear = FindViewById<Button>(Resource.Id.Clear);

            encryptedText = FindViewById<TextView>(Resource.Id.textviewEncrypted);

            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            fileNameCrypted = Path.Combine(path, filename1);
            string fileKeyPath = Path.Combine(path, "key");
            string fileIVPath = Path.Combine(path, "IV");

            load.Click += LoadClick;
            save.Click += SaveClick;
            clear.Click += ClearClick;

            if (!File.Exists(fileNameCrypted))
            {
                using (var streamWriter = new StreamWriter(fileNameCrypted, true))
                {
                    streamWriter.WriteLine("File: " + fileNameCrypted);
                }
            }
            if (!File.Exists(fileKeyPath) || !File.Exists(fileIVPath))
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

        private void ClearClick(object sender, EventArgs e)
        {
            sourceText.Text = "";
            encryptedText.Text = "";
        }

        private void SaveClick(object sender, EventArgs e)
        {         
            DateTime localDate = DateTime.Now;
            var culture = new CultureInfo("ru-RU");
            string tmp = string.Format("\n{0}: {1}", culture, localDate.ToString(culture));
            sourceText.Text += tmp;

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

        private void LoadClick(object sender, EventArgs e)
        {
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.Key = keyBytes;
            cipher.IV = iVBytes;
            string path = Application.Context.FilesDir.Path;
            string fileName = Path.Combine(path, fileNameCrypted);
            MemoryStream memoryStream = new MemoryStream();

            byte[] encryptedBytes = File.ReadAllBytes(fileName);
            string text = Convert.ToBase64String(encryptedBytes, 0, encryptedBytes.Length);
            encryptedText.Text = text;

            FileStream fileStreamInput = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            cipher = new RijndaelManaged();
            cipher.Key = keyBytes;
            cipher.IV = iVBytes;
            ICryptoTransform decryptor = cipher.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(fileStreamInput, decryptor, CryptoStreamMode.Read);
            cryptoStream.CopyTo(memoryStream);

            cryptoStream.Close();
            fileStreamInput.Close();
            sourceText.Text = System.Text.Encoding.Unicode.GetString(memoryStream.ToArray());

            memoryStream.Close();
        }


    }
}

