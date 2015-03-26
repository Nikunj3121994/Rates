using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private StankinQuestionnaireEntities dataContext;
        public StankinQuestionnaireEntities Get()
        {
            return dataContext ?? (dataContext = new StankinQuestionnaireEntities());
        }

        protected override void DisposeCore()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
            }
        }
    }
}
