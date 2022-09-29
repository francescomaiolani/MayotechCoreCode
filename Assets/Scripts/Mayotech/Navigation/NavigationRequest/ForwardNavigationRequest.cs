namespace Mayotech.Navigation
{
    public class ForwardNavigationRequest : NavigationRequest
    {
        private string nextScene;

        public string NextScene => nextScene;

        public ForwardNavigationRequest(string nextScene)
        {
            this.nextScene = nextScene;
        }
    }
}