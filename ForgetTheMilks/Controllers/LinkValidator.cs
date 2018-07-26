using System;
using System.Net;
using System.Web;

namespace ForgetTheMilks.Controllers
{
    public class LinkValidator : ILinkValidator
    {
        public void Validate(string link)
        {
            var request = WebRequest.CreateHttp(link);
            request.Method = "HEAD";
            try
            {
                request.GetResponse();
            }
            catch
            {
                throw new ApplicationException("Invalid Link " + link);
            }
        }
    }

    public interface ILinkValidator
    {
        void Validate(string link);
    }
}
