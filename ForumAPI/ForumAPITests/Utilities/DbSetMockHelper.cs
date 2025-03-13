using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;



namespace ForumAPITests.Utilities
{
    public static class DbSetMockHelper
    {
        public static Mock<DbSet<T>> CreateDbSetMock<T>(IQueryable<T> elements) where T : class
        {
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elements.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elements.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elements.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elements.GetEnumerator());
            dbSetMock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => elements.ToList().Add(s));
            return dbSetMock;
        }
    }
}
