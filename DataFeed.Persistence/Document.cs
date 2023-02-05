using MongoDB.Bson;
using System;

namespace DataFeed.Persistence
{
    internal class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}
