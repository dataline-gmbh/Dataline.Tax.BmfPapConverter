using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public interface ICodeGen
    {
        void CodeGen(CodeBuilder builder);
    }
}
