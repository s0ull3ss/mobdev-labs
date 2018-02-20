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

namespace lab5
{
    [Activity(Label = "lab5", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button httpRequest;
        TextView responseView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            responseView = FindViewById<TextView>(Resource.Id.textView1);
            httpRequest = FindViewById<Button>(Resource.Id.button1);
            httpRequest.Click += HttpRequestClick;
        }

        private void HttpRequestClick(object sender, EventArgs e)
        {
            var rxcui = "198440";
            var request = HttpWebRequest.Create(string.Format(@"http://rxnav.nlm.nih.gov/REST/RxTerms/rxcui/{0}/allinfo", rxcui));
            request.ContentType = "application/json";
            request.Method = "GET";
            HttpWebResponse response;
            using (response = request.GetResponse() as HttpWebResponse)
            {
                if(response.StatusCode != HttpStatusCode.OK)
                {
                    responseView.Text = string.Format("Error. Status code: {0}", response.StatusCode);
                }
                else
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        responseView.Text = Regex.Match(content, "<displayName>(.*?)</displayName>").Groups[1].Value.ToString();
                    }
                }
            }
            //throw new NotImplementedException();
        }
    }
}

