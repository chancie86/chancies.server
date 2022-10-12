using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using chancies.Server.Blog.Interfaces;
using chancies.Server.Common.Exceptions;
using chancies.Server.Persistence.Models;
using chancies.server.Persistence.Repositories;
using chancies.Server.Persistence.Repositories;

namespace chancies.Server.Blog.Implementation
{
    internal class ImageService
        : IImageService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IImageRepository _imageRepository;

        public ImageService(IDocumentRepository documentRepository, IImageRepository imageRepository)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
        }

        public async Task Upload(DocumentId documentId, Stream fileStream, string filePath)
        {
            // Check that the document exists
            _ = await _documentRepository.Read(documentId);
            await _imageRepository.Upload(fileStream, $"{documentId}/{filePath}");
        }
        
        public async Task<IList<ImageReference>> List(DocumentId documentId)
        {
            // Check that the document exists
            _ = await _documentRepository.Read(documentId);
            return await _imageRepository.List(documentId.ToString());
        }

        public async Task Delete(DocumentId documentId, string filePath)
        {
            var doc = await _documentRepository.Read(documentId);
            var images = doc.Elements.Where(x => x.Type == DocumentElementType.Images).Cast<ImagesDocumentElement>();
            var inUse = images.Any(x => x.Images.Any(y => string.Equals(y.Path, filePath, StringComparison.Ordinal)));

            if (inUse)
            {
                throw new InUseException("Cannot delete the file because it is referenced by the document");
            }

            await _imageRepository.Delete($"{documentId}/{filePath}");
        }
    }
}
