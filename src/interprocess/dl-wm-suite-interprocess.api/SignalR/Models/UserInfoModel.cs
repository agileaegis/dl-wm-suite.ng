namespace dl.wm.suite.interprocess.api.SignalR.Models
{
    public sealed class UserInfoModel
    {
        public string Name { get; set; }

        public UserInfoModel(string name)
        {
            Name = name;
        }
    }
}