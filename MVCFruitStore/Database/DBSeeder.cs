using MVCFruitStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MVCFruitStore.Database
{
    public class DBSeeder
    {
        public DBSeeder(CartContext dbcontext)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                User user1 = new User();
                user1.Id = Guid.NewGuid().ToString();
                user1.Name = "Marine";
                user1.LastName = "Lee";
                user1.Email = "marine@gmail.com";
                string hashPwd1 = MD5Hash.GetMd5Hash(md5Hash, "marine123");
                user1.Password = hashPwd1;
                dbcontext.Add(user1);

                User user2 = new User();
                user2.Id = Guid.NewGuid().ToString();
                user2.Name = "Anne";
                user2.LastName = "Lim";
                user2.Email = "anne@gmail.com";
                string hashPwd2 = MD5Hash.GetMd5Hash(md5Hash, "anne123");
                user2.Password = hashPwd2;
                dbcontext.Add(user2);

                User user3 = new User();
                user3.Id = Guid.NewGuid().ToString();
                user3.Name = "John";
                user3.LastName = "Tan";
                user3.Email = "john@gmail.com";
                string hashPwd3 = MD5Hash.GetMd5Hash(md5Hash, "john123");
                user3.Password = hashPwd3;
                dbcontext.Add(user3);


                Product product1 = new Product();
                product1.Id = Guid.NewGuid().ToString();
                product1.Name = "Grapes";
                product1.Price = 5.50;
                product1.Description = "These grapes are sweet and juicy and has a thin firm skin.";
                product1.Image = "grape.jpg";
                dbcontext.Add(product1);

                Product product2 = new Product();
                product2.Id = Guid.NewGuid().ToString();
                product2.Name = "Apples";
                product2.Price = 4.50;
                product2.Description = "Large variety apple.Beautiful red or pinkish red skin.";
                product2.Image = "apple.jpg";
                dbcontext.Add(product2);

                Product product3 = new Product();
                product3.Id = Guid.NewGuid().ToString();
                product3.Name = "Orange";
                product3.Price = 4.30;
                product3.Description = "These mandarins are sweet, juicy, seedless and easy to peel.";
                product3.Image = "orange.jpg";
                dbcontext.Add(product3);

                Product product4 = new Product();
                product4.Id = Guid.NewGuid().ToString();
                product4.Name = "Strawberry";
                product4.Price = 5.45;
                product4.Description = "The fruit is an excellent source of vitamins C and K as well as fibre.";
                product4.Image = "strawberry.jpg";
                dbcontext.Add(product4);

                Product product5 = new Product();
                product5.Id = Guid.NewGuid().ToString();
                product5.Name = "Dragonfruit";
                product5.Price = 5.50;
                product5.Description = "The dragonfruit is a 'magical' fruit that integrates fruits, flowers.";
                product5.Image = "dragon.jpg";
                dbcontext.Add(product5);


                Product product6 = new Product();
                product6.Id = Guid.NewGuid().ToString();
                product6.Name = "Watermelon";
                product6.Price = 3.50;
                product6.Description = "A limited edition,this watermelon has much less seeds than other varieties.";
                product6.Image = "watermelon.jpg";
                dbcontext.Add(product6);
                dbcontext.SaveChanges();


                Review review11 = new Review();
                review11.Id = Guid.NewGuid().ToString();
                review11.UserId = user1.Id;
                review11.ProductId = product1.Id;
                review11.ReviewText = "Amazing quality, reliable seller. Extremely fast delivery.";
                dbcontext.Add(review11);
                dbcontext.SaveChanges();


                Review review21 = new Review();
                review21.Id = Guid.NewGuid().ToString();
                review21.UserId = user2.Id;
                review21.ProductId = product1.Id;
                review21.ReviewText = "Wonderful transaction. Fruits were of the highest quality.";
                dbcontext.Add(review21);
                dbcontext.SaveChanges();

                Review review31 = new Review();
                review31.Id = Guid.NewGuid().ToString();
                review31.UserId = user3.Id;
                review31.ProductId = product1.Id;
                review31.ReviewText = "Good product. Would buy again. Thank you MVC Fruit Store.";
                dbcontext.Add(review31);
                dbcontext.SaveChanges();


                Review review12 = new Review();
                review12.Id = Guid.NewGuid().ToString();
                review12.UserId = user1.Id;
                review12.ProductId = product2.Id;
                review12.ReviewText = "The apples were fresh and sweet.";
                dbcontext.Add(review12);
                dbcontext.SaveChanges();


                Review review22 = new Review();
                review22.Id = Guid.NewGuid().ToString();
                review22.UserId = user2.Id;
                review22.ProductId = product2.Id;
                review22.ReviewText = "Great quality. The apples are fresh.";
                dbcontext.Add(review22);
                dbcontext.SaveChanges();

                Review review32 = new Review();
                review32.Id = Guid.NewGuid().ToString();
                review32.UserId = user3.Id;
                review32.ProductId = product2.Id;
                review32.ReviewText = "Very fast delivery. Reliable seller. Will order again.";
                dbcontext.Add(review32);
                dbcontext.SaveChanges();

                Review review13 = new Review();
                review13.Id = Guid.NewGuid().ToString();
                review13.UserId = user1.Id;
                review13.ProductId = product3.Id;
                review13.ReviewText = "Great quality and taste. I will only buy from this shop from now on.";
                dbcontext.Add(review13);
                dbcontext.SaveChanges();


                Review review23 = new Review();
                review23.Id = Guid.NewGuid().ToString();
                review23.UserId = user2.Id;
                review23.ProductId = product3.Id;
                review23.ReviewText = "Wonderful transaction. Fruits were of the highest quality.";
                dbcontext.Add(review23);
                dbcontext.SaveChanges();

                Review review33 = new Review();
                review33.Id = Guid.NewGuid().ToString();
                review33.UserId = user3.Id;
                review33.ProductId = product3.Id;
                review33.ReviewText = "Very fast delivery.Reliable seller. Will order again.";
                dbcontext.Add(review33);
                dbcontext.SaveChanges();
            }


        }
    }
}
