﻿using System;
using System.Collections;
using System.Collections.Generic;
using LibgenServer.Core.Database;
using LibgenServer.Core.Entities;
using LibgenServer.Core.Import.SqlDump;

namespace LibgenServer.Core.Import
{
    public class FictionImporter : Importer<FictionBook>
    {
        private readonly DateTime lastModifiedDateTime;
        private readonly int lastModifiedLibgenId;

        public FictionImporter(LocalDatabase localDatabase, BitArray existingLibgenIds)
            : base(localDatabase, existingLibgenIds, TableDefinitions.Fiction)
        {
            if (IsUpdateMode)
            {
                FictionBook lastModifiedFictionBook = LocalDatabase.GetLastModifiedFictionBook();
                lastModifiedDateTime = lastModifiedFictionBook.LastModifiedDateTime;
                lastModifiedLibgenId = lastModifiedFictionBook.LibgenId;
            }
        }

        protected override void InsertBatch(List<FictionBook> objectBatch)
        {
            LocalDatabase.AddFictionBooks(objectBatch);
        }

        protected override void UpdateBatch(List<FictionBook> objectBatch)
        {
            LocalDatabase.UpdateFictionBooks(objectBatch);
        }

        protected override bool IsNewObject(FictionBook importingObject)
        {
            return importingObject.LastModifiedDateTime > lastModifiedDateTime ||
                (importingObject.LastModifiedDateTime == lastModifiedDateTime && importingObject.LibgenId != lastModifiedLibgenId);
        }

        protected override int? GetExistingObjectIdByLibgenId(int libgenId)
        {
            return LocalDatabase.GetFictionBookIdByLibgenId(libgenId);
        }
    }
}
