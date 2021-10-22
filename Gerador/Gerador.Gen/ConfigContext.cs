using Common.Gen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador.Gen
{
    public class ConfigContext
    {
        #region Config Contexts

        public IEnumerable<Context> GetConfigContext()
        {
            return new List<Context>
            {
                ConfigContextBack(),
                ConfigContextFront()
            };
        }
        private Context ConfigContextBack()
        {
            var contextName = "Project";

            return new Context
            {

                ConnectionString = ConfigurationManager.ConnectionStrings["Core"].ConnectionString,

                Namespace = contextName,
                Module = "Core",
                ContextName = contextName,
                ShowKeysInFront = true,
                LengthBigField = 250,

                OutputClassDomain = ConfigurationManager.AppSettings[string.Format("outputClassDomain")],
                OutputClassDto = ConfigurationManager.AppSettings[string.Format("outputClassDto")],
                OutputClassApp = ConfigurationManager.AppSettings[string.Format("outputClassApp")],
                OutputClassApi = ConfigurationManager.AppSettings[string.Format("outputClassApi")],
                OutputClassFilter = ConfigurationManager.AppSettings[string.Format("outputClassFilter")],

                OverrideFiles = false,

                Arquiteture = ArquitetureType.TableModel,

                TableInfo = new UniqueListTableInfo
                {
                    new TableInfo { TableName = "Pessoa" },
                    



                }
            };
        }

        private Context ConfigContextFront()
        {
            return new Context
            {

                ConnectionString = ConfigurationManager.ConnectionStrings["Core"].ConnectionString,

                Namespace = "Project.ui",
                Module = "Core",
                ContextName = "Project.ui",

                OutputFrontend = ConfigurationManager.AppSettings["outputClassFrontend"],
                CamelCasing = true,
                MakeFront = true,
                AlertNotFoundTable = true,

                TableInfo = new UniqueListTableInfo
                {
                    new TableInfo { TableName = "Pessoa", ClassNameFormated = "Cadastro > Pessoas", MakeFront = true,  },
                }
            };
        }

        #endregion
    }
}
