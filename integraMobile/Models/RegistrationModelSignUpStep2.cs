using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using integraMobile.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Response;
using System.Text;

namespace integraMobile.Models
{
    #region Models
    [Serializable]
    public class RegistrationModelSignUpStep2
    {
        #region Properties
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("CustomerInscriptionModel_Password", NameResourceType = typeof(Resources))]
        public string Password { get; set; }


        [DataType(DataType.Text)]
        [LocalizedDisplayName("SignUp2_CountryLiteral", NameResourceType = typeof(Resources))]
        public string Country { get; set; }

        public List<CheckBoxListQuestionsModel> ListQuestions { get; set; }
        public List<CountryModel> ListCountries { get; set; }

        public IList<String> SelectedQuestions { get; set; }
        #endregion

        #region Constructor
        public RegistrationModelSignUpStep2()
        {
            ListQuestions = new List<CheckBoxListQuestionsModel>();
            ListCountries = new List<CountryModel>();
            SelectedQuestions = new List<string>();
        }
        #endregion

        public IEnumerable<CountryModel> GetCountriesList(List<COUNTRy> oCountries)
        {
            List<CountryModel> ListCountries = new List<CountryModel>();
           
            foreach(COUNTRy coun in oCountries)
            {
                CountryModel oCountryModel = new CountryModel();
                oCountryModel.Id = Convert.ToString(coun.COU_ID);
                oCountryModel.Description = coun.COU_DESCRIPTION;
                ListCountries.Add(oCountryModel);
            }
            return ListCountries;
        }

        public IEnumerable<CheckBoxListQuestionsModel> GetCheckBoxListQuestionsList(List<integraMobile.Models.CountriesModel.Questions> questions)
        {
            List<CheckBoxListQuestionsModel> ListCheckBoxListQuestionsModel = new List<CheckBoxListQuestionsModel>();
            foreach (integraMobile.Models.CountriesModel.Questions oQuestions in questions)
            {
                foreach (integraMobile.Models.CountriesModel.Question oQuestion in oQuestions.question)
                {

                    CheckBoxListQuestionsModel quest = new CheckBoxListQuestionsModel();

                    quest.Id = oQuestion.idversion;
                    quest.QuestionNameHTML = getQuestionText(oQuestion);
                    quest.QuestionName = oQuestion.literal;
                    quest.Mandatory = oQuestion.mandatory;
                    ListCheckBoxListQuestionsModel.Add(quest);
                }
            }
            return ListCheckBoxListQuestionsModel;
        }

        private String getQuestionText(integraMobile.Models.CountriesModel.Question question)
        {
            StringBuilder sBuilder = new StringBuilder();

            sBuilder.Append(question.literal);

            int currentSpanIndex = 0;
            int firstSpan = -1;
            int secondSpan = -1;
            int currentUrl = 0;
            String text = question.literal;

            while (currentSpanIndex >= 0)
            {
                currentSpanIndex = sBuilder.ToString().IndexOf('$', currentSpanIndex);

                if (currentSpanIndex >= 0)
                {
                    //Delete $ span char separator from string
                    sBuilder = sBuilder.Replace("$", "", currentSpanIndex, 1);
                    text = sBuilder.ToString();

                    if (firstSpan == -1)
                    {
                        firstSpan = currentSpanIndex;
                    }
                    else if (secondSpan == -1)
                    {
                        secondSpan = currentSpanIndex;
                    }

                    //We have a complete span, so make the link
                    if (firstSpan != -1 && secondSpan != -1)
                    {

                        if (question.urls != null && question.urls.url != null && question.urls.url.Count() > currentUrl)
                        {
                            String sUrl = String.Empty;

                            String sUrl1 = question.urls.url[0];
                            String sUrl2 = question.urls.url[1];

                            if (currentUrl == 0)
                            {
                                sUrl = sUrl1;
                            }
                            else if (currentUrl == 1)
                            {
                                sUrl = sUrl2;
                            }

                            if (!String.IsNullOrEmpty(sUrl))
                            {
                                String resultado = sBuilder.ToString().Substring(firstSpan, secondSpan - firstSpan);
                                sBuilder = sBuilder.Replace(resultado, String.Format("<a href=\"{0} \" target=\"_blank\" >{1}</a>", sUrl, resultado));

                            }

                            firstSpan = -1;
                            secondSpan = -1;
                            currentUrl++;
                        }
                    }
                }
            }
            return sBuilder.ToString();
        }
    }
    #endregion
}