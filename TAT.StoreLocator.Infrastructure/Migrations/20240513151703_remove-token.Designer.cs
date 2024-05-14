﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240513151703_remove-token")]
    partial class removetoken
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ProviderKey", "LoginProvider");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Address", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("District")
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .HasColumnType("text");

                    b.Property<string>("Province")
                        .HasColumnType("text");

                    b.Property<string>("RoadName")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Ward")
                        .HasColumnType("text");

                    b.Property<decimal>("latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal>("longitude")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("GalleryId")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ParentCategoryId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GalleryId")
                        .IsUnique();

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Gallery", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("FileBelongsTo")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<bool>("IsThumbnail")
                        .HasColumnType("boolean");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<string>("PublicId")
                        .HasColumnType("text");

                    b.Property<string>("StoreId")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("StoreId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Galleries");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.MapGalleryProduct", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<string>("GalleryId")
                        .HasColumnType("text");

                    b.HasKey("ProductId", "GalleryId");

                    b.HasIndex("GalleryId");

                    b.ToTable("MapGalleryProducts");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.MapGalleryStore", b =>
                {
                    b.Property<string>("StoreId")
                        .HasColumnType("text");

                    b.Property<string>("GalleryId")
                        .HasColumnType("text");

                    b.HasKey("StoreId", "GalleryId");

                    b.HasIndex("GalleryId");

                    b.ToTable("MapGalleryStores");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.MapProductWishlist", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<string>("WishlistId")
                        .HasColumnType("text");

                    b.HasKey("ProductId", "WishlistId");

                    b.HasIndex("WishlistId");

                    b.ToTable("MapProductWishlists");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.MapStoreWishlist", b =>
                {
                    b.Property<string>("StoreId")
                        .HasColumnType("text");

                    b.Property<string>("WishlistId")
                        .HasColumnType("text");

                    b.HasKey("StoreId", "WishlistId");

                    b.HasIndex("WishlistId");

                    b.ToTable("MapStoreWishlists");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CategoryId")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<decimal>("Discount")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MetaDescription")
                        .HasColumnType("text");

                    b.Property<string>("MetaTitle")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("ProductViewCount")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision");

                    b.Property<string>("SKU")
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<string>("StoreId")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StoreId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Review", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<int>("RatingValue")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Store", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AddressId")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("AddressId")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Dob")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdateBy")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Wishlist", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Wishlists");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TAT.StoreLocator.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Category", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Gallery", "Gallery")
                        .WithOne("Category")
                        .HasForeignKey("TAT.StoreLocator.Core.Entities.Category", "GalleryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TAT.StoreLocator.Core.Entities.Category", "ParentCategory")
                        .WithMany("ChildrenCategories")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gallery");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Gallery", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Product", null)
                        .WithMany("Galleries")
                        .HasForeignKey("ProductId");

                    b.HasOne("TAT.StoreLocator.Core.Entities.Store", null)
                        .WithMany("Galleries")
                        .HasForeignKey("StoreId");

                    b.HasOne("TAT.StoreLocator.Core.Entities.User", "User")
                        .WithOne("Gallery")
                        .HasForeignKey("TAT.StoreLocator.Core.Entities.Gallery", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.MapGalleryProduct", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Gallery", "Gallery")
                        .WithMany("MapGalleryProducts")
                        .HasForeignKey("GalleryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TAT.StoreLocator.Core.Entities.Product", "Product")
                        .WithMany("MapGalleryProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gallery");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.MapGalleryStore", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Gallery", "Gallery")
                        .WithMany("MapGalleryStores")
                        .HasForeignKey("GalleryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TAT.StoreLocator.Core.Entities.Store", "Store")
                        .WithMany("MapGalleryStores")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gallery");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.MapProductWishlist", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Product", "Product")
                        .WithMany("MapProductWishlists")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TAT.StoreLocator.Core.Entities.Wishlist", "Wishlist")
                        .WithMany("MapProductWishlists")
                        .HasForeignKey("WishlistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Wishlist");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.MapStoreWishlist", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Store", "Store")
                        .WithMany("MapStoreWishlists")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TAT.StoreLocator.Core.Entities.Wishlist", "Wishlist")
                        .WithMany("MapStoreWishlists")
                        .HasForeignKey("WishlistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");

                    b.Navigation("Wishlist");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Product", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TAT.StoreLocator.Core.Entities.Store", "Store")
                        .WithMany("Products")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Category");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Review", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TAT.StoreLocator.Core.Entities.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Store", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Address", "Address")
                        .WithOne("Store")
                        .HasForeignKey("TAT.StoreLocator.Core.Entities.Store", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Address");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.User", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.Address", "Address")
                        .WithOne("User")
                        .HasForeignKey("TAT.StoreLocator.Core.Entities.User", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Address");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Wishlist", b =>
                {
                    b.HasOne("TAT.StoreLocator.Core.Entities.User", "User")
                        .WithMany("Wishlists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Address", b =>
                {
                    b.Navigation("Store");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Category", b =>
                {
                    b.Navigation("ChildrenCategories");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Gallery", b =>
                {
                    b.Navigation("Category");

                    b.Navigation("MapGalleryProducts");

                    b.Navigation("MapGalleryStores");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Product", b =>
                {
                    b.Navigation("Galleries");

                    b.Navigation("MapGalleryProducts");

                    b.Navigation("MapProductWishlists");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Store", b =>
                {
                    b.Navigation("Galleries");

                    b.Navigation("MapGalleryStores");

                    b.Navigation("MapStoreWishlists");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.User", b =>
                {
                    b.Navigation("Gallery");

                    b.Navigation("Reviews");

                    b.Navigation("Wishlists");
                });

            modelBuilder.Entity("TAT.StoreLocator.Core.Entities.Wishlist", b =>
                {
                    b.Navigation("MapProductWishlists");

                    b.Navigation("MapStoreWishlists");
                });
#pragma warning restore 612, 618
        }
    }
}
