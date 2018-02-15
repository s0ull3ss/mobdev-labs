using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace lab1
{
    [Activity(Label = "lab1", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Button buttonCalculate = FindViewById<Button>(Resource.Id.buttonCalculate);
            buttonCalculate.Click += CalculateRoots;
        }

        private void CalculateRoots(object sender, EventArgs e)
        {
            decimal a = string.IsNullOrEmpty(FindViewById<EditText>(Resource.Id.editTextA).Text) ? 0 : decimal.Parse(FindViewById<EditText>(Resource.Id.editTextA).Text);
            decimal b = string.IsNullOrEmpty(FindViewById<EditText>(Resource.Id.editTextB).Text) ? 0 : decimal.Parse(FindViewById<EditText>(Resource.Id.editTextB).Text);
            decimal c = string.IsNullOrEmpty(FindViewById<EditText>(Resource.Id.editTextC).Text) ? 0 : decimal.Parse(FindViewById<EditText>(Resource.Id.editTextC).Text);
            decimal d = (decimal)Math.Pow((Double)b, 2) - 4 * a * c;
            decimal x1r, x1i, x2r;
            TextView result = FindViewById<TextView>(Resource.Id.textViewResult);
            if (d >= 0 && a != 0)
            {
                x1r = (-b + (decimal)Math.Sqrt((double)d)) / (2 * a);
                x2r = (-b - (decimal)Math.Sqrt((double)d)) / (2 * a);
                result.Text = string.Format("x1 = {0}\nx2 = {1}", x1r, x2r);
            }
            else if (d < 0 && a != 0)
            {
                x1r = -b / 2 * a;
                x1i = Math.Abs((decimal)Math.Sqrt((double)Math.Abs(d)) / (2 * a));
                result.Text = string.Format("x1 = {0} + {1}i\nx2 = {0} - {1}i", x1r, x1i);
            }
            else if (a == 0 && b != 0)
            {
                x1r = -c / b;
                result.Text = string.Format("x1 = {0}", x1r);
            }
            else if (a == 0 && b == 0)
            {
                result.Text = string.Format("Error");
            }
        }
    }
}

