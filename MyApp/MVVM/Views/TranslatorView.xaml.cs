using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using System.Net;
using System.Web;

namespace MyApp.MVVM.Views;

public partial class TranslatorView : ContentPage
{
    public string lang_first = "en";
    public string lang_second = "vi";
    public string inputEntry = "";

    public TranslatorView()
    {
        InitializeComponent();
    }

    public string TranslateText(string input, string lang_first, string lang_second)
    {
        var fromLanguage = lang_first;
        var toLanguage = lang_second;
        var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(input)}";
        var webclient = new WebClient
        {
            Encoding = System.Text.Encoding.UTF8
        };
        var result = webclient.DownloadString(url);
        try
        {
            result = result.Substring(4, result.IndexOf("\"", 4
                , StringComparison.Ordinal) - 4);
            return result;
        }
        catch (Exception e1)
        {
            return "error";
        }
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Alert", "copy click", "ok");
    }

    private void twoArrowBtn_Clicked(object sender, EventArgs e)
    {
        string temp1 = upLabel.Text;
        upLabel.Text = downLabel.Text;
        downLabel.Text = temp1;


        string temp2 = firstBtn.Text;
        firstBtn.Text = secondBtn.Text;
        secondBtn.Text = temp2;

        string temp3 = lang_first;
        lang_first = lang_second;
        lang_second = temp3;
    }

    private async void CustomEntry_Completed(object sender, EventArgs e)
    {
        inputEntry = ((Entry)sender).Text;

        // Sử dụng thuật toán thời gian đợi luỹ thừa với hàm RetryWithExponentialBackoff
        bool translationSuccess = await RetryWithExponentialBackoff(async () =>
        {
            // Thực hiện cuộc gọi API dịch
            outputEntry.Text = TranslateText(inputEntry, lang_first, lang_second);

            // Kiểm tra kết quả và trả về true nếu thành công
            return !string.IsNullOrEmpty(outputEntry.Text);
        });

        if (!translationSuccess)
        {
            // Xử lý khi không thể dịch sau số lần thử lại tối đa
            DisplayAlert("Translation Error", "Translation failed after multiple retries.", "OK");
        }
    }

    public async Task<bool> RetryWithExponentialBackoff(Func<Task<bool>> action, int maxRetries = 5)
    {
        int retries = 0;

        while (retries < maxRetries)
        {
            try
            {
                // Thực hiện hành động
                bool result = await action();

                // Nếu hành động thành công, trả về kết quả
                if (result)
                    return true;
            }
            catch (Exception)
            {
                // Xử lý lỗi (có thể log hoặc thực hiện các thao tác khác)
            }

            // Tính thời gian chờ giữa các lần thử lại
            int waitTime = (int)Math.Pow(2, retries) * 1000 + new Random().Next(1000);

            // Chờ thời gian xác định trước khi thử lại
            await Task.Delay(waitTime);

            retries++;
        }

        // Nếu không thành công sau số lần thử lại tối đa, trả về false
        return false;
    }
}