namespace Health.Configuration.Sms
{
    public interface ISmsSender
    {
        /// <summary>
        /// Send a SMS message
        /// </summary>
        /// <param name="mobileNumber">The mobile number receiver</param>
        /// <param name="message">The message sent</param>
        void Send(string mobileNumber, string message);
    }
}
