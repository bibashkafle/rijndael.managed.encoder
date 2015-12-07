
namespace Yubikey
{

    public class DbResult
    {
        public DbResult() { }

        public string ErrorCode { get; set; }
        public string Msg { get; set; }
        public string Id { get; set; }
        public string XML { get; set; }
        public string Extra { get; set; }
        public string Extra2 { get; set; }
        public void SetError(string errorCode, string msg, string id)
        {
            ErrorCode = errorCode;
            Msg = msg;
            Id = id;
        }
        public void SetError(string errorCode, string msg, string id, string ExtraTxt = "", string Extra2Txt = "")
        {
            ErrorCode = errorCode;
            Msg = msg;
            Id = id;
            Extra = ExtraTxt;
            Extra2 = Extra2Txt;
        }
    }
}
