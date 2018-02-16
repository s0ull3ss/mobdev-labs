﻿using System;
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
    [Activity(Label = "PageRename")]
    public class PageRename : Activity
    {
        EditText editTextRename;
        Button buttonSave;
        Button buttonGoToMainPage;
        string path;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.PageRename);
            editTextRename = FindViewById<EditText>(Resource.Id.editTextRename);
            editTextRename.Text = Intermediate.fileName;

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
            //Not realised
            string filename = FindViewById<EditText>(Resource.Id.editTextRename).Text;
            if (string.IsNullOrEmpty(filename) || string.IsNullOrWhiteSpace(filename))
            {
                Toast.MakeText(this, "The file is not saved", ToastLength.Long).Show();
                Toast.MakeText(this, "The file name must be non-empty", ToastLength.Long).Show();
            }
            else
            {
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                File.WriteAllText(Path.Combine(path, filename), filename);
                Toast.MakeText(this, "The file is saved", ToastLength.Long).Show();
            }
        }
    }
}