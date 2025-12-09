using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSGTS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateTr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IscilikUcretleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HizmetTuru = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SaatlikUcret = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IscilikUcretleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategoriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriAdi = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Markalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkaAdi = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Eposta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VergiNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OdemeTipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdemeTipleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServisDurumlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DurumAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RenkKodu = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServisDurumlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YedekParcalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParcaAdi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ParcaKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StokMiktari = table.Column<int>(type: "int", nullable: false),
                    BirimFiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KritikSeviye = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YedekParcalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modeller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkaId = table.Column<int>(type: "int", nullable: false),
                    KategoriId = table.Column<int>(type: "int", nullable: false),
                    ModelAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modeller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modeller_Kategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Modeller_Markalar_MarkaId",
                        column: x => x.MarkaId,
                        principalTable: "Markalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SifreOzeti = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Eposta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kullanicilar_Roller_RolId",
                        column: x => x.RolId,
                        principalTable: "Roller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cihazlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MarkaId = table.Column<int>(type: "int", nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    SatinAlmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GarantiBitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cihazlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cihazlar_Markalar_MarkaId",
                        column: x => x.MarkaId,
                        principalTable: "Markalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cihazlar_Modeller_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Modeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServisKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusteriId = table.Column<int>(type: "int", nullable: false),
                    CihazId = table.Column<int>(type: "int", nullable: false),
                    AcanKullaniciId = table.Column<int>(type: "int", nullable: false),
                    DurumId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServisKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServisKayitlari_Cihazlar_CihazId",
                        column: x => x.CihazId,
                        principalTable: "Cihazlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServisKayitlari_Kullanicilar_AcanKullaniciId",
                        column: x => x.AcanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServisKayitlari_Musteriler_MusteriId",
                        column: x => x.MusteriId,
                        principalTable: "Musteriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServisKayitlari_ServisDurumlari_DurumId",
                        column: x => x.DurumId,
                        principalTable: "ServisDurumlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Atamalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KayitId = table.Column<int>(type: "int", nullable: false),
                    TeknisyenId = table.Column<int>(type: "int", nullable: false),
                    AtamaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atamalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atamalar_Kullanicilar_TeknisyenId",
                        column: x => x.TeknisyenId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atamalar_ServisKayitlari_KayitId",
                        column: x => x.KayitId,
                        principalTable: "ServisKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Faturalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KayitId = table.Column<int>(type: "int", nullable: false),
                    ToplamTutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Indirim = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KDV = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetTutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FaturaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faturalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faturalar_ServisKayitlari_KayitId",
                        column: x => x.KayitId,
                        principalTable: "ServisKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IslemLoglari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KayitId = table.Column<int>(type: "int", nullable: false),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    IslemAciklamasi = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ZamanDamgasi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IslemLoglari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IslemLoglari_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IslemLoglari_ServisKayitlari_KayitId",
                        column: x => x.KayitId,
                        principalTable: "ServisKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServisParcaKullanimlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KayitId = table.Column<int>(type: "int", nullable: false),
                    ParcaId = table.Column<int>(type: "int", nullable: false),
                    Miktar = table.Column<int>(type: "int", nullable: false),
                    KullanimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServisParcaKullanimlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServisParcaKullanimlari_ServisKayitlari_KayitId",
                        column: x => x.KayitId,
                        principalTable: "ServisKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServisParcaKullanimlari_YedekParcalar_ParcaId",
                        column: x => x.ParcaId,
                        principalTable: "YedekParcalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Odemeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaturaId = table.Column<int>(type: "int", nullable: false),
                    OdemeTipiId = table.Column<int>(type: "int", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odemeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odemeler_Faturalar_FaturaId",
                        column: x => x.FaturaId,
                        principalTable: "Faturalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Odemeler_OdemeTipleri_OdemeTipiId",
                        column: x => x.OdemeTipiId,
                        principalTable: "OdemeTipleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atamalar_KayitId",
                table: "Atamalar",
                column: "KayitId");

            migrationBuilder.CreateIndex(
                name: "IX_Atamalar_TeknisyenId",
                table: "Atamalar",
                column: "TeknisyenId");

            migrationBuilder.CreateIndex(
                name: "IX_Cihazlar_MarkaId",
                table: "Cihazlar",
                column: "MarkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cihazlar_ModelId",
                table: "Cihazlar",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cihazlar_SeriNo",
                table: "Cihazlar",
                column: "SeriNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faturalar_KayitId",
                table: "Faturalar",
                column: "KayitId");

            migrationBuilder.CreateIndex(
                name: "IX_IslemLoglari_KayitId",
                table: "IslemLoglari",
                column: "KayitId");

            migrationBuilder.CreateIndex(
                name: "IX_IslemLoglari_KullaniciId",
                table: "IslemLoglari",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_KullaniciAdi",
                table: "Kullanicilar",
                column: "KullaniciAdi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_RolId",
                table: "Kullanicilar",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Modeller_KategoriId",
                table: "Modeller",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Modeller_MarkaId",
                table: "Modeller",
                column: "MarkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_VergiNo",
                table: "Musteriler",
                column: "VergiNo",
                unique: true,
                filter: "[VergiNo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Odemeler_FaturaId",
                table: "Odemeler",
                column: "FaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Odemeler_OdemeTipiId",
                table: "Odemeler",
                column: "OdemeTipiId");

            migrationBuilder.CreateIndex(
                name: "IX_ServisKayitlari_AcanKullaniciId",
                table: "ServisKayitlari",
                column: "AcanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_ServisKayitlari_CihazId",
                table: "ServisKayitlari",
                column: "CihazId");

            migrationBuilder.CreateIndex(
                name: "IX_ServisKayitlari_DurumId",
                table: "ServisKayitlari",
                column: "DurumId");

            migrationBuilder.CreateIndex(
                name: "IX_ServisKayitlari_MusteriId",
                table: "ServisKayitlari",
                column: "MusteriId");

            migrationBuilder.CreateIndex(
                name: "IX_ServisParcaKullanimlari_KayitId",
                table: "ServisParcaKullanimlari",
                column: "KayitId");

            migrationBuilder.CreateIndex(
                name: "IX_ServisParcaKullanimlari_ParcaId",
                table: "ServisParcaKullanimlari",
                column: "ParcaId");

            migrationBuilder.CreateIndex(
                name: "IX_YedekParcalar_ParcaKodu",
                table: "YedekParcalar",
                column: "ParcaKodu",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atamalar");

            migrationBuilder.DropTable(
                name: "IscilikUcretleri");

            migrationBuilder.DropTable(
                name: "IslemLoglari");

            migrationBuilder.DropTable(
                name: "Odemeler");

            migrationBuilder.DropTable(
                name: "ServisParcaKullanimlari");

            migrationBuilder.DropTable(
                name: "Faturalar");

            migrationBuilder.DropTable(
                name: "OdemeTipleri");

            migrationBuilder.DropTable(
                name: "YedekParcalar");

            migrationBuilder.DropTable(
                name: "ServisKayitlari");

            migrationBuilder.DropTable(
                name: "Cihazlar");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Musteriler");

            migrationBuilder.DropTable(
                name: "ServisDurumlari");

            migrationBuilder.DropTable(
                name: "Modeller");

            migrationBuilder.DropTable(
                name: "Roller");

            migrationBuilder.DropTable(
                name: "Kategoriler");

            migrationBuilder.DropTable(
                name: "Markalar");
        }
    }
}
