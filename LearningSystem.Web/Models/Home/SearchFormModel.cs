namespace LearningSystem.Web.Models.Home
{
    using System.ComponentModel.DataAnnotations;

    public class SearchFormModel
    {
        public string SearchText { get; set; }

        [Display(Name = "Users")]
        public bool SearchInUsers { get; set; } = true;

        [Display(Name = "Courses")]
        public bool SearchInCourses { get; set; } = true;
    }
}
