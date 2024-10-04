using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTrades.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DefinedTradeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Trades",
                newName: "Quantity");

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDateTime",
                table: "Trades",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "EntryPrice",
                table: "Trades",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExitDateTime",
                table: "Trades",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "ExitPrice",
                table: "Trades",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "TradeDirection",
                table: "Trades",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TradeType",
                table: "Trades",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TradedStock",
                table: "Trades",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryDateTime",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "EntryPrice",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ExitDateTime",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ExitPrice",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TradeDirection",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TradeType",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TradedStock",
                table: "Trades");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Trades",
                newName: "Amount");
        }
    }
}
