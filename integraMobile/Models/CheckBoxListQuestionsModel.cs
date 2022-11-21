using integraMobile.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Models
{
    [Serializable]
    public class CheckBoxListQuestionsModel
    {
        public Int32 Id { get; set; }
        public String QuestionNameHTML { get; set; }
        public String QuestionName { get; set; }
        public Int32 Mandatory { get; set; }

        public CheckBoxListQuestionsModel()
        {
            
        }
       
    }
}