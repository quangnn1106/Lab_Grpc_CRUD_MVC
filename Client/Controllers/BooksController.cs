using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using MyProto;
using System.Linq;
using System.Collections.Generic;
using static MyProto.GrpcBook;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http.Features;

namespace Client.Controllers
{
    public class BooksController : Controller
    {
        // GET: CustomersController
        public IActionResult Index()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GrpcBookClient(channel);
            MyProto.BookList data = client.GetAll(new MyProto.Empty());
            ViewBag.Data = data;

            MyProto.AuthorList AuthorData = client.GetAllAuthor(new MyProto.Empty());
            ViewBag.AuthorData = AuthorData;

            return View();
        }

        // GET: CustomersController/GetCustomer/5
        public IActionResult GetBookById(int id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GrpcBookClient(channel);
            MyProto.Book data = client.GetBookById(new MyProto.IDRequest() { Id = id });
            ViewBag.Data = data;
            MyProto.AuthorList audata = client.GetAllAuthor(new MyProto.Empty());
            ViewBag.AuthorData = audata;

            return View();
        }

        // GET: CustomersController
        public IActionResult GetAllAuthor()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GrpcBookClient(channel);
            MyProto.AuthorList data = client.GetAllAuthor(new MyProto.Empty());
            ViewBag.AuthorData = data;

            return View();
        }

        // GET: BooksController/Create
        public IActionResult Create()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GrpcBookClient(channel);
            MyProto.AuthorList data = client.GetAllAuthor(new MyProto.Empty());
            ViewBag.AuthorData = data;

            return View();
        }

        // POST: BooksController/Create
        [HttpPost]
        public IActionResult Create(MyProto.CreateBookRequest model)
        {
            if (ModelState.IsValid)
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new GrpcBookClient(channel);
               
                
                // Create a new book using gRPC
                var response = client.CreateBook(model);
               
                // Redirect to the details page of the newly created book
                return RedirectToAction("Index");
            }

            // If the ModelState is not valid, return to the Create view with errors
            return View(model);
        }

        // GET: BooksController/Update/5
        public IActionResult Update(int id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GrpcBookClient(channel);

            MyProto.AuthorList audata = client.GetAllAuthor(new MyProto.Empty());
            ViewBag.AuthorData = audata;
            // Get the book by ID
            MyProto.Book data = client.GetBookById(new MyProto.IDRequest() { Id = id });

            // Display the book details in the Update view
            return View(data);
        }

        [HttpPost]
        public IActionResult Update(MyProto.Book model)
        {
            if (ModelState.IsValid)
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new GrpcBookClient(channel);

                // Create an UpdateBookRequest object and populate it with data from the Book model
                var updateRequest = new MyProto.UpdateBookRequest
                {
                    Id = model.Id,
                    Name = model.Name,
                    Version = model.Version,
                    Page = model.Page,
                    Price = model.Price,
                    Year = model.Year,
                    AuthorId = model.AuthorId
                };

                // Update the book using gRPC
                var response = client.UpdateBook(updateRequest);

                // Redirect to the details page of the updated book
                return RedirectToAction("Index", new { id = response.Id });
            }

            // If the ModelState is not valid, return to the Update view with errors
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GrpcBookClient(channel);
            MyProto.Book data = client.GetBookById(new MyProto.IDRequest() { Id = (int)id });
            ViewBag.Data = data;
            MyProto.AuthorList audata = client.GetAllAuthor(new MyProto.Empty());
            ViewBag.AuthorData = audata;
            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GrpcBookClient(channel);

            // Create an IDRequest with the book ID to be deleted
            var idRequest = new MyProto.IDRequest { Id = id };

            try
            {
                // Call the DeleteBook gRPC method to delete the book
                client.DeleteBook(idRequest);

                // Redirect to the book list or another appropriate page
                return RedirectToAction("Index");
            }
            catch (RpcException ex)
            {
                return View("Error");
            }
        }
    }
}
