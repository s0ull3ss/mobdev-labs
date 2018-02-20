using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Media;
using Android.Runtime;

namespace lab4
{
    [Activity(Label = "lab4", MainLauncher = true)]
    public class MainActivity : Activity, ISensorEventListener
    {
        SensorManager sensorManager;
        Android.Media.MediaPlayer mp;
        bool play = false;

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            //throw new System.NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (!mp.IsPlaying)
            {
                mp.Start();
            }
            else
            {
                mp.Pause();
            }
            //throw new System.NotImplementedException();
        }

        protected override void OnResume()
        {
            base.OnResume();
            sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.StepDetector), SensorDelay.Ui);
        }

        protected override void OnPause()
        {
            base.OnPause();
            sensorManager.UnregisterListener(this);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            sensorManager = (SensorManager)GetSystemService(SensorService);
            mp = MediaPlayer.Create(this, Resource.Raw.test);
        }
    }
}

