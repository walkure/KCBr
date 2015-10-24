using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace KCB2
{
    public partial class FormJSONLog : Form
    {
        FormMain _parent;
        public FormJSONLog(FormMain parent,IEnumerable<JSONData> skipbackLog)
        {
            _parent = parent;
            InitializeComponent();
            if (skipbackLog != null)
                foreach (var it in skipbackLog)
                    AddJSON(it);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            JSONData selected = listBox1.SelectedItem as JSONData;
            if (selected == null)
                return;

            textBox1.Text = string.Format("Path:{0}\r\nReq:\r\n{1}\r\nRes:\r\n{2}",
                selected.Path,selected.Request,selected.Response);
        }

        private void FormJSONLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parent.JSONLogWndClosed();
        }

        public void AddJSON(JSONData data)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() =>  listBox1.Items.Add(data)) );
            else
                listBox1.Items.Add(data);
        }
    }

    public class JSONData
    {
        public override string ToString()
        {
            return string.Format("{0}: Query[{1}]", _dt.ToShortTimeString(), _path);
        }

        public string Response { get { return Inflate(_res); } }
        public string Path { get { return _path; } }
        public string Request
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var kv in _req)
                {
                    sb.AppendFormat("{0}:{1}\r\n", kv.Key, kv.Value);
                }
                return sb.ToString();
            }
        }

        string _path;
        byte[] _res;
        DateTime _dt;
        IDictionary<string, string> _req;

        public JSONData(SessionData data)
        {
            _path = data.PathQuery;
            _res = Deflate(SessionData.DecodeJSON(data.ResponseJSON));
            _req = data.QueryParam;
            _dt = DateTime.Now;
        }

        static byte[] Deflate(string data)
        {
            using (var dst = new MemoryStream())
            {
                using (var src = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                using (var z_stream = new DeflateStream(dst, CompressionMode.Compress))
                    src.CopyTo(z_stream);
                return dst.ToArray();
           }
        }

        static string Inflate(byte[] data)
        {
            using (var dst = new MemoryStream())
            {
                using (var src = new MemoryStream(data))
                using (var z_stream = new DeflateStream(src, CompressionMode.Decompress))
                    z_stream.CopyTo(dst);
                return  Encoding.UTF8.GetString(dst.ToArray());
            }
        }

    }
}
