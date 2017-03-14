using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.vshop
{
    public class ProductDistributor : AdminPage
    {
        private int ProductId;
        protected System.Web.UI.WebControls.Literal productName;
        protected HtmlInputHidden into;
        protected HtmlInputHidden mainId;
        protected HtmlInputHidden cProductId;
        protected HtmlInputHidden cDistributor;
        protected HtmlInputHidden cPrice;
        
        protected void btnAddButton_Click(object sender, System.EventArgs e)
        {
            //�õ��������Դ
            DataTable dataBase = DataBaseHelper.GetDataTable("Hishop_ProductDistributor", string.Format("ProductId ='{0}'", ProductId));
            //��ȡ��ǰ�����������
            dataBase.PrimaryKey = new DataColumn[] { dataBase.Columns["id"] };
            //�±��¡�ɱ�ļܹ�
            DataTable newDt = dataBase.Clone();
            //��ȡ��ǰ�����������
            newDt.PrimaryKey = new DataColumn[] { newDt.Columns["id"] };
             //��ȡ�������ֵ
            string strUserInput = into.Value;
            //�������ֵȥ�ֺ�
            string[] strUserInputTrs = strUserInput.Split(';');
            //����������ֵ��Ϊ��
            if (into.Value != "")
            {
                //ѭ��
                foreach (string UserInputTrs in strUserInputTrs)
                {
                    //ȥ����
                    string[] sUserInputTr = UserInputTrs.Split(',');
                    //������ͬ���ܵı�
                    DataRow drNew = newDt.NewRow();
                    //��Ԫ����� ���sUserInputTr[0]����Ϊ�� �������µ�Guid 
                    drNew["id"] = (string.IsNullOrEmpty(sUserInputTr[0])) ? Guid.NewGuid() : new Guid(sUserInputTr[0]);
                    drNew["ProductId"] = ProductId;
                    drNew["Distributor"] = sUserInputTr[1];
                    drNew["Price"] = sUserInputTr[2];
                    newDt.Rows.Add(drNew);
                }            
            }
            //�������������ɵ�Datatable ���������ύ
            dataBase = DataBaseHelper.GetDtDifferent(dataBase, newDt);

            if (DataBaseHelper.CommitDataTable(dataBase, "Select * From Hishop_ProductDistributor") != -1)
            {
                this.ShowMsgAndReUrl("����ɹ���", true, "ProductDistributor.aspx?ProductId=" + ProductId);
            }   
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            ProductId = Convert.ToInt32(base.Request.QueryString["ProductId"]);
            if (!this.Page.IsPostBack)
            {
                this.pageInit();
                this.GetHishop_ProductsId(ProductId);
            }            
        }
        private void GetHishop_ProductsId(int ProductId)
        {
            ProductQuery query = ProductHelper.GetHishop_ProductsId(ProductId);
            this.productName.Text = query.ProductName;
        }
        private void pageInit()
        {
          //�õ�ԭʼ����
            DataTable dataBase = DataBaseHelper.GetDataTable("Hishop_ProductDistributor", string.Format("ProductId ='{0}'", ProductId));
            foreach (DataRow row in dataBase.Rows)
            {             
                mainId.Value += row["id"].ToString() + ";";
                cProductId.Value += row["ProductId"].ToString() + ";";
                cDistributor.Value += row["Distributor"].ToString() + ";";
                cPrice.Value += row["Price"].ToString() + ";";
            }
                mainId.Value = mainId.Value.TrimEnd(';');
                cProductId.Value = cProductId.Value.TrimEnd(';');
                cDistributor.Value = cDistributor.Value.TrimEnd(';');
                cPrice.Value = cPrice.Value.TrimEnd(';');
        }
    }
}
