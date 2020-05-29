﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IoTConnect.Model
{
    /// <summary>
    /// Add Rule class
    /// </summary>
    public class AddRuleModel
    {
        /// <summary>
        /// Gets and sets the name
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string name { get; set; }
        /// <summary>
        /// Gets and sets the ruleType
        /// </summary>
        [Required(ErrorMessage = "RuleType should be 1/2")]
        public int ruleType { get; set; }
        /// <summary>
        /// Gets and sets the templateGuid
        /// </summary>
        [Required(ErrorMessage = "TemplateGuid is required")]
        public string templateGuid { get; set; }
        /// <summary>
        /// Gets and sets the commandGuid
        /// </summary>
        public string commandGuid { get; set; }
        /// <summary>
        /// Gets and sets the parameterValue
        /// </summary>
        public string parameterValue { get; set; }
        /// <summary>
        /// Gets and sets the url
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// Gets and sets the wheaders
        /// </summary>
        public List<object> wheaders { get; set; }
        /// <summary>
        /// Gets and sets the entityGuid
        /// </summary>
        public string entityGuid { get; set; }
        /// <summary>
        /// Gets and sets the applyTo
        /// </summary>
        [Required(ErrorMessage = "ApplyTo is required")]
        public int applyTo { get; set; }
        /// <summary>
        /// Gets and sets the conditionText
        /// </summary>
        public string conditionText { get; set; }
        /// <summary>
        /// Gets and sets the isIncludeChildEntity
        /// </summary>
        public bool isIncludeChildEntity { get; set; }
        /// <summary>
        /// Gets and sets the ignorePreference
        /// </summary>
        public bool ignorePreference { get; set; }
        /// <summary>
        /// Gets and sets the ruleAttr
        /// </summary>
        internal List<RuleAttr> ruleAttr { get; set; }
        /// <summary>
        /// Gets and sets the ruleAttribute
        /// </summary>
        public List<object> ruleAttribute { get; set; }
        /// <summary>
        /// Gets and sets the deliveryMethod
        /// </summary>
        public List<string> deliveryMethod { get; set; }
        /// <summary>
        /// Gets and sets the eventSubscriptionGuid
        /// </summary>
        public string eventSubscriptionGuid { get; set; }
        /// <summary>
        /// Gets and sets the severityLevelGuid
        /// </summary>
        [Required(ErrorMessage = "SeverityLevelGuid is required")]
        public string severityLevelGuid { get; set; }
        /// <summary>
        /// Gets and sets the parentAttributeGuid
        /// </summary>
        public string parentAttributeGuid { get; set; }
        /// <summary>
        /// Gets and sets the devices
        /// </summary>
        public List<string> devices { get; set; }
        /// <summary>
        /// Gets and sets the roles
        /// </summary>
        public List<string> roles { get; set; }
        /// <summary>
        /// Gets and sets the users
        /// </summary>
        public List<string> users { get; set; }
    }
    public class RuleAttr
    {
        /// <summary>
        /// Gets and sets the condition
        /// </summary>
        public string condition { get; set; }
        /// <summary>
        /// Gets and sets the value
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// Gets and sets the dataTypeName
        /// </summary>
        public string dataTypeName { get; set; }
    }
}
