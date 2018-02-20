using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using System.Collections.Generic;
using System;
using Android.Views;

namespace lab3
{
    public static class Intermediate
    {
        public static string fileName;
    }
    [Activity(Label = "lab3", MainLauncher = true)]
    public class MainActivity : Activity
    {
        string path;
        private ListView fileList;
        private List<string> fileNames = new List<string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var files = System.IO.Directory.GetFiles(path);
            if(files.Length <= 0)
            {
                File.WriteAllText(Path.Combine(path, "Res1"), "Res1text");
                File.WriteAllText(Path.Combine(path, "Res2"), "Res2text");
                File.WriteAllText(Path.Combine(path, "Res3"), "Res3text");
            }
            foreach(string file in files)
            {
                fileNames.Add(Path.GetFileName(file));
            }
            fileList = FindViewById<ListView>(Resource.Id.listViewFiles);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, fileNames);
            fileList.Adapter = adapter;
            fileList.ItemClick += ListViewItemClick;

            Button buttonAdd = FindViewById<Button>(Resource.Id.Add);
            buttonAdd.Click += ButtonAddClick;

            Button buttonDel = FindViewById<Button>(Resource.Id.Del);
            buttonDel.Click += ButtonDelClick;

            Button buttonRename = FindViewById<Button>(Resource.Id.Rename);
            buttonRename.Click += ButtonRenameClick; 

            Button buttonEdit = FindViewById<Button>(Resource.Id.Edit);
            buttonEdit.Click += ButtonEditClick; 
        }

        private void ButtonAddClick(object sender, EventArgs e)
        {
            StartActivity(typeof(PageAdd));
        }

        private void ButtonDelClick(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(Intermediate.fileName))
            {
                Toast.MakeText(this, "You must select a file", ToastLength.Long).Show();
            }
            else
            {
                File.Delete(Intermediate.fileName);
                Intermediate.fileName = null;
                Finish();
                StartActivity(typeof(MainActivity));
            }
        }

        private void ButtonRenameClick(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(Intermediate.fileName))
            {
                Toast.MakeText(this, "You must select a file", ToastLength.Long).Show();
            }
            else
            {
                StartActivity(typeof(PageRename));
            }
        }

        private void ButtonEditClick(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(Intermediate.fileName))
            {
                Toast.MakeText(this, "You must select a file", ToastLength.Long).Show();
            }
            else
            {
                StartActivity(typeof(Page2));
            }
        }

        private void ListViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, Path.Combine(path, fileNames[e.Position]), ToastLength.Long).Show();
            Intermediate.fileName = Path.Combine(path, fileNames[e.Position]);
        }
    }
}

