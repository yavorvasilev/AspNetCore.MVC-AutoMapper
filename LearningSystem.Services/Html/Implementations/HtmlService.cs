namespace LearningSystem.Services.Html.Implementations
{
    using Ganss.XSS;

    public class HtmlService : IHtmlService
    {
        private readonly HtmlSanitizer sanitizer;

        public HtmlService()
        {
            sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
        }

        public string Sanitize(string htmlContent)
            => sanitizer.Sanitize(htmlContent);
    }
}
