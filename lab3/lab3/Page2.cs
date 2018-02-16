using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace lab3
{
    [Activity(Label = "Page2")]
    public class Page2 : Activity
    {
        EditText editText;
        Button buttonSave;
        Button buttonGoToMainPage;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Page2);
            editText = FindViewById<EditText>(Resource.Id.editText);
            using (var streamReader = new StreamReader(Intermediate.fileName))
            {
                string content = streamReader.ReadToEnd();
                editText.Text = content;
            }
            buttonGoToMainPage = FindViewById<Button>(Resource.Id.gotoMainPage);
            buttonGoToMainPage.Click += delegate
            {
                Intermediate.fileName = null;
                StartActivity(typeof(MainActivity));
            };
            buttonSave = FindViewById<Button>(Resource.Id.Save);
            buttonSave.Click += SaveClick;
        }

        private void SaveClick(object sender, EventArgs e)
        {
            using (var streamWriter = new StreamWriter(Intermediate.fileName, false))
            {
                Toast.MakeText(this, "The file is saved", ToastLength.Long).Show();
                streamWriter.Write(editText.Text.ToCharArray());
            }
        }
    }
}