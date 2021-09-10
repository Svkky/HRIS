using System.Collections.Generic;

namespace HRIS.Application.AppContants
{
    public class ApplicationConstant
    {
        public static string SuccessResponseCode = "00";
        public static string FailureResponse = "-1";
        public static int SuccessStatusCode = 200;
        public static int NotFoundStatusCode = 404;
        public static int NotAuthenticatedStatusCode = 401;
        public static int BadRequestStatusCode = 400;


        public static string Sp_Category = "Sp_Category";
        public static string Sp_Product = "Sp_Product";
        public static string Sp_Expenditure = "Sp_Expenditure";
        public static string Sp_Dashboard = "Sp_Dashboard";
        public static string Sp_StoreSetup = "Sp_StoreSetup";
        public static string Sp_MenuSetup = "Sp_MenuSetup";
        public static string Sp_SubCategory = "Sp_SubCategory";
        public static string Sp_ProductTypeVariation = "Sp_ProductTypeVariation";
        public static string Sp_Branch = "Sp_Branch";
        public static string Sp_AllocateProduct = "Sp_AllocateProduct";
        public static string Sp_CustomerVoucher = "Sp_CustomerVoucher";
        public static string Sp_Supplier = "Sp_Supplier";
        public static string Sp_Customer = "Sp_Customer";
        public static string Sp_BranchAdmin = "Sp_BranchAdmin";

        public static string SP_vat = "SP_vat";
        public static string SP_Voucher = "SP_Voucher";
        public static string SP_AssignPermissionToUser = "SP_ProcessMenuToUsers";
        public static string SP_Sales = "SP_Sales";
        public static string SP_SalesDetail = "SP_SalesDetail";
        public static string Sp_StoreProduct = "Sp_StoreProduct";
        // public static string Sp_AllocateProduct = "Sp_AllocateProduct";


        public static List<string> StoreAdminPages()
        {
            var menuIds = new List<string>
            {
                 "StoreSETUPParentStoreAdmin","SetUpBranchPARENT", "SetupCategory","SetupSubCategory","StoreProduct","SetupClientChild","SetupVendorPayment","ProductAllocation","ProductWarehouse","SalesStoreAdmin",
            };
            return menuIds;
        }
        public static List<string> BranchAdminPages()
        {
            var menuIds = new List<string>
            {
                 "SetupProductVariationParent","SetupVoucher","AssignVoucherToCustomer","VatManagement","SetupProductParent",
                 "SetupCustomerChild","PosChild","Expenditure","SetupUsers","SalesReport"
            };
            return menuIds;
        }
    }
}
