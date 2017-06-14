﻿using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;

namespace TestAndorid
{
    [Activity(Label = "TestAndorid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView text1;
        Task _timer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.button1);

            button.Click += button1_onClick;

            text1 = FindViewById<TextView>(Resource.Id.textView1);

            _timer = new Task(async () =>
            {
                while (true)
                {
                    RunOnUiThread(() =>
                    {
                        text1.Text = DateTime.Now.ToString("HH:mm:ss");
                    }
                    );
                    await Task.Delay(1000);
                }
            });
            _timer.Start();

        }

        private void button1_onClick(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Hello,Xamarin.Android", ToastLength.Long).Show();
        }

        // CSVのデータを格納するクラス
        class Record
        {
            public string Date { get; set; }
            public string ArrivalTime { get; set; }
            public string LeaveTime { get; set; }
            public string Note { get; set; }
        }

        // 格納するルール
        class CsvMapper : CsvHelper.Configuration.CsvClassMap<Record>
        {
            public CsvMapper()
            {
                Map(x => x.Date).Index(0);
                Map(x => x.ArrivalTime).Index(1);
                Map(x => x.LeaveTime).Index(2);
                Map(x => x.Note).Index(3);
            }
        }

        public void ReadCSV()
        {
			using (var r = new StreamReader("sample.csv", Encoding.GetEncoding("SHIFT_JIS")))
			using (var csv = new CsvHelper.CsvReader(r))
			{
				// ヘッダーはないCSV
				csv.Configuration.HasHeaderRecord = false;
				// 先ほど作ったマッピングルールを登録
				csv.Configuration.RegisterClassMap<CsvMapper>();
				// データを読み出し
				var records = csv.GetRecords<Record>();

				// 出力
				foreach (var record in records)
				{
					Console.WriteLine("{0}/{1}/{2}", record.Date, record.ArrivalTime, record.LeaveTime);
				}
			}
        }
    }

}