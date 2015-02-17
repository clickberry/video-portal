namespace IntegrationTestInfrastructure.Encoder
{
    public interface IMediaInfo
    {
        int Open(string path);
        string Option(string option, string value);
        void Close();
    }
}
