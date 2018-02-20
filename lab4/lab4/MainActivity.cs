using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Media;
using Android.Runtime;
using static Android.Widget.SeekBar;

namespace lab4
{
    [Activity(Label = "lab4", MainLauncher = true)]
    public class MainActivity : Activity, ISensorEventListener, IOnSeekBarChangeListener
    {
        SensorManager sensorManager;
        Android.Media.MediaPlayer mp;
        TextView textView;
        float volume;


        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
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
            volume = 0.5f;
            mp.SetVolume(volume, volume);

            SeekBar seekBar = FindViewById<SeekBar>(Resource.Id.seekBar);
            seekBar.SetOnSeekBarChangeListener(this);
            seekBar.Progress =  (int)(volume * 100);

            textView = FindViewById<TextView>(Resource.Id.textView);
            textView.Text = string.Format("Volume {0}\nProgress {1}", volume, seekBar.Progress); 
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            volume = seekBar.Progress / 100f;
            mp.SetVolume(volume, volume);
            textView.Text = string.Format("Volume {0}\nProgress {1}", volume, seekBar.Progress); 
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
        }
    }
}

