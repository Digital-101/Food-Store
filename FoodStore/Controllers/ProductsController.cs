using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodStore.DAL;
using FoodStore.Models;
using FoodStore.ViewModels;
using PagedList;

namespace FoodStore.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductsController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Products
        [AllowAnonymous]
        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            //instantiate new viewmodel
            ProductIndexViewModel viewModel = new ProductIndexViewModel();

            //select the products
            var products = db.Products.Include(p => p.Category);

            //filter by category
            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category);
            }

            //search product
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search) || p.Category.Name.Contains(search));
                //store search in viewbag
                //ViewBag.Search = search;

                //store in vm
                viewModel.Search = search;
            }

            //group search results into categories and count how many items in each category
            viewModel.CatsWithCount = from matchingProducts in products
                                      where
                                      matchingProducts.CategoryID != null
                                      group matchingProducts by
                                      matchingProducts.Category.Name into
                                      catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          ProductCount = catGroup.Count()
                                      };


            //var categories = products.OrderBy(p => p.Category.Name).Select(p => p.Category.Name).Distinct();
            
            //products filtered by category after categories variable
            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category);
                viewModel.Category = category;
            }

            //sort the results
            switch (sortBy)
            {
                case "price_lowest":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_highest":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            //ViewBag.Category = new SelectList(categories);

            //const int PageItems = 3;
            int currentPage = (page ?? 1);
            viewModel.Products = products.ToPagedList(currentPage, Constants.PageItems);
            viewModel.SortBy = sortBy;

            //hold data to populate select element
            viewModel.Sorts = new Dictionary<string, string>
            {
                {"Price low to high", "price_lowest" },
                {"Price high to low", "price_highest" },
            };

            return View(viewModel);
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            //ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            //return View();

            //Add Images to a New Product
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.CategoryList = new SelectList(db.Categories, "ID", "Name");
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName"));
            }
            return View(viewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {
            //Manually mapping products to productsVM
            Product product = new Product();
            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.CategoryID = viewModel.CategoryID;
            product.ProductImageMappings = new List<ProductImageMapping>();
            //get a list of selected images without any blanks
            string[] productImages = viewModel.ProductImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
            for (int i = 0; i < productImages.Length; i++)
            {
                product.ProductImageMappings.Add(new ProductImageMapping
                {
                    ProductImage = db.ProductImages.Find(int.Parse(productImages[i])),
                    ImageNumber = i
                });
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            viewModel.CategoryList = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName", viewModel.ProductImages[i]));
            }
            return View(viewModel);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ProductViewModel viewModel = new ProductViewModel();
            viewModel.CategoryList = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            viewModel.ImageLists = new List<SelectList>();

            foreach (var imageMapping in product.ProductImageMappings.OrderBy(pim=>pim.ImageNumber))
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName", imageMapping.ProductImageID));
            }

            viewModel.ID = product.ID;
            viewModel.Name = product.Name;
            viewModel.Description = product.Description;
            viewModel.Price = product.Price;

            return View(viewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel viewModel)
        {
            var productToUpdate = db.Products.Include(p => p.ProductImageMappings).Where(p => p.ID == viewModel.ID).Single();
            if (TryUpdateModel(productToUpdate, "", new string[] { "Name", "Description", "Price", "CategoryID"}))
            {
                if (productToUpdate.ProductImageMappings == null)
                {
                    productToUpdate.ProductImageMappings = new List<ProductImageMapping>();
                }
                //get a list of selected images without any blanks
                string[] productImages = viewModel.ProductImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
                for (int i = 0; i < productImages.Length; i++)
                {
                    //get the image currently stored
                    var imageMappingToEdit = productToUpdate.ProductImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    //find the new image
                    var image = db.ProductImages.Find(int.Parse(productImages[i]));
                    //if there is nothing stored then we need to add a new mapping
                    if (imageMappingToEdit == null)
                    {
                        //add image to the imagemappings
                        productToUpdate.ProductImageMappings.Add(new ProductImageMapping
                        {
                            ImageNumber = i,
                            ProductImage = image,
                            ProductImageID = image.ID
                        });
                    }
                    //else it's not a new file so edit the current mapping
                    else
                    {
                        //if they are not the same
                        if (imageMappingToEdit.ProductImageID != int.Parse(productImages[i]))
                        {
                            //assign image property of the mapping
                            imageMappingToEdit.ProductImage = image;
                        }
                    }
                }
                //delete any other imagemappings that the user did not include in their selections for the product
                for (int i = productImages.Length; i < Constants.NumberOfProductImages; i++)
                {
                    var imageMappingToEdit = productToUpdate.ProductImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    //if there is something stored in the mapping
                    if (imageMappingToEdit != null)
                    {
                        //delete the record from the mapping table directly.
                        //just calling productToUpdate.ProductImageMappings.Remove(imageMappingToEdit)
                        //results in a FK error
                        db.ProductImageMappings.Remove(imageMappingToEdit);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);

            //Prevent foreign key violation
            var orderLines = db.OrderLines.Where(ol => ol.ProductID == id);
            foreach (var ol in orderLines)
            {
                ol.ProductID = null;
            }
            //
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
