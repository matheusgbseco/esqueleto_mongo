using Common.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador.Gen
{
    public class HelperSysObjects : HelperSysObjectsTableModel
    {

        public HelperSysObjects(IEnumerable<Context> contexts)
            : base(contexts)
        {

        }

        protected override void DefineAuditFields(params string[] fields)
        {
            base.DefineAuditFields("ResponsavelId", "DtCadastro", "ResponsavelAtualizacaoId", "DtAtualizacao");
        }


        protected override string MakePropertyName(string column, string className, int key)
        {
            if (column.ToLower() != "idioma")
                column = base.MakePropertyName(column, className, key);

            if (key == 1)
            {
                if (column == "Idioma")
                    return column;
            }

            return column;
        }

    }
}
