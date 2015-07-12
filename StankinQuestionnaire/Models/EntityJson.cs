using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Models
{
    public class EntityJson
    {
        public object Entity { get; set; }
        public string Text { get; set; }
        public EntityStatus Status { get; set; }
    }

    public class IDJson<T>
    {
        public T ID { get; set; }
        public string Text { get; set; }
        public EntityStatus Status { get; set; }
    }

    public enum EntityStatus
    {
        SUCCESS = 1,
        ERROR = 2
    }
}