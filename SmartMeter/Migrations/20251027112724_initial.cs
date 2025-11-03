using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartMeter.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    aid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    houseno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    lanelocality = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    state = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    pincode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("address_pkey", x => x.aid);
                });

            migrationBuilder.CreateTable(
                name: "orgunit",
                columns: table => new
                {
                    orgunitid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    parentid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orgunit_pkey", x => x.orgunitid);
                    table.ForeignKey(
                        name: "orgunit_parentid_fkey",
                        column: x => x.parentid,
                        principalTable: "orgunit",
                        principalColumn: "orgunitid");
                });

            migrationBuilder.CreateTable(
                name: "tariff",
                columns: table => new
                {
                    tariffid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    effectivefrom = table.Column<DateOnly>(type: "date", nullable: false),
                    effectiveto = table.Column<DateOnly>(type: "date", nullable: true),
                    baserate = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    taxrate = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tariff_pkey", x => x.tariffid);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    passwordhash = table.Column<byte[]>(type: "bytea", nullable: false),
                    displayname = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    lastloginutc = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "consumer",
                columns: table => new
                {
                    consumerid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    aid = table.Column<long>(type: "bigint", nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    orgunitid = table.Column<int>(type: "integer", nullable: false),
                    tariffid = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValueSql: "'Active'::character varying"),
                    createdat = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "'system'::character varying"),
                    updatedat = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true),
                    updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("consumer_pkey", x => x.consumerid);
                    table.ForeignKey(
                        name: "consumer_aid_fkey",
                        column: x => x.aid,
                        principalTable: "address",
                        principalColumn: "aid");
                    table.ForeignKey(
                        name: "consumer_orgunitid_fkey",
                        column: x => x.orgunitid,
                        principalTable: "orgunit",
                        principalColumn: "orgunitid");
                    table.ForeignKey(
                        name: "consumer_tariffid_fkey",
                        column: x => x.tariffid,
                        principalTable: "tariff",
                        principalColumn: "tariffid");
                });

            migrationBuilder.CreateTable(
                name: "tariffslab",
                columns: table => new
                {
                    tariffslabid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tariffid = table.Column<int>(type: "integer", nullable: false),
                    fromkwh = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    tokwh = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    rateperkwh = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tariffslab_pkey", x => x.tariffslabid);
                    table.ForeignKey(
                        name: "tariffslab_tariffid_fkey",
                        column: x => x.tariffid,
                        principalTable: "tariff",
                        principalColumn: "tariffid");
                });

            migrationBuilder.CreateTable(
                name: "todrule",
                columns: table => new
                {
                    todruleid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tariffid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    starttime = table.Column<TimeOnly>(type: "time(0) without time zone", precision: 0, nullable: false),
                    endtime = table.Column<TimeOnly>(type: "time(0) without time zone", precision: 0, nullable: false),
                    rateperkwh = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("todrule_pkey", x => x.todruleid);
                    table.ForeignKey(
                        name: "todrule_tariffid_fkey",
                        column: x => x.tariffid,
                        principalTable: "tariff",
                        principalColumn: "tariffid");
                });

            migrationBuilder.CreateTable(
                name: "meter",
                columns: table => new
                {
                    meterserialno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ipaddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    iccid = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    imsi = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    manufacturer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    firmware = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    installtsutc = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValueSql: "'Active'::character varying"),
                    consumerid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("meter_pkey", x => x.meterserialno);
                    table.ForeignKey(
                        name: "meter_consumerid_fkey",
                        column: x => x.consumerid,
                        principalTable: "consumer",
                        principalColumn: "consumerid");
                });

            migrationBuilder.CreateTable(
                name: "tariffdetails",
                columns: table => new
                {
                    tariffdetailsid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tariffid = table.Column<int>(type: "integer", nullable: false),
                    tariffslabid = table.Column<int>(type: "integer", nullable: false),
                    tarifftodid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tariffdetails_pkey", x => x.tariffdetailsid);
                    table.ForeignKey(
                        name: "tariffdetails_tariffid_fkey",
                        column: x => x.tariffid,
                        principalTable: "tariff",
                        principalColumn: "tariffid");
                    table.ForeignKey(
                        name: "tariffdetails_tariffslabid_fkey",
                        column: x => x.tariffslabid,
                        principalTable: "tariffslab",
                        principalColumn: "tariffslabid");
                    table.ForeignKey(
                        name: "tariffdetails_tarifftodid_fkey",
                        column: x => x.tarifftodid,
                        principalTable: "todrule",
                        principalColumn: "todruleid");
                });

            migrationBuilder.CreateTable(
                name: "billing",
                columns: table => new
                {
                    billid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consumerid = table.Column<long>(type: "bigint", nullable: false),
                    meterid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    billingperiodstart = table.Column<DateOnly>(type: "date", nullable: false),
                    billingperiodend = table.Column<DateOnly>(type: "date", nullable: false),
                    totalunitsconsumed = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    baseamount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    taxamount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    totalamount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true, computedColumnSql: "(baseamount + taxamount)", stored: true),
                    generatedat = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false, defaultValueSql: "now()"),
                    duedate = table.Column<DateOnly>(type: "date", nullable: false),
                    paiddate = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: true),
                    paymentstatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValueSql: "'Unpaid'::character varying"),
                    disconnectiondate = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("billing_pkey", x => x.billid);
                    table.ForeignKey(
                        name: "billing_consumerid_fkey",
                        column: x => x.consumerid,
                        principalTable: "consumer",
                        principalColumn: "consumerid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "billing_meterid_fkey",
                        column: x => x.meterid,
                        principalTable: "meter",
                        principalColumn: "meterserialno",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meterreading",
                columns: table => new
                {
                    meterreadingid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    meterid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    meterreadingdate = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false),
                    energyconsumed = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    voltage = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: false),
                    current = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("meterreading_pkey", x => x.meterreadingid);
                    table.ForeignKey(
                        name: "meterreading_meterid_fkey",
                        column: x => x.meterid,
                        principalTable: "meter",
                        principalColumn: "meterserialno");
                });

            migrationBuilder.CreateTable(
                name: "arrears",
                columns: table => new
                {
                    aid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consumerid = table.Column<long>(type: "bigint", nullable: false),
                    atype = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    paidstatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValueSql: "'Unpaid'::character varying"),
                    billid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("arrears_pkey", x => x.aid);
                    table.ForeignKey(
                        name: "arrears_billid_fkey",
                        column: x => x.billid,
                        principalTable: "billing",
                        principalColumn: "billid");
                    table.ForeignKey(
                        name: "arrears_consumerid_fkey",
                        column: x => x.consumerid,
                        principalTable: "consumer",
                        principalColumn: "consumerid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_arrears_billid",
                table: "arrears",
                column: "billid");

            migrationBuilder.CreateIndex(
                name: "IX_arrears_consumerid",
                table: "arrears",
                column: "consumerid");

            migrationBuilder.CreateIndex(
                name: "IX_billing_consumerid",
                table: "billing",
                column: "consumerid");

            migrationBuilder.CreateIndex(
                name: "IX_billing_meterid",
                table: "billing",
                column: "meterid");

            migrationBuilder.CreateIndex(
                name: "IX_consumer_aid",
                table: "consumer",
                column: "aid");

            migrationBuilder.CreateIndex(
                name: "IX_consumer_orgunitid",
                table: "consumer",
                column: "orgunitid");

            migrationBuilder.CreateIndex(
                name: "IX_consumer_tariffid",
                table: "consumer",
                column: "tariffid");

            migrationBuilder.CreateIndex(
                name: "IX_meter_consumerid",
                table: "meter",
                column: "consumerid");

            migrationBuilder.CreateIndex(
                name: "IX_meterreading_meterid",
                table: "meterreading",
                column: "meterid");

            migrationBuilder.CreateIndex(
                name: "IX_orgunit_parentid",
                table: "orgunit",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "ix_orgunit_type_including",
                table: "orgunit",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_tariffdetails_tariffid",
                table: "tariffdetails",
                column: "tariffid");

            migrationBuilder.CreateIndex(
                name: "IX_tariffdetails_tariffslabid",
                table: "tariffdetails",
                column: "tariffslabid");

            migrationBuilder.CreateIndex(
                name: "IX_tariffdetails_tarifftodid",
                table: "tariffdetails",
                column: "tarifftodid");

            migrationBuilder.CreateIndex(
                name: "IX_tariffslab_tariffid",
                table: "tariffslab",
                column: "tariffid");

            migrationBuilder.CreateIndex(
                name: "ix_todrule_name",
                table: "todrule",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_todrule_tariffid",
                table: "todrule",
                column: "tariffid");

            migrationBuilder.CreateIndex(
                name: "User_username_key",
                table: "User",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "arrears");

            migrationBuilder.DropTable(
                name: "meterreading");

            migrationBuilder.DropTable(
                name: "tariffdetails");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "billing");

            migrationBuilder.DropTable(
                name: "tariffslab");

            migrationBuilder.DropTable(
                name: "todrule");

            migrationBuilder.DropTable(
                name: "meter");

            migrationBuilder.DropTable(
                name: "consumer");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "orgunit");

            migrationBuilder.DropTable(
                name: "tariff");
        }
    }
}
