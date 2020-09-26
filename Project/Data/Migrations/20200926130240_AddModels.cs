using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class AddModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Content = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 200, nullable: false),
                    URL = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TimeStarted = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    TimeFinished = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<byte>(nullable: false),
                    OriginalPrice = table.Column<double>(nullable: false),
                    FinalPrice = table.Column<double>(nullable: false),
                    Plan = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IP = table.Column<string>(maxLength: 15, nullable: false),
                    Cores = table.Column<byte>(nullable: false),
                    RAM = table.Column<byte>(nullable: false),
                    SSD = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(maxLength: 50, nullable: false),
                    Message = table.Column<string>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    ParentId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Posts_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromoCode",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<float>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(maxLength: 50, nullable: false),
                    Message = table.Column<string>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VPSs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Plan = table.Column<int>(nullable: false),
                    Location = table.Column<int>(nullable: false),
                    IP = table.Column<string>(maxLength: 15, nullable: false),
                    Cores = table.Column<byte>(nullable: false),
                    RAM = table.Column<byte>(nullable: false),
                    SSD = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VPSs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VPSs_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VPSs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodePlans",
                columns: table => new
                {
                    PromoCodeId = table.Column<string>(nullable: false),
                    PlanNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodePlans", x => x.PromoCodeId);
                    table.ForeignKey(
                        name: "FK_PromoCodePlans_PromoCode_PromoCodeId",
                        column: x => x.PromoCodeId,
                        principalTable: "PromoCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPromoCode",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    PromoCodeId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPromoCode", x => new { x.UserId, x.PromoCodeId });
                    table.ForeignKey(
                        name: "FK_UserPromoCode_PromoCode_PromoCodeId",
                        column: x => x.PromoCodeId,
                        principalTable: "PromoCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPromoCode_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Message = table.Column<string>(maxLength: 100, nullable: false),
                    URL = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    UserId = table.Column<string>(nullable: true),
                    VPSId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_VPSs_VPSId",
                        column: x => x.VPSId,
                        principalTable: "VPSs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CPU = table.Column<float>(nullable: false),
                    RAM = table.Column<float>(nullable: false),
                    VPSId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_VPSs_VPSId",
                        column: x => x.VPSId,
                        principalTable: "VPSs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserId",
                table: "Activities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_VPSId",
                table: "Activities",
                column: "VPSId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Number",
                table: "Plans",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ParentId",
                table: "Posts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_States_VPSId",
                table: "States",
                column: "VPSId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPromoCode_PromoCodeId",
                table: "UserPromoCode",
                column: "PromoCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_VPSs_OrderId",
                table: "VPSs",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_VPSs_UserId",
                table: "VPSs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "PromoCodePlans");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "UserPromoCode");

            migrationBuilder.DropTable(
                name: "VPSs");

            migrationBuilder.DropTable(
                name: "PromoCode");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
