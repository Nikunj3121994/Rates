using Microsoft.VisualBasic.FileIO;
using StankinQuestionnaire.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Helper
{
    public class UsersParser
    {
        readonly string _csv;
        List<string> _errors = new List<string>();

        public UsersParser(string csv)
        {
            _csv = csv;
        }

        public IEnumerable<string> Errors
        {
            get { return _errors; }
            private set { }
        }

        public IEnumerable<UserEditModel> ParseUsers()
        {
            List<string[]> lines = new List<string[]>();
            try
            {
                using (StringReader stringReader = new StringReader(_csv))
                {
                    const int fieldCount = 2;
                    using (TextFieldParser textFieldParser = new TextFieldParser(stringReader))
                    {
                        textFieldParser.Delimiters = new string[] { "," };
                        while (textFieldParser.EndOfData == false)
                        {
                            var line = textFieldParser.ReadFields();
                            if (line.Length != fieldCount)
                            {
                                _errors.Add("В каждой строке должно быть по два поля");
                            }
                            lines.Add(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            var users = lines.Select(l => new UserEditModel { UserName = l[0], Password = l[1] });
            return users;
        }

    }
}