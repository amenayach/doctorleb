namespace Health.Configuration.Sms
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography;
    using System.Text;

    public class GlobalSmsSender : ISmsSender
    {
        private readonly SmsConfig smsConfig;

        private readonly HttpClient httpClient;

        private readonly Uri smsEndPoint;

        public GlobalSmsSender(SmsConfig smsConfig)
        {
            this.httpClient = new HttpClient();
            this.smsConfig = smsConfig ?? throw new ArgumentNullException(nameof(smsConfig));
            this.smsEndPoint = new Uri(smsConfig.EndPoint);

            httpClient.BaseAddress = new Uri($"{smsEndPoint.Scheme}://{smsEndPoint.Host}");
        }

        /// <summary>
        /// Send a SMS message
        /// </summary>
        /// <param name="mobileNumber">The mobile number receiver</param>
        /// <param name="message">The message sent</param>
        public void Send(string mobileNumber, string message)
        {
            if (string.IsNullOrWhiteSpace(mobileNumber))
            {
                throw new ArgumentNullException(nameof(mobileNumber));
            }

            string credentials = GetCredentials();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("MAC", credentials);

            var payload = new JObject
                {
                    { "destination", GetCleanMobileNumber(mobileNumber) },
                    { "message", message },
                    { "origin", "AtDoctor" }
                };

            var result = httpClient.PostAsync(smsEndPoint, new StringContent(payload.ToString(), Encoding.UTF8, "application/json")).Result;
        }

        private string GetCleanMobileNumber(string mobileNumber)
        {
            mobileNumber = mobileNumber.Replace(" ", string.Empty);

            if (mobileNumber.StartsWith("00961"))
            {
                return "+961" + mobileNumber.Substring(5);
            }

            if (mobileNumber.StartsWith("03"))
            {
                return "+961" + mobileNumber.Substring(1);
            }

            if (mobileNumber.Length == 8)
            {
                return "+961" + mobileNumber;
            }

            return mobileNumber;
        }

        private string GetCredentials()
        {
            var timestamp = ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

            var nonce = Guid.NewGuid().ToString("N");

            //var mac = $"{timestamp}\n{nonce}\nPOST\n{smsEndPoint.PathAndQuery}\n{smsEndPoint.Host}\n443\n\n";
            string mac = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n\n", timestamp, nonce, "POST", smsEndPoint.PathAndQuery, smsEndPoint.Host, "443");

            mac = Convert.ToBase64String((new HMACSHA256(Encoding.ASCII.GetBytes(smsConfig.Secret))).ComputeHash(Encoding.ASCII.GetBytes(mac)));

            return string.Format("id=\"{0}\", ts=\"{1}\", nonce=\"{2}\", mac=\"{3}\"", smsConfig.Key, timestamp, nonce, mac);
        }
    }
}
