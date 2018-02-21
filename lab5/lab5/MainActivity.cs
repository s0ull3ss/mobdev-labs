using Android.App;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace lab5
{
    [Activity(Label = "lab5", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button httpRequest;
        ListView drugInfoListView;
        List<string> drugInfo = new List<string>();
        ArrayAdapter<string> adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            httpRequest = FindViewById<Button>(Resource.Id.button1);
            httpRequest.Click += HttpRequestClick;

            drugInfoListView = FindViewById<ListView>(Resource.Id.listViewInfo);
        }

        private void HttpRequestClick(object sender, EventArgs e)
        {
            var rxcui = "198440";
            var request = HttpWebRequest.Create(string.Format(@"http://rxnav.nlm.nih.gov/REST/RxTerms/rxcui/{0}/allinfo", rxcui));
            request.ContentType = "application/json";
            request.Method = "GET";
            HttpWebResponse response;
            try
            {
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        string err = string.Format("Error:\n{0}", response.StatusCode);
                        Toast.MakeText(this, err, ToastLength.Long).Show();
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            var content = reader.ReadToEnd();
                            drugInfo.Clear();
                            drugInfo.Add("Name: " + Regex.Match(content, "<displayName>(.*?)</displayName>").Groups[1].Value.ToString());
                            drugInfo.Add("Synonym: " + Regex.Match(content, "<synonym>(.*?)</synonym>").Groups[1].Value.ToString());
                            drugInfo.Add("Route: " + Regex.Match(content, "<route>(.*?)</route>").Groups[1].Value.ToString());
                            drugInfo.Add("Strength: " + Regex.Match(content, "<strength>(.*?)</strength>").Groups[1].Value.ToString());
                            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, drugInfo);
                            drugInfoListView.Adapter = adapter;
                        }
                    }
                }
            }
            catch(Exception exc)
            {
                string err = string.Format("Error:\n{0}", exc.ToString());
                Toast.MakeText(this, err, ToastLength.Long).Show();
            }
        }
    }
}

