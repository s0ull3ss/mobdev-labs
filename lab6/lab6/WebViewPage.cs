using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace lab6
{
    [Activity(Label = "WebViewPage")]
    public class WebViewPage : Activity
    {
        WebView wv;
        Button home;
        Button back;
        Button forward;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WebViewPage);
            // Create your application here

            home = FindViewById<Button>(Resource.Id.home);
            back = FindViewById<Button>(Resource.Id.back);
            forward = FindViewById<Button>(Resource.Id.forward);
            home.Click += HomeClick;
            back.Click += BackClick;
            forward.Click += ForwardClick;
            //WebView init
            wv = FindViewById<WebView>(Resource.Id.webView1);
            wv.SetWebViewClient(new WebViewClient());
            wv.Settings.JavaScriptEnabled = true;
            wv.LoadUrl(Intermediate.URLNews);

        }

        private void ForwardClick(object sender, EventArgs e)
        {
            if (wv.CanGoForward())
            {
                wv.GoForward();
            }
        }

        private void BackClick(object sender, EventArgs e)
        {
            if (wv.CanGoBack())
            {
                wv.GoBack();
            }
        }

        private void HomeClick(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}