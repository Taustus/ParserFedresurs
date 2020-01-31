using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Web;

namespace ParserFedresource
{
    public partial class BrowserForm : Form
    {
        ChromiumWebBrowser browser;
        public BrowserForm()
        {
            InitializeComponent();
            Location = new Point(1500, 1500);
            InitializeChromium();
        }

        private void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);
            browser = new ChromiumWebBrowser();
            Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            DesktopLocation = new Point(1500, 1500);
        }

        
        public async Task<string> Algorithm(CancellationToken cts, string forSearch, bool beginning)
        {
            Writer writer = new Writer();

            string baseUrl = "https://fedresurs.ru/search/encumbrances?searchString="+HttpUtility.HtmlEncode(forSearch)+"&group=All&additionalSearchFnp=true";

            await LoadPageAsync(browser, baseUrl);
            string script = string.Format("document.getElementsByClassName('search-count-block')[0].innerText;");
            string pages = await EvaluateScript(script);

            int start = 0;
            if(beginning && forSearch.Equals(writer.LastString))
            {
                try
                {
                    start = writer.PageNum;
                }
                catch { }
            }
            for (int i = start; i < Convert.ToInt32(pages); i++)
            {
                if (cts.IsCancellationRequested)
                {
                    writer.Finish();
                    throw new Exception("Парсинг остановлен!");
                }
                //Before going to any page we should upload all of them
                await EvaluateScript("window.scrollTo(0,document.body.scrollHeight);");
                await EvaluateScript("var loadMore = setInterval(function() {" +
                    $"if(document.getElementsByClassName('encumbrances-result__body')[{i}])" +
                    "{clearInterval(loadMore);}" +
                    "if (document.getElementsByClassName('btn btn_load_more').length != 0)" +
                    "{document.getElementsByClassName('btn btn_load_more')[0].click();}}, 3000)");
                //Taking part of url to add to base url
                string addition = await EvaluateScript($"document.getElementsByClassName('encumbrances-result__body')[{i}].children[0].children[0].getAttribute('href')");
                string url = "https://fedresurs.ru";
                while (true)
                {
                    if (addition.Equals("fuck"))
                    {
                        Thread.Sleep(1000);
                        addition = await EvaluateScript($"document.getElementsByClassName('encumbrances-result__body')[{i}].children[0].children[0].getAttribute('href')");
                    }
                    else
                    {
                        url += addition;
                        break;
                    }
                }
                await LoadPageAsync(browser, url);

                Info info = new Info();

                //Here script contains setInterval cuz we need to be sure that page was fully loaded
                info.Contract = await EvaluateScript
                (@"var checkExist = setInterval(function() {if (document.getElementsByClassName('container')[2].length) {clearInterval(checkExist);}}, 1000);document.getElementsByClassName('container')[2].children[0].children[0].innerText.split('\n')[0];");
                info.MessageNumber = await EvaluateScript(@"document.getElementsByClassName('container')[2].children[0].innerText.split('\n')[2].split('Сообщение ')[1]");
                info.URL = url;

                if (info.MessageNumber.Equals("fuck"))
                {
                    i--;
                    await LoadPageAsync(browser, baseUrl);
                    continue;
                }

                var data = await browser.GetTextAsync();
                var arr = data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                //I'm doing this stupid method cuz site fedresurs.ru is fucked(i mean nodes and other things)
                for (int k = 0; k < arr.Length; k++)
                {
                    //Lessor
                    if (arr[k].Contains("Залогодержатели"))
                    {
                        info.Lessor = arr[k + 2];
                        info.LessorINN = arr[k + 3].Substring(4);
                    }
                    if (arr[k].Contains("Лизингополучатели"))
                    {
                        info.Lessor = arr[k + 1];
                        info.LessorINN = arr[k + 2].Substring(4);
                    }
                    //Pledger
                    if (arr[k].Contains("Залогодатели"))
                    {
                        info.Pledger = arr[k + 2];
                        info.PledgerINN = arr[k + 3].Substring(4);
                    }
                    if (arr[k].Contains("Лизингодатели"))
                    {
                        info.Pledger = arr[k + 1];
                        info.PledgerINN = arr[k + 2].Substring(4);
                    }
                    //Other
                    if (arr[k].Contains("Договор:"))
                    {
                        info.ContractNumber = arr[k + 1];
                    }
                    if (arr[k].Contains("Документы"))
                    {
                        info.File = arr[k + 1];
                    }
                    if (arr[k].Contains("Описание:"))
                    {
                        try
                        {
                            if (arr[k + 3].Contains("Описание:"))
                            {
                                info.Description += arr[k + 1] + '\n';
                                info.Identifier += arr[k + 2] + '\n';
                            }
                            else
                            {
                                info.Description += arr[k + 1];
                            }
                        }
                        catch { }
                    }
                    if (arr[k].Contains("Срок финансовой аренды:"))
                    {
                        info.RentalPeriod = arr[k + 1];
                    }
                    if (arr[k].Contains("ИДЕНТИФИКАТОР") && arr[k].Contains("КЛАССИФИКАЦИЯ") && arr[k].Contains("КЛАССИФИКАЦИЯ"))
                    {
                        info.Identifier = arr[k + 1].Split('\t')[0];
                        info.Classification = arr[k + 1].Split('\t')[1];
                        info.Description = arr[k + 1].Split('\t')[2];
                    }
                    if (arr[k].Contains("Срок финансовой аренды:"))
                    {
                        info.RentalPeriod = arr[k + 1];
                    }
                    if (arr[k].Contains("Связанные сообщения"))
                    {
                        while (true)
                        {
                            info.LinkedMessages += arr[++k];
                            if (arr[k + 1].Contains('№'))
                            {
                                info.LinkedMessages += '\n';
                            }
                            else
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
                writer.Write(info, forSearch, i + 1);
                await LoadPageAsync(browser, baseUrl);
            }
            writer.Finish();
            return null;
        }

        async Task<string> EvaluateScript(string script)
        {
            string toReturn = null;
            //We need to bu sure that V8 was fully loaded
            while (true)
            {
                if (browser.CanExecuteJavascriptInMainFrame)
                {
                    await browser.EvaluateScriptAsync(script).ContinueWith(x =>
                    {
                        var response = x.Result;

                        if (response.Success && response.Result != null)
                        {
                            toReturn = response.Result.ToString();
                        }
                    });
                    break;
                }
                else Thread.Sleep(500);
            }

            if (toReturn == null)
            {
                toReturn = "fuck";
            }
            return toReturn;
        }

        private Task LoadPageAsync(IWebBrowser browser, string address = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler += (sender, args) =>
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= handler;
                    tcs.TrySetResult(true);
                }
            };

            browser.LoadingStateChanged += handler;

            if (!string.IsNullOrEmpty(address))
            {
                browser.Load(address);
            }
            return tcs.Task;
        }
    }
}
