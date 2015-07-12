using Moq;
using StankinQuestionnaire.Areas.Admin.Controllers;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace Stankin.Test
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            //Arrage
            var docLogRep = new Mock<IDocumentLogRepository>();
            var docLogList = new List<DocumentLog>() { 
                new DocumentLog { ActionDate = DateTime.Now, CalculationId = 1, Description = "asdad", DocumentActionId = 1, DocumentLogId = 1, UserId = 1 },
                new DocumentLog { ActionDate = DateTime.Now, CalculationId = 1, Description = "asdad", DocumentActionId = 1, DocumentLogId = 1, UserId = 1 }};
            docLogRep.Setup(dlr => dlr.GetMany(dl => dl.UserId > 1)).Returns(docLogList);

            var documentLogCntr = new DocumentLogController(docLogRep.Object);
            //Act

            var result = documentLogCntr.Index();
            //Assert
            Assert.Equal(result, "");
        }
    }
}
