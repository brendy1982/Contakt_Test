﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Json_Test.Models.Request
{
    public class RegisterMailModel_Sk
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Email(ErrorMessage = ModelUtil.invalidEmailErrMessage_Sk)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        /// <summary>
        /// Captcha
        /// </summary>
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Display(Name = "Heslo")]
        public string Password { get; set; }
        /// <summary>
        /// Confirm password
        /// </summary>
        [Display(Name = "Heslo zopakované")]
        public string ConfirmPassword { get; set; }
    }
}