namespace MISA.WebFresher042023.Demo.Common.Exceptions
{
    public class DupCodeException:Exception
    {
        public DupCodeException(List<string> userMsg, Dictionary<string,List<string>>? errorsMore)
        {
            UserMsg = userMsg;
            ErrorsMore = errorsMore;
        }

        #region Properties

        public List<string> UserMsg { get; set; }
        public Dictionary<string,List<string>>? ErrorsMore { get; set; }

        #endregion
    }
}
