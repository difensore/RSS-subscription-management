using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class RssSubscriptionManagementContext : DbContext
{
    public RssSubscriptionManagementContext()
    {
    }

    public RssSubscriptionManagementContext(DbContextOptions<RssSubscriptionManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FeedItem> FeedItems { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Rssfeed> Rssfeeds { get; set; }  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeedItem>(entity =>
        {
            entity.ToTable("FeedItem");

            entity.Property(e => e.FeedId).HasMaxLength(450);
            entity.Property(e => e.ItemId).HasMaxLength(450);

            entity.HasOne(d => d.Feed).WithMany(p => p.FeedItems)
                .HasForeignKey(d => d.FeedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeedItem_RSSFeed");

            entity.HasOne(d => d.Item).WithMany(p => p.FeedItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeedItem_Item");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");

            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Link).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Rssfeed>(entity =>
        {
            entity.ToTable("RSSFeed");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Link).HasMaxLength(50);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
