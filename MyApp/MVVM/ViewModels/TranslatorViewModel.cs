using System.ComponentModel;
using System.Net;
using System.Net.Http;
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

        //private string wordCount;
        //public string WordCount
        //{
        //    get
        //    {
        //        return wordCount;
        //    }
        //    set
        //    {
        //        wordCount = value;
        //        OnPropertyChanged(WordCount);
        //    }
        //}


        public ICommand ConvertCommand { get; }
        public ICommand CopyCommand1 { get; }
        public ICommand CopyCommand2 { get; }
        public ICommand CallAPI { get; }
        public ICommand ClearCommand { get; }

        //public ICommand TextChangedCommand { get; }

        public TranslatorViewModel()
        {
            LangFirst = "ENGLISH";
            LangSecond = "VIETNAMESE";
            ConvertCommand = new Command(twoArrowBtn_Clicked);
            CopyCommand1 = new Command(copyBtn1_Clicked);
            CopyCommand2 = new Command(copyBtn2_Clicked);
            CallAPI = new Command(async () => await CustomEntry_Completed());
            ClearCommand = new Command(clearBtn_Clicked);
            //TextChangedCommand = new Command(OnTextChanged);
        }

        //private void OnTextChanged()
        //{
        //    OnPropertyChanged(nameof(WordCount));
        //}
        private string CountWords(string text)
        {
            var words = text.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length.ToString();
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

        private void copyBtn1_Clicked()
        {
            try
            {
                Clipboard.SetTextAsync(Input);

                Application.Current.MainPage.DisplayAlert("Alert", "Text copied to clipboard", "OK");
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", $"Failed to copy text to clipboard: {ex.Message}", "OK");
            }
        }
        private void copyBtn2_Clicked()
        {
            try
            {
                Clipboard.SetTextAsync(Output);

                Application.Current.MainPage.DisplayAlert("Alert", "Text copied to clipboard", "OK");
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", $"Failed to copy text to clipboard: {ex.Message}", "OK");
            }
        }

        private void clearBtn_Clicked()
        {
            Input = string.Empty;
        }

        public string TranslateText(string input, string lang_first, string lang_second)
        {
            var fromLanguage = lang_first;
            var toLanguage = lang_second;
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(input)}";
#pragma warning disable SYSLIB0014 // Type or member is obsolete
            var webclient = new WebClient
            {
                Encoding = System.Text.Encoding.UTF8
            };
#pragma warning restore SYSLIB0014 // Type or member is obsolete
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
            bool translationSuccess = await RetryWithExponentialBackoff(async () =>
            {
                Output = TranslateText(Input, _lang_first, _lang_second);

                return !string.IsNullOrEmpty(Output);
            });

            if (!translationSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Translation Error", "Translation failed after multiple retries.", "OK");
            }
        }

        public async Task<bool> RetryWithExponentialBackoff(Func<Task<bool>> action, int maxRetries = 5)
        {
            int retries = 0;

            while (retries < maxRetries)
            {
                try
                {
                    bool result = await action();

                    if (result)
                        return true;
                }
                catch (Exception)
                {
                }

                int waitTime = (int)Math.Pow(2, retries) * 1000 + new Random().Next(1000);

                await Task.Delay(waitTime);

                retries++;
            }

            return false;
        }
    }
}
