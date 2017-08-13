using MvvmCross.Core.ViewModels;

namespace Glados.Core.ViewModels
{
    public class ResponseViewModel
        : MvxViewModel
    {
        public static string responseSender;
        public string ResponseSender
        {
            get { return responseSender; }
            set
            {
                SetProperty(ref responseSender, value);
            }
        }
        public static string responseMessage;
        public string ResponseMessage
        {
            get { return responseMessage; }
            set
            {
                SetProperty(ref responseMessage, value);
            }
        }


        public ResponseViewModel()
        {

        }

    }
}
