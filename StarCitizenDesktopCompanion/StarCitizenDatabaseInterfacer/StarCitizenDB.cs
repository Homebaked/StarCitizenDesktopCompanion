using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenDatabaseInterfacer
{
    class StarCitizenDB
    {


        public void SaveNew(SCDataManager dataManager)
        {
            nonQueryCommand("BEGIN TRANSACTION");
            createDatabaseTables();
            throw new NotImplementedException();
            nonQueryCommand("END TRANSACTION");
        }

        public void SaveDelta(SCDeltaManager deltaManager)
        {
            nonQueryCommand("BEGIN TRANSACTION");
            throw new NotImplementedException();
            nonQueryCommand("END TRANSACTION");
        }

        public SCDataManager Load()
        {
            nonQueryCommand("BEGIN TRANSACTION");
            throw new NotImplementedException();
            nonQueryCommand("END TRANSACTION");
        }

        private void createDatabaseTables()
        {

        }
    }
}
