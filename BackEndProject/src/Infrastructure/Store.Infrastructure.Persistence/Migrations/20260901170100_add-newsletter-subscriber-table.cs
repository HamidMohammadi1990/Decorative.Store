using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class add_newsletter_subscriber_table : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            IF OBJECT_ID(N'[dbo].[NewsletterSubscriber]', N'U') IS NULL
            BEGIN
                CREATE TABLE [NewsletterSubscriber] (
                    [Id] int NOT NULL IDENTITY,
                    [Email] VARCHAR(256) NOT NULL,
                    [LanguageId] int NOT NULL,
                    [SubscribedAtUtc] datetime2 NOT NULL,
                    [IsActive] bit NOT NULL,
                    CONSTRAINT [PK_NewsletterSubscriber] PRIMARY KEY ([Id]),
                    CONSTRAINT [FK_NewsletterSubscriber_Language_LanguageId] FOREIGN KEY ([LanguageId]) REFERENCES [Language] ([Id]) ON DELETE NO ACTION
                );

                CREATE UNIQUE INDEX [IX_NewsletterSubscriber_Email] ON [NewsletterSubscriber] ([Email]);
                CREATE INDEX [IX_NewsletterSubscriber_LanguageId_SubscribedAtUtc] ON [NewsletterSubscriber] ([LanguageId], [SubscribedAtUtc]);
            END
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "NewsletterSubscriber");
    }
}
