using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Data.Configuration
{
    public class DocumentActionConfiguration : EntityTypeConfiguration<DocumentAction>
    {
        public DocumentActionConfiguration()
        {
            Property(da => da.DocumentActionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
