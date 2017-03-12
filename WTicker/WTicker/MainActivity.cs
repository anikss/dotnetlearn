using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Timer = System.Timers.Timer;


namespace WTicker
{
    [Activity(Label = "WTicker", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Timer timer;
        private string wavesUrl = "https://bittrex.com/api/v1.1/public/getmarketsummary?market=btc-waves";
        private double high;
        private double low;
        private double timerSet = 5000;
        private bool check = true;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            EditText editHigh = FindViewById<EditText>(Resource.Id.editHigh);
            EditText editLow = FindViewById<EditText>(Resource.Id.editLow);
            EditText editTime = FindViewById<EditText>(Resource.Id.editTime);
            Button setHigh = FindViewById<Button>(Resource.Id.setHigh);
            Button setLow = FindViewById<Button>(Resource.Id.setLow);
            Button setTime = FindViewById<Button>(Resource.Id.setTime);


            //editTime.SetText(0);//(timerSet / 1000).ToString(CultureInfo.InvariantCulture);
            setHigh.Click += (object sender, EventArgs e) =>
            {
                high = Convert.ToDouble(editHigh.Text, CultureInfo.InvariantCulture);
                setHigh.Text = Convert.ToString(high, CultureInfo.InvariantCulture);
                check = true;
            };

            setLow.Click += (object sender, EventArgs e) =>
            {
                low = Convert.ToDouble(editLow.Text, CultureInfo.InvariantCulture);
                setLow.Text = Convert.ToString(low, CultureInfo.InvariantCulture);
                check = true;
            };

            setTime.Click += (object sender, EventArgs e) =>
            {
                timerSet = Convert.ToDouble(editTime.Text, CultureInfo.InvariantCulture)*1000;
                setTime.Text = Convert.ToString((timerSet/1000), CultureInfo.InvariantCulture)+" sec";
            };

            RunUpdateLoop();

        }
        protected override void OnResume()
        {       
            base.OnResume();
            Download(wavesUrl);
        }
        protected override void OnPause()
        {
            base.OnPause();
            RunUpdateLoop();
        }

        private async void RunUpdateLoop()
        {
            while (true)
            {
                await Task.Delay((int) timerSet);
                Download(wavesUrl);
            }
        }
        private async void Download(string url)
        {
            var r = await GetAsync(url);
            check = true;
            IList<JToken> results = r["result"].Children().ToList();
            IList<Result> resultsData = new List<Result>();
            foreach (JToken resultInd in results)
            {
                Result result = JsonConvert.DeserializeObject<Result>(resultInd.ToString());
                resultsData.Add(result);
            }
            RenewResults(resultsData);
            Button setHigh = FindViewById<Button>(Resource.Id.setHigh);
            Button setLow = FindViewById<Button>(Resource.Id.setLow);
            if (((Convert.ToDouble(resultsData[0].Last, CultureInfo.InvariantCulture) >= high) || (Convert.ToDouble(resultsData[0].Last, CultureInfo.InvariantCulture) <= low)) && ((setHigh.Text!="Set")||(setLow.Text!="Set"))&&(check=true))
            {
                string mainNotifi = string.Format("Last price: " + Convert.ToString(resultsData[0].Last,
                    CultureInfo.InvariantCulture) + " BTC.");
                string notifi = string.Format("High alert: " + Convert.ToString(high, CultureInfo.InvariantCulture) + 
                    "\n" + "Low alert: " + Convert.ToString(low, CultureInfo.InvariantCulture));
                Notification(mainNotifi, notifi);
                check = false;
                setHigh.Text = "Set";
                setLow.Text = "Set";
            }
        }
        private static async Task<JObject> GetAsync(string uri)
        {
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri);
            return await Task.Run(() => JObject.Parse(content));
        }
        private void RenewResults (IList<Result> resultsData)
        {
            TextView marketName = FindViewById<TextView>(Resource.Id.marketName);
            marketName.Text = resultsData[0].MarketName;

            TextView timeStapm = FindViewById<TextView>(Resource.Id.timeStamp);
            timeStapm.Text = resultsData[0].TimeStamp;

            TextView volumeView = FindViewById<TextView>(Resource.Id.volumeView);
            volumeView.Text = resultsData[0].Volume;

            TextView baseVolumeView = FindViewById<TextView>(Resource.Id.baseVolumeView);
            baseVolumeView.Text = resultsData[0].BaseVolume;

            TextView highView = FindViewById<TextView>(Resource.Id.highView);
            highView.Text = resultsData[0].High;

            TextView lowView = FindViewById<TextView>(Resource.Id.lowView);
            lowView.Text = resultsData[0].Low;

            TextView bidView = FindViewById<TextView>(Resource.Id.bidView);
            bidView.Text = resultsData[0].Bid;

            TextView askView = FindViewById<TextView>(Resource.Id.askView);
            askView.Text = resultsData[0].Ask;

            TextView lastPriceView = FindViewById<TextView>(Resource.Id.lastPriceView);
            lastPriceView.Text = resultsData[0].Last;

        }
        private void Notification(string mainNotifi, string notifi)
        {
            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent(this, typeof(MainActivity));

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);

            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(this)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(mainNotifi)
                .SetContentText(notifi)
                .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                .SetSmallIcon(Resource.Drawable.ic_notification);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
    }
}


// var callDialog = new AlertDialog.Builder(this);
// callDialog.SetMessage(Convert.ToString(high));
//callDialog.SetNegativeButton("Cancel", delegate { });

// Show the alert dialog to the user and wait for response.
//  callDialog.Show();