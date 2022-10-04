namespace Mayotech.Toast
{
    public class ToastRequest
    {
        private string textKey;
        private float showDuration;

        public string TextKey => textKey;
        public float ShowDuration => showDuration;

        public ToastRequest(string textKey, float showDuration)
        {
            this.textKey = textKey;
            this.showDuration = showDuration;
        }
    }
}