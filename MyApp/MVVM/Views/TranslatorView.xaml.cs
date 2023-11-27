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

namespace MyApp.MVVM.Views;

public partial class TranslatorView : ContentPage
{
    public string lang_first = "en";
    public string lang_second = "vi";
    public string inputEntry = "";
    HttpClient httpClient; 

    public TranslatorView()
	{
		InitializeComponent();
        httpClient = new HttpClient();
    }

    //public string TranslateText(string input)
    //{
    //    string url = String.Format
    //    ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
    //     lang_first, lang_second, Uri.EscapeUriString(input));
    //    HttpClient httpClient = new HttpClient();
    //    string result = httpClient.GetStringAsync(url).Result;
    //    var jsonData = JsonConvert.DeserializeObject<List<dynamic>>(result); var translationItems = jsonData[0];
    //    string translation = "";
    //    foreach (object item in translationItems)
    //    {
    //        IEnumerable translationLineObject = item as IEnumerable;
    //        IEnumerator translationLineString = translationLineObject.GetEnumerator();
    //        translationLineString.MoveNext();
    //        translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
    //    }
    //    if (translation.Length > 1) { translation = translation.Substring(1); };
    //    return translation;
    //}

    public string TranslateText(string input, string lang_first, string lang_second)
    {
        // tạo link để gọi API
        string url = String.Format
        ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
         lang_first, lang_second, Uri.EscapeDataString(input));
        // gọi API và lấy kết quả trả về
        string result = httpClient.GetStringAsync(url).Result;

        // trích xuất thông tin từ kiểu dữ liệu json được trả về
        var jsonData = JsonConvert.DeserializeObject<List<dynamic>>(result);
        var translationItems = jsonData[0];
        string translation = "";
        foreach (object item in translationItems)
        {
            IEnumerable<dynamic> translationLineObject = item as IEnumerable<dynamic>;
            IEnumerator<dynamic> translationLineString = translationLineObject.GetEnumerator();
            translationLineString.MoveNext();
            translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
        }
        if (translation.Length > 1) { translation = translation.Substring(1); };
        return translation;
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