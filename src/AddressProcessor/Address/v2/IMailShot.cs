namespace AddressProcessing.Address.v2
{
    public interface IMailShot
    {
        void SendPostalMailShot(string name, string address1, string town, string county, string country, string postCode);
        void SendEmailMailShot(string name, string email);
        void SendSmsMailShot(string name, string number);
    }
}
