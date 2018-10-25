using System.ComponentModel.DataAnnotations;

namespace CRMM.Models
{
    public class TestModel
    {
        [Key]
        public string a { get;  private set; }
        public int b { get; set; }

        public TestModel()
        {
            a = "0";
            b = 0;
        }
        public TestModel(string _a, int _b)
        {
            a = _a;
            b = _b;
        }
    }
}