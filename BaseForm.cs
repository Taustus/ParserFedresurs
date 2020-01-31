using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParserFedresource
{
    public partial class BaseForm : Form
    {
        BrowserForm _bf;
        CancellationTokenSource _cts;
        Thread _parse_thr;

        public BaseForm()
        {
            InitializeComponent();
            _bf = new BrowserForm();
            _bf.Show();
        }

        private void parse_btn_Click(object sender, EventArgs e)
        {
            if (forSearch_tb.Text.Equals(""))
            {
                MessageBox.Show("Вы не ввели значение для поиска!");
                return;
            }
            _cts = new CancellationTokenSource();
            _parse_thr = new Thread(new ParameterizedThreadStart(Algorithm));
            _parse_thr.Start(_cts.Token);
        }

        async void Algorithm(object obj)
        {
            try
            {
                stop_btn.Invoke(new Action(() => stop_btn.Enabled = true));
                parse_btn.Invoke(new Action(() => parse_btn.Enabled = false));
                var cts = (CancellationToken)obj;
                await _bf.Algorithm(cts, forSearch_tb.Text, beginning_cb.Checked);
                parse_btn.Invoke(new Action(() => parse_btn.Enabled = true));
                stop_btn.Invoke(new Action(() => stop_btn.Enabled = false));
                Stop_btn_Click(null, null);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Stop_btn_Click(null, null);
            }
        }

        private void Stop_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    _cts.Cancel();
                }
                _cts.Dispose();
                _parse_thr.Abort();
                parse_btn.Enabled = true;
                stop_btn.Enabled = false;
            }
            catch
            {

            }
        }
    }
}
