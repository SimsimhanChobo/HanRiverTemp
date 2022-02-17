using Newtonsoft.Json;
using System.Net;

namespace HanRiver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TempRefresh(false);
        }

        class HanRiverStatus
        {
            public string station = "";
            public string status = "";
            public string temp = "";
            public string time = "";
            public string type = "";
        }

        private void timer1_Tick(object sender, EventArgs e) => TempRefresh();

        public async void TempRefresh(bool thread = true)
        {
            HanRiverStatus? hanRiverStatus;

            if (thread)
            {
                string json = await Task.Run(() => ApiCrawling("https://api.hangang.msub.kr/"));
                hanRiverStatus = JsonConvert.DeserializeObject<HanRiverStatus>(json);
            }
            else
                hanRiverStatus = JsonConvert.DeserializeObject<HanRiverStatus>(ApiCrawling("https://api.hangang.msub.kr/"));

            if (hanRiverStatus != null)
                label1.Text = $"한강 온도: {hanRiverStatus.temp}℃";

            Size = new Size(label1.Size.Width + 42, label1.Size.Height + 63);
        }

        public static string ApiCrawling(string url)
        {
#pragma warning disable SYSLIB0014 // 형식 또는 멤버는 사용되지 않습니다.
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (WebResponse response = request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
#pragma warning restore SYSLIB0014 // 형식 또는 멤버는 사용되지 않습니다.
        }
    }
}