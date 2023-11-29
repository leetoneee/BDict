using System.ComponentModel;
using System.Net;
using System.Web;
using System.Windows.Input;

namespace MyApp.MVVM.ViewModels
{
    public class TranslatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _input = string.Empty;
        public string Input
        {
            get { return _input; }
            set
            {
                if (_input != value)
                {
                    _input = value;
                    OnPropertyChanged(nameof(Input));
                }
            }
        }

        private string _output = string.Empty;
        public string Output
        {
            get { return _output; }
            set
            {
                if (_output != value)
                {
                    _output = value;
                    OnPropertyChanged(nameof(Output));
                }
            }
        }

        private string _langFirst = string.Empty;
        public string LangFirst
        {
            get { return _langFirst; }
            set
            {
                if (_langFirst != value)
                {
                    _langFirst = value;
                    OnPropertyChanged(nameof(LangFirst));
                }
            }
        }

        private string _langSecond = string.Empty;
        public string LangSecond
        {
            get { return _langSecond; }
            set
            {
                if (_langSecond != value)
                {
                    _langSecond = value;
                    OnPropertyChanged(nameof(LangSecond));
                }
            }
        }

        private string _lang_first = "en";
        private string _lang_second = "vi";

        public ICommand ConvertCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand CallAPI { get; }

        public TranslatorViewModel()
        {
            LangFirst = "English";
            LangSecond = "Vietnamese";
            ConvertCommand = new Command(twoArrowBtn_Clicked);
            CopyCommand = new Command(copyBtn_Clicked);
            CallAPI = new Command(async () => await CustomEntry_Completed());
        }

        private void twoArrowBtn_Clicked()
        {
            string temp = LangFirst;
            LangFirst = LangSecond;
            LangSecond = temp;

            string temp1 = _lang_first;
            _lang_first = _lang_second;
            _lang_second = temp1;
        }

        private void copyBtn_Clicked()
        {
            Application.Current.MainPage.DisplayAlert("Alert", "Copy click", "OK");
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
            catch (Exception)
            {
                return "error";
            }
        }

        private async Task CustomEntry_Completed()
        {
            // Sử dụng thuật toán thời gian đợi luỹ thừa với hàm RetryWithExponentialBackoff
            bool translationSuccess = await RetryWithExponentialBackoff(async () =>
            {
                // Thực hiện cuộc gọi API dịch
                Output = TranslateText(Input, _lang_first, _lang_second);

                // Kiểm tra kết quả và trả về true nếu thành công
                return !string.IsNullOrEmpty(Output);
            });

            if (!translationSuccess)
            {
                // Xử lý khi không thể dịch sau số lần thử lại tối đa
                Application.Current.MainPage.DisplayAlert("Translation Error", "Translation failed after multiple retries.", "OK");
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
}
