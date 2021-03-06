﻿using System;
using System.IO;

namespace Protsyk.PMS.FullText.Core
{
    internal static class PersistentMetadataFactory
    {
        public static IMetadataStorage<string> CreateStorage(string fieldsType, string folder, string fileName)
        {
            if (fieldsType == PersistentMetadataBtree.Id)
            {
                return new PersistentMetadataBtree(folder, fileName);
            }

            if (fieldsType == PersistentMetadataList.Id)
            {
                return new PersistentMetadataList(folder, fileName);
            }

            throw new NotSupportedException($"Fields type {fieldsType} is not supported");
        }
    }
}
