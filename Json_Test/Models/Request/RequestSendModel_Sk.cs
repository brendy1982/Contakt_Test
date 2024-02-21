using Json_Test.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Json_Test.Models.Request
{
    public class RequestSendModel_Sk : RegisterMailModel_Sk
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Meno")]
        public string Name { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Telefón")]
        public string Phone { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Sem napíšte správu")]
        public string Text { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [Display(Name = "Požadovaná cena")]
        public string Price { get; set; }
    }
}