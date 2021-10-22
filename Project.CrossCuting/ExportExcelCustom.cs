using Common.API;
using Common.Domain.Base;
using System;
using System.Collections.Generic;

namespace Project.Core.CrossCuting
{
    public class ExportExcelCustom<T> : ExportExcel<T>
    {

        public ExportExcelCustom(FilterBase filter, string controller) : base(filter)
        {
            this.DefineDefaultProperties();
            this.DefineCustomProperties(filter, controller);
        }

        private void DefineCustomProperties(FilterBase filter, string controller)
        {
            
        }

        private void DefineDefaultProperties()
        {
            
        }
    }
}
