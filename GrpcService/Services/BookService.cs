using Grpc.Core;
using GrpcService.Models;
using MyProto;
using System.Linq;
using System.Threading.Tasks;
using static MyProto.GrpcBook;

namespace GrpcService.Services
{
    public class BookService: GrpcBookBase
    {
        public readonly gRPC_ExContext _db;

        public BookService(gRPC_ExContext db)
        {
            _db = db;
        }

        public override Task<BookList> GetAll(MyProto.Empty requestData, ServerCallContext context)
        {
            BookList response = new BookList();
            var cusList = from obj in _db.Books
                          select new MyProto.Book()
                          {
                              Id = obj.Bid,
                              Name = obj.Bname,
                              Version = (int)obj.Bversion,
                              Page = (int)obj.Bpages,
                              Price = (int)obj.Bprice,
                              Year = (int)obj.Byear,
                              AuthorId = (int)obj.Aid
                          };
            response.Book.AddRange(cusList);

            return Task.FromResult(response);
        }

        public override Task<AuthorList> GetAllAuthor(MyProto.Empty requestData, ServerCallContext context)
        {
            AuthorList response = new AuthorList();
            var cusList = from obj in _db.Authors
                          select new MyProto.Author()
                          {
                              AuthorId = obj.Aid,
                              AuthorName = obj.Aname,

                          };
            response.Author.AddRange(cusList);

            return Task.FromResult(response);
        }

        public override Task<MyProto.Book> GetBookById(IDRequest request, ServerCallContext context)
        {

            var obj = this._db.Books.Find(request.Id);

            if (obj == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
            }


            MyProto.Book book = new MyProto.Book()
            {
                Id = obj.Bid,
                Name = obj.Bname,
                Version = (int)obj.Bversion,
                Page = (int)obj.Bpages,
                Price = (int)obj.Bprice,
                Year = (int)obj.Byear,
                AuthorId = (int)obj.Aid
            };

            return Task.FromResult(book);
        }

        public override async Task<MyProto.Book> CreateBook(CreateBookRequest request, ServerCallContext context)
        {
            var bookEntity = new Models.Book()
            {
                Bid = request.Bid,
                Bname = request.Name,
                Bversion = request.Version,
                Bpages = request.Page,
                Bprice = request.Price,
                Byear = request.Year,
                Aid = request.AuthorId
            };

            _db.Books.Add(bookEntity);
            await _db.SaveChangesAsync();

            var book = new MyProto.Book()
            {
                Id = bookEntity.Bid,
                Name = bookEntity.Bname,
                Version = (int)bookEntity.Bversion,
                Page = (int)bookEntity.Bpages,
                Price = (int)bookEntity.Bprice,
                Year = (int)bookEntity.Byear,
                AuthorId = (int)bookEntity.Aid
            };

            return book;
        }

        public override async Task<MyProto.Book> UpdateBook(UpdateBookRequest request, ServerCallContext context)
        {
            var bookEntity = _db.Books.Find(request.Id);

            if (bookEntity == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
            }

            bookEntity.Bname = request.Name;
            bookEntity.Bversion = request.Version;
            bookEntity.Bpages = request.Page;
            bookEntity.Bprice = request.Price;
            bookEntity.Byear = request.Year;
            bookEntity.Aid = request.AuthorId;

            await _db.SaveChangesAsync();

            var updatedBook = new MyProto.Book()
            {
                Id = bookEntity.Bid,
                Name = bookEntity.Bname,
                Version = (int)bookEntity.Bversion,
                Page = (int)bookEntity.Bpages,
                Price = (int)bookEntity.Bprice,
                Year = (int)bookEntity.Byear,
                AuthorId = (int)bookEntity.Aid
            };

            return updatedBook;
        }

        public override async Task<Empty> DeleteBook(IDRequest request, ServerCallContext context)
        {
            var bookEntity = _db.Books.Find(request.Id);

            if (bookEntity == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
            }

            _db.Books.Remove(bookEntity);
            await _db.SaveChangesAsync();

            return new Empty();
        }
    }
}

