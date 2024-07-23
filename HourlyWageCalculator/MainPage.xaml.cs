using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;

namespace HourlyWageCalculator
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public List<int>? Hours { get; set; }
        public List<int>? Minutes { get; set; }

        public MainPage()
        {
            InitializeComponent();
            InitializePickers();
            BindingContext = this;
        }

        private void InitializePickers()
        {
            Hours = [];
            for (int i = 0; i <= 23; i++)
            {
                Hours.Add(i);
            }

            Minutes = [];
            for (int i = 0; i <= 59; i++)
            {
                Minutes.Add(i);
            }

            OnPropertyChanged(nameof(Hours));
            OnPropertyChanged(nameof(Minutes));
        }

        private async void SaveBtn(object sender, EventArgs e)
        {
            bool result = await DisplayAlert(null, "保存しました", "OK", "履歴を確認");
            if (!result)
            {
                await Shell.Current.GoToAsync("SubMain");
            }
        }

        private void OnCalculateClicked(object sender, EventArgs e)
        {
            // 時給の取得
            if (!decimal.TryParse(HourlyWageEntry.Text, out decimal hourlyWage))
            {
                DisplayAlert("エラー", "有効な時給を入力してください。", "OK");
                return;
            }

            // 開始時間の取得
            if (StartHourPicker.SelectedIndex == -1 && StartMinutePicker.SelectedIndex == -1)
            {
                DisplayAlert("エラー", "有効な開始時間を選択してください。", "OK");
                return;
            } 

            // 終了時間の取得
            if (EndHourPicker.SelectedIndex == -1 && EndMinutePicker.SelectedIndex == -1)
            {
                DisplayAlert("エラー", "有効な終了時間を選択してください。", "OK");
                return;
            }

            // 時間が未入力の場合は0を代入
            if (StartHourPicker.SelectedIndex == -1) StartHourPicker.SelectedItem = 0;
            if (StartMinutePicker.SelectedIndex == -1) StartMinutePicker.SelectedItem = 0;
            if (EndHourPicker.SelectedIndex == -1) EndHourPicker.SelectedItem = 0;
            if (EndMinutePicker.SelectedIndex == -1) EndMinutePicker.SelectedItem = 0;
            if (!decimal.TryParse(SuperTime.Text, out decimal superTime)) superTime = 0;

            // 入力値をint型にキャストして変数に格納
            int startHour = (int)StartHourPicker.SelectedItem;
            int startMinute = (int)StartMinutePicker.SelectedItem;
            int endHour = (int)EndHourPicker.SelectedItem;
            int endMinute = (int)EndMinutePicker.SelectedItem;

            TimeSpan startTime = new (startHour, startMinute, 0);
            TimeSpan endTime = new (endHour, endMinute, 0);
            TimeSpan delayTime = new (0, (int)superTime, 0);

            // 勤務時間の計算
            TimeSpan workDuration = endTime - startTime - delayTime;

            if (workDuration < TimeSpan.Zero)
            {
                DisplayAlert("エラー", "終了時間は開始時間より後でなければなりません。", "OK");
                return;
            }

            // 総賃金の計算
            decimal totalWage = hourlyWage * (decimal)workDuration.TotalHours;

            // 結果の表示
            ResultLabel.Text = "総賃金:"+ $"￥{(int)totalWage}";
            TimeRangeLabel.Text = $"{startHour:D2}:{startMinute:D2} ～ {endHour:D2}:{endMinute:D2}";
            WorkingTime.Text = "実働時間:" + $"{workDuration}";
        }
    }
}
