namespace BaseProjectApp.API.Helpers
{
    public class MessagesKeys
    {
        //General
        #region 
        public static string GeneralMessage { get => "General Message"; } 
        #endregion


        //Validations
        #region  
        public static string ValidationsMessage { get => "Validations Message"; } 
        #endregion


        //Errors
        #region   
        public static string ErrorsMessages { get => "Errors Messages"; } 
        #endregion

    }


    public class APIMessenger
    {
        private readonly Dictionary<string, Dictionary<string, string>> _messages;

        public APIMessenger()
        {
            _messages = new Dictionary<string, Dictionary<string, string>>();

            var englishMessages = new Dictionary<string, string>
        {

            { MessagesKeys.GeneralMessage, "GeneralMessage English Message"},
            { MessagesKeys.ValidationsMessage, "ValidationsMessage English Message"},
            { MessagesKeys.ErrorsMessages, "ErrorsMessages English Message"},

        };
            _messages.Add("en", englishMessages);

            var arabicMessages = new Dictionary<string, string>
        {
            { MessagesKeys.GeneralMessage, "GeneralMessage Arabic Message"},
            { MessagesKeys.ValidationsMessage, "ValidationsMessage Arabic Message"},
            { MessagesKeys.ErrorsMessages, "ErrorsMessages Arabic Message"},

        };
            _messages.Add("ar", arabicMessages);
        }

        public string GetMessage(string language, string key)
        {
            if (_messages.TryGetValue(language, out var languageMessages))
            {
                if (languageMessages.TryGetValue(key, out var message))
                {
                    return message;
                }
            }

            return string.Empty;
        }
    }
}
