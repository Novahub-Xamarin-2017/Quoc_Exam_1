using StudentManager.Controllers;
using StudentManager.Models.BaseModels;

namespace StudentManager.Models
{
    public class Subject : EasyModel
    {
        [IgnoreInput]
        public int Id { get; set; }

        [PrompDisplay("subject's name")]
        public string Name { get; set; }

        [PrompDisplay("subject's coefficient number")]
        public double CoefficientNumber { get; set; }

        public override void Input()
        {
            base.Input();
            Id = DataProvider.GetInstance.Subjects.Count + 1;
        }
    }
    
}